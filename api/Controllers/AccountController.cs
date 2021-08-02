using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using api.DTOs;
using api.Errors;
using api.Extensions;
using AutoMapper;
using core.Entities.Identity;
using core.Entities.Users;
using core.Interfaces;
using core.ParamsAndDtos;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
     public class AccountController : BaseApiController
     {
          private readonly UserManager<AppUser> _userManager;
          private readonly SignInManager<AppUser> _signInManager;
          private readonly ITokenService _tokenService;
          private readonly IMapper _mapper;
          private readonly IUserService _userService;
          private readonly RoleManager<IdentityRole> _roleManager;

          public AccountController(
               UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
               RoleManager<IdentityRole> roleManager, ITokenService tokenService,
              IMapper mapper, IUserService userService)
          {
               _roleManager = roleManager;
               _userService = userService;
               _mapper = mapper;
               _tokenService = tokenService;
               _signInManager = signInManager;
               _userManager = userManager;
          }

          [Authorize]
          [HttpGet]
          public async Task<ActionResult<core.ParamsAndDtos.UserDto>> GetCurrentUser()
          {
               var user = await _userManager.FindByEmailFromClaimsPrinciple(User);
               return new core.ParamsAndDtos.UserDto
               {
                    Email = user.Email,
                    Token = _tokenService.CreateToken(user),
                    DisplayName = user.DisplayName
               };
          }

          [HttpGet("emailexists")]
          public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email)
          {
               return await _userManager.FindByEmailAsync(email) != null;
          }

          [Authorize]
          [HttpGet("address")]
          public async Task<ActionResult<AddressDto>> GetUserAddress()
          {
               var user = await _userManager.FindByEmailWithAddressAsync(User);

               return _mapper.Map<AddressDto>(user.Address);
          }

          [Authorize]
          [HttpPut("address")]
          public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto address)
          {
               var user = await _userManager.FindByEmailWithAddressAsync(User);

               user.Address = _mapper.Map<Address>(address);

               var result = await _userManager.UpdateAsync(user);

               if (result.Succeeded) return Ok(_mapper.Map<AddressDto>(user.Address));

               return BadRequest("Problem updating the user");
          }


          [HttpPost("login")]
          public async Task<ActionResult<core.ParamsAndDtos.UserDto>> Login(LoginDto loginDto)
          {
               var user = await _userManager.FindByEmailAsync(loginDto.Email);

               if (user == null) return Unauthorized(new ApiResponse(401));

               var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

               if (!result.Succeeded) return Unauthorized(new ApiResponse(401));

               return new core.ParamsAndDtos.UserDto
               {
                    Email = user.Email,
                    Token = _tokenService.CreateToken(user),
                    DisplayName = user.DisplayName
               };
          }

          //registers individuals. For customers and vendors, it will register the users for customers that exist
          [HttpPost("register")]
          public async Task<ActionResult<core.ParamsAndDtos.UserDto>> Register(RegisterDto registerDto)
          {
               if (CheckEmailExistsAsync(registerDto.Email).Result.Value)
               {
                    return new BadRequestObjectResult(new ApiValidationErrorResponse { Errors = new[] { "Email address is in use" } });
               }

               //for customer and vendor official, customer Id is mandatory
               if (registerDto.UserType.ToLower() == "employee" && string.IsNullOrEmpty(registerDto.AadharNo))
               {
                    return BadRequest(new ApiResponse(400, "for employees, Aadhar number is mandatory"));
               }

               if (registerDto.UserType.ToLower() == "official" && (int)registerDto.CompanyId == 0)
               {
                    return BadRequest(new ApiResponse(400, "For officials, customer Id is essential"));
               }

               if (registerDto.UserPhones != null && registerDto.UserPhones.Count() > 0)
               {
                    foreach (var ph in registerDto.UserPhones)
                    {
                         if (ph.PhoneNo == "" && ph.MobileNo == "") return BadRequest(new ApiResponse(400, "either phone no or mobile no must be mentioned"));
                    }
               }

               var objPP = new UserPassport();

               if (string.IsNullOrEmpty(registerDto.PpNo))
               {
                    objPP = null;
               }
               else
               {
                    objPP = new UserPassport(registerDto.PpNo, registerDto.Nationality, registerDto.PPValidity);
               }

               var user = new AppUser
               {
                    UserType = registerDto.UserType,
                    DisplayName = registerDto.DisplayName,
                    Address = registerDto.Address,
                    //UserPassport = objPP,

                    Email = registerDto.Email,
                    UserName = registerDto.Email
               };

               var result = await _userManager.CreateAsync(user, registerDto.Password);

               if (!result.Succeeded) return BadRequest(new ApiResponse(400));

               var userDtoToReturn = new core.ParamsAndDtos.UserDto
               {
                    DisplayName = user.DisplayName,
                    Token = _tokenService.CreateToken(user),
                    Email = user.Email
               };

               var userAdded = await _userManager.FindByEmailAsync(registerDto.Email);
               //user registered. 


               var lstPP = new List<UserPassport>();
               lstPP.Add(objPP);
               registerDto.UserPassports = lstPP;

               registerDto.AppUserId = userAdded.Id;

               if (registerDto.UserPhones != null)
               {    //ensure no duplicate user phones in the collection
                    var qry = (from p in registerDto.UserPhones
                               group p by p.PhoneNo into g
                               where g.Count() > 1
                               select g.Key);
                    if (qry != null) registerDto.UserPhones = null;     //disallow if any duplicate numbers
               }

               //depending upon usertype, create other entities
               switch (registerDto.UserType.ToLower())
               {
                    case "candidate":
                         await _userService.CreateCandidateAsync(registerDto);
                         break;
                    case "employee":
                         await _userService.CreateEmployeeAsync(registerDto);
                         break;
                    case "customerofficial":
                         await _userService.CreateCustomerOfficialAsync(registerDto);
                         break;
                    case "vendorofficial":
                         await _userService.CreateCustomerOfficialAsync(registerDto);
                         break;
                    default:
                         break;
               }

               return userDtoToReturn;
          }

          [HttpDelete("user/{useremail}")]
          public async Task<ActionResult<bool>> DeleteIdentityUser (string email)
          {
               
          }
//userRoles

          [HttpPost("userrole/{userEmail}/{newRole}")]
          public async Task<ActionResult<bool>> AddNewRoleToUser(string userEmail, string newRole)
          {
               var user = await _userManager.FindByEmailAsync(userEmail);
               if (user==null) {
                    return BadRequest(new ApiResponse(400, "no user with the selected email exists"));
               }
               var roleExists = await _roleManager.RoleExistsAsync(newRole);
               if (!roleExists) return BadRequest(new ApiResponse(400, "the role " + newRole + " does not exist"));

               var roleAdded = await _userManager.AddToRoleAsync(user, newRole);

               return roleAdded.Succeeded;
          }

          [HttpPut("userrole/{userEmail}/{oldRoleName}/{newRoleName}")]
          public async Task<ActionResult<bool>> EditUserRole(string userEmail, string oldRoleName, string newRoleName)
          {
               var user = await _userManager.FindByEmailAsync(userEmail);
               if (user==null) {
                    return BadRequest(new ApiResponse(400, "no user with the selected email exists"));
               }
               var roleExists = await _roleManager.RoleExistsAsync(newRoleName);
               if (!roleExists) return BadRequest(new ApiResponse(400, "the role " + newRoleName + " does not exist"));

               var roleAdded = await _userManager.RemoveFromRoleAsync(user,oldRoleName);
               if (roleAdded.Succeeded) await _userManager.AddToRoleAsync(user, newRoleName);

               return roleAdded.Succeeded;
          }

          [HttpGet("userswithgivenrole/{rolename}")]
          public async Task<ActionResult<IReadOnlyList<AppUser>>> GetIdentityUsersWithARole(string roleName)
          {
               var users = await _userManager.GetUsersInRoleAsync(roleName);
               if (users == null) return NotFound(new ApiResponse(404, "No users found with role '" + roleName + "'"));
               return Ok(users);
          }

          [HttpGet("userwithroles/{useremail}")]
          public async Task<ActionResult<IReadOnlyList<AppUserRole>>> GetIdentityUserWithRoles(string useremail)
          {
               var user = await _userManager.FindByEmailAsync(useremail);
               if (user == null) return NotFound(new ApiResponse(404, "User not found"));
               return Ok(user.UserRoles);
          }

          [HttpGet("userhastherole/{useremail}/{rolename}")]
          public async Task<ActionResult<bool>> UserHasTheRole(string useremail, string roleName)
          {
               var user = await _userManager.FindByEmailAsync(useremail);
               if (user == null) return NotFound(new ApiResponse(404, "user not found"));
               return await _userManager.IsInRoleAsync(user, roleName);
          }
          
          [HttpGet("deleteuserrole/{useremail}/{rolename}")]
          public async Task<ActionResult<bool>> DeleteUserRole(string useremail, string roleName)
          {
               var user = await _userManager.FindByEmailAsync(useremail);
               if (user == null) return NotFound(new ApiResponse(404, "user not found"));
               var result = await _userManager.RemoveFromRoleAsync(user, roleName);
               return result.Succeeded;
          }
          
//Roles
          [HttpPut("role/{existingRoleName}/{newRoleName}")]
          public async Task<ActionResult<bool>> EditRole(string existingRoleName, string newRoleName)
          {
               var role = await _roleManager.FindByNameAsync(existingRoleName);
               if (role==null) return BadRequest(new ApiResponse(400, "The requested role does not exist"));
               role.Name=newRoleName;
               if (await _roleManager.UpdateAsync(role) == null) {
                    return BadRequest(new ApiResponse(404, "failed to update the role " + existingRoleName));
               } else {
                    return Ok();
               }
          }

          [HttpPost("role/{newRole}")]
          public async Task<ActionResult<bool>> AddNewRole(string newRole)
          {
                var roleExists = await _roleManager.RoleExistsAsync(newRole);
                if (!roleExists)
                {
                    IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(newRole));
                    return Ok(true);
                } else {
                     return BadRequest(new ApiResponse(404, "the role '" + newRole + "' already exists"));
                }
          }


     }
}