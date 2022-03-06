using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using api.DTOs;
using api.Errors;
using api.Extensions;
using core.Entities.Identity;
using core.Entities.Users;
using core.Interfaces;
using core.ParamsAndDtos;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using core.Params;
using infra.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.Extensions.Configuration;
using AutoMapper;
using System.Net.Http;
using core.Entities.Attachments;
using System.Security.Claims;

namespace api.Controllers
{
     public class AccountController : BaseApiController
     {
          private readonly UserManager<AppUser> _userManager;
          private readonly SignInManager<AppUser> _signInManager;
          private readonly ITokenService _tokenService;
          private readonly IMapper _mapper;
          private readonly IUserService _userService;
          private readonly AppIdentityDbContext _identityContext;
          private readonly ITaskService _taskService;
          private readonly IEmployeeService _empService;
          private readonly IConfiguration _config;
          private readonly RoleManager<AppRole> _roleManager;
          public AccountController(
               UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
               ITokenService tokenService, RoleManager<AppRole> roleManager,
               IMapper mapper, IUserService userService, AppIdentityDbContext identityContext,
               ITaskService taskService, IEmployeeService empService, IConfiguration config)
          {
               _userService = userService ?? throw new ArgumentNullException(nameof(userService));
               _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
               _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
               _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
               _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
               _identityContext = identityContext ?? throw new ArgumentNullException(nameof(identityContext));
               _taskService = taskService ?? throw new ArgumentNullException(nameof(taskService));
               _empService = empService ?? throw new ArgumentNullException(nameof(empService));
               _config = config ?? throw new ArgumentNullException(nameof(config));
               _roleManager = roleManager;
          }


          //[Authorize]
          [HttpGet]
          public async Task<ActionResult<core.ParamsAndDtos.UserDto>> GetCurrentUser()
          {
               var user = await _userManager.FindByEmailFromClaimsPrinciple(User);
               if (user==null) return BadRequest("User Claim not found");
               return new core.ParamsAndDtos.UserDto
               {
                    Email = user.Email,
                    Token = await _tokenService.CreateToken(user),
                    DisplayName = user.DisplayName
               };
          }
          

          [HttpGet("emailexists")]
          public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email)
          {
               return await _userManager.FindByEmailAsync(email) != null;
          }
          
          [HttpGet("ppexists")]
          public async Task<ActionResult<string>> CheckPPNumberExistsAsync([FromQuery] string ppnumber)
          {
               var pp = await _userService.CheckPPNumberExists(ppnumber);
               return pp;
          }
          
          [HttpGet("aadahrexists/{aadharno}")]
          public async Task<ActionResult<bool>> CheckAadharNoExistsAsync([FromQuery] string aadharno)
          {
               return await _userService.CheckAadharNoExists(aadharno);
          }

          /*
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
          */

          [HttpPost("login")]
          public async Task<ActionResult<core.ParamsAndDtos.UserDto>> Login(LoginDto loginDto)
          {
               //var user = await _userManager.FindByEmailAsync(loginDto.Email);
               var user = await _userManager.Users.Where(x => x.Email == loginDto.Email)
                    .Include(x => x.UserRoles).ThenInclude(x => x.Role)
                    .FirstOrDefaultAsync();
               if (user == null) return Unauthorized(new ApiResponse(401));
               var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
               if (!result.Succeeded) return Unauthorized(new ApiResponse(401));
          
               //authorization
               var claims = new List<Claim>();
               claims.Add(new Claim("username", user.UserName));
               claims.Add(new Claim("displayname", user.KnownAs));
               
               // Add roles as multiple claims
               if (user.UserRoles != null) {
                    foreach(var role in user.UserRoles) 
                    {
                         claims.Add(new Claim(ClaimTypes.Role, role.Role.Name));
                    }
               }
               // Optionally add other app specific claims as needed
               //claims.Add(new Claim("UserState", UserState.ToString()));
          
          
               int loggedInEmployeeId = await _empService.GetEmployeeIdFromAppUserIdAsync(user.Id);
               //var taskParams = new TaskParams{TaskOwnerId = loggedInEmployeeId, AssignedToId = loggedInEmployeeId, TaskStatus = "Open"};

               var tasksOfLoggedInUser = await _taskService.GetDashboardTasks(loggedInEmployeeId);

               var userdto = new core.ParamsAndDtos.UserDto
               {
                    dashboardTasks = tasksOfLoggedInUser,
                    Email = user.Email,
                    Token = await _tokenService.CreateToken(user),
                    DisplayName = user.DisplayName
               };

               return userdto;
          }

          [HttpGet("users")]
          public async Task<ActionResult<ICollection<UserDto>>> GetUsers (AppUserSpecParams userParams)
          {
               var users = await _userManager.Users.ToListAsync();
               return  Ok(_mapper.Map<ICollection<UserDto>>(users));
          }

          
          //registers individuals. For customers and vendors, it will register the users for customers that exist
          //the IFormFile collection has following prefixes to filenames:
          //pp: passport; ph: photo, ec: educational certificates, qc: qualification certificates
          [HttpPost("register")]
          public async Task<ActionResult<core.ParamsAndDtos.UserDto>> Register(RegisterDto registerDto)  //, ICollection<IFormFile> UserFormFiles )
          {
               var loggedInUser = new AppUser();
               int loggedInEmployeeId =0;
               
               if(User==null && registerDto.UserType != "Candidate") return Unauthorized("Log-in required!");
               if (User!=null) {
                    loggedInUser = await _userManager.FindByEmailFromClaimsPrinciple(User);
                    loggedInEmployeeId = loggedInUser == null ? 0 : await _empService.GetEmployeeIdFromAppUserIdAsync(loggedInUser.Id);
               }
               
               //populate loggedInUser
               if (registerDto.UserType.ToLower() != "candidate") {
                    if (loggedInUser == null) return BadRequest(new ApiResponse(401, "Unauthorized"));
                    registerDto.LoggedInAppUserId = loggedInUser.Id;
               } 
               
               if (registerDto.UserType.ToLower() == "candidate") registerDto.UserRole = "Candidate";
               //check if user email already on record
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
                              if (string.IsNullOrEmpty(ph.MobileNo)) return BadRequest(new ApiResponse(400, "mobile no cannot be blank"));
                         }
                    }

               //validate passport obj
                    var objPP = new UserPassport();
                    objPP = string.IsNullOrEmpty(registerDto.PpNo) 
                         ? null 
                         : new UserPassport(registerDto.PpNo, registerDto.Nationality, registerDto.PPValidity);
                    var objAddress = registerDto.EntityAddresses;
               //create and save AppUser IdentityObject
                    var user = new AppUser
                    {
                         UserType = registerDto.UserType,
                         DisplayName = registerDto.KnownAs, // registerDto.DisplayName,
                         //Address = registerDto.Address,
                         //UserPassport = objPP,
                         KnownAs = registerDto.KnownAs,
                         Gender = registerDto.Gender,
                         PhoneNumber = registerDto.UserPhones.Where(x => x.IsMain).Select(x => x.MobileNo).FirstOrDefault(),
                         Email = registerDto.Email,
                         UserName = registerDto.Email
                    };
                    registerDto.DisplayName = registerDto.DisplayName ?? user.DisplayName;
                    registerDto.PlaceOfBirth = registerDto.PlaceOfBirth ?? "";
                    var result = await _userManager.CreateAsync(user, registerDto.Password);
                    if (!result.Succeeded) return BadRequest(result.Errors);

                    if (registerDto.UserRole != "") {
                         var succeeded = await _roleManager.CreateAsync(new AppRole{Name="Candidate"});
                         var roleResult = await _userManager.AddToRoleAsync(user, registerDto.UserRole);
                         if (!result.Succeeded) return BadRequest(result.Errors);
                    }
                    
               //the plain dto object to return, irrespective of type of user, i.e. whether candidate, employee or customer
                    var userDtoToReturn = new core.ParamsAndDtos.UserDto
                    {
                         DisplayName = user.DisplayName,
                         Token = await _tokenService.CreateToken(user),
                         Email = user.Email
                    };

                    //var userAdded = await _userManager.FindByEmailAsync(registerDto.Email);
                    //no need to retreive obj from DB - the object user can be used for the same
                    var userAdded = user;
               //user registered. 

               //now save the objects in DataContext database
                    if(objPP != null) {
                         var lstPP = new List<UserPassport>();
                         lstPP.Add(objPP);
                         registerDto.UserPassports = lstPP;
                    }

                    registerDto.AppUserId = userAdded.Id;
                    //*** flg not working..
                    /*
                    if (registerDto.UserPhones != null)
                    {    //ensure no duplicate user phones in the collection
                         var qry = (from p in registerDto.UserPhones
                                   group p by p.MobileNo into g
                                   where g.Count() > 1
                                   select g.Key);
                         if (qry != null) registerDto.UserPhones = null;     //disallow if any duplicate numbers
                    }
                    */

               //depending upon usertype, create other entities
                    switch (registerDto.UserType.ToLower())
                    {
                         case "candidate":
                              //var userCreated = await _userService.CreateCandidateAsync(registerDto, UserFormFiles, loggedInEmployeeId);
                              var userCreated = await _userService.CreateCandidateAsync(registerDto, loggedInEmployeeId);
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
               //return
                    return userDtoToReturn;
               /*
               */
          }

          [HttpPost("registeremployee")]
          public async Task<ActionResult<core.ParamsAndDtos.UserDto>> RegisterEmployee(RegisterEmployeeDto registerDto )
          {
               
               var loggedInUser = await _userManager.FindByEmailFromClaimsPrinciple(User);
               int loggedInEmployeeId = loggedInUser == null ? 0 : await _empService.GetEmployeeIdFromAppUserIdAsync(loggedInUser.Id);
               
               /* if (loggedInUser==null) {
                    loggedInUser = await _userManager.FindByEmailAsync("sanjaypatil@agenterprises.com");
               }
               */
               //populate loggedInUser
               if (loggedInUser == null) return BadRequest(new ApiResponse(401, "Unauthorized"));
               registerDto.LoggedInAppUserId = loggedInUser.Id;
               
               //check if user email already on record
                    if (CheckEmailExistsAsync(registerDto.Email).Result.Value)
                    {
                         return new BadRequestObjectResult(new ApiValidationErrorResponse { Errors = new[] { "Email address is in use" } });
                    }

               //for customer and vendor official, customer Id is mandatory
                    if (string.IsNullOrEmpty(registerDto.AadharNo))
                    {
                         return BadRequest(new ApiResponse(400, "for employees, Aadhar number is mandatory"));
                    }

                    if (registerDto.EmployeePhones != null && registerDto.EmployeePhones.Count() > 0)
                    {
                         foreach (var ph in registerDto.EmployeePhones)
                         {
                              if (string.IsNullOrEmpty(ph.MobileNo)) return BadRequest(new ApiResponse(400, "mobile no cannot be blank"));
                         }
                    }

               //update address 
               //registerDto.Address.
               //create and save AppUser IdentityObject
                    var user = new AppUser
                    {
                         UserType = "Employee",
                         DisplayName = registerDto.KnownAs, // registerDto.DisplayName,
                         //Address = registerDto.Address,
                         
                         Email = registerDto.Email,
                         UserName = registerDto.Email
                    };
                    registerDto.DisplayName = registerDto.DisplayName ?? user.DisplayName;
                    registerDto.PlaceOfBirth = registerDto.PlaceOfBirth ?? "";
                    var result = await _userManager.CreateAsync(user, registerDto.Password);
                    if (!result.Succeeded) return BadRequest(result.Errors);

                    
                    var roleResult = await _userManager.AddToRoleAsync(user, registerDto.UserRole);
                    if (!result.Succeeded) return BadRequest(result.Errors);
                    
               //the plain dto object to return, irrespective of type of user, i.e. whether candidate, employee or customer
                    var userDtoToReturn = new core.ParamsAndDtos.UserDto
                    {
                         DisplayName = user.DisplayName,
                         Token = await _tokenService.CreateToken(user),
                         Email = user.Email
                    };

                    //var userAdded = await _userManager.FindByEmailAsync(registerDto.Email);
                    //no need to retreive obj from DB - the object user can be used for the same
                    var userAdded = user;
               //user registered. 

               //now save the objects in DataContext database
                    registerDto.AppUserId = userAdded.Id;
                    //*** flg not working..
                    /*
                    if (registerDto.UserPhones != null)
                    {    //ensure no duplicate user phones in the collection
                         var qry = (from p in registerDto.UserPhones
                                   group p by p.MobileNo into g
                                   where g.Count() > 1
                                   select g.Key);
                         if (qry != null) registerDto.UserPhones = null;     //disallow if any duplicate numbers
                    }
                    */

                    await _userService.CreateEmployeeAsync(registerDto);
               //return
                    return userDtoToReturn;
               /*
               */
          }

         private async Task<string> WriteFile(IFormFile file)
          {
               //bool isSaveSuccess = false;
               string fileName;
               try
               {
                    var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                    fileName = DateTime.Now.Ticks + extension; //Create a new Name for the file due to security reasons.

                    var pathBuilt = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\files");

                    if (!Directory.Exists(pathBuilt))
                    {
                         Directory.CreateDirectory(pathBuilt);
                    }

                    var path = Path.Combine(Directory.GetCurrentDirectory(), "Upload\\files", fileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                         await file.CopyToAsync(stream);
                    }

                    //isSaveSuccess = true;
               }
               catch (Exception e)
               {
                    return e.Message;
               }

               return "";
          }



          [HttpDelete("user/{useremail}")]
          public async Task<ActionResult<bool>> DeleteIdentityUser (string useremail)
          {
               var user = await _userManager.FindByEmailAsync(useremail);
               if (user==null) {
                    return BadRequest(new ApiResponse(400, "no user with the selected email exists"));
               }
               var result = await _userManager.DeleteAsync(user);

               return result.Succeeded;
          }
//userRoles

          /*
          [Authorize]
          [HttpGet("users-with-roles")]
          public async Task<ActionResult<ICollection<AppUser>>> GetUsersWithRoles()
          {
               var users = await (from u in _identityContext.Users
                    join r in _identityContext.UserRoles on u.Id equals r.UserId
                    //join userroles in _identityContext.UserRoles on r.RoleId equals userroles.UserId
                    //join rolenames in _identityContext.Roles on userroles.RoleId equals rolenames.Id
                    orderby u.Address.FirstName
                    select new {
                         UserType = u.UserType,
                         Roles = r.RoleId,
                         Id = u.Id,
                         FirstName = u.Address.FirstName,
                         Username = u.UserName,
                         Email = u.Email
                    }).ToListAsync();
               
               
               var users = await _userManager.Users
                    //.Include(r =>r.UserRoles)
                    //.ThenInclude(r => r.Role)
                    .OrderBy(u => u.UserName)
                    
                    .Select(u => new
                    {
                         UserType = u.UserType,
                         Roles = u.UserRoles,
                         Id = u.Id,
                         FirstName = u.Address.FirstName,
                         Username = u.UserName,
                         Email = u.Email
                         
                         //RoleName = u.UserRoles.Select(x => x.Role.Name).ToList()
                         //, Roles = u.UserRoles.Select(r => r.Role.Name).ToList()
                    })
                    
                    .ToListAsync();
               
               return Ok(users);
          }

         */
     
          private async Task<bool> UserExists(string username)
          {
               return await _userManager.Users.AnyAsync(x => x.UserName == username.ToLower());
          }
     }
}