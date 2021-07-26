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

          public AccountController(
               UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
              ITokenService tokenService, 
              IMapper mapper, IUserService userService)
          {
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
                    Token =  _tokenService.CreateToken(user),
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
               if (registerDto.UserType.ToLower() =="employee" && string.IsNullOrEmpty(registerDto.AadharNo)) {
                    return BadRequest(new ApiResponse (400, "for employees, Aadhar number is mandatory"));
               }

               if (registerDto.UserType.ToLower()=="official" && (int)registerDto.CompanyId==0) {
                    return BadRequest(new ApiResponse(400, "For officials, customer Id is essential"));
               }
               
               if (registerDto.UserPhones !=null && registerDto.UserPhones.Count() > 0) {
                    foreach(var ph in registerDto.UserPhones) {
                         if (ph.PhoneNo == "" && ph.MobileNo == "") return BadRequest(new ApiResponse(400, "either phone no or mobile no must be mentioned"));
                    }
               }

               var objPP = new UserPassport();

               if(string.IsNullOrEmpty(registerDto.PpNo)) {
                    objPP = null;
               } else {
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
               registerDto.UserPassports=lstPP;
               
               registerDto.AppUserId = userAdded.Id;

               if (registerDto.UserPhones != null)
               {    //ensure no duplicate user phones in the collection
                    var qry = (from p in registerDto.UserPhones
                               group p by p.PhoneNo into g
                               where g.Count() > 1
                               select g.Key);
                    if (qry != null) registerDto.UserPhones=null;     //disallow if any duplicate numbers
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
 

     }
}