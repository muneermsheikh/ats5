using System.Threading.Tasks;
using api.DTOs;
using api.Errors;
using api.Extensions;
using AutoMapper;
using core.Entities.Identity;
using core.Interfaces;
using core.ParamsAndDtos;
using infra.Services;
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
          public async Task<ActionResult<UserDto>> GetCurrentUser()
          {
               var user = await _userManager.FindByEmailFromClaimsPrinciple(User);
               return new UserDto
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
          public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
          {
               var user = await _userManager.FindByEmailAsync(loginDto.Email);

               if (user == null) return Unauthorized(new ApiResponse(401));

               var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

               if (!result.Succeeded) return Unauthorized(new ApiResponse(401));

               return new UserDto
               {
                    Email = user.Email,
                    Token = _tokenService.CreateToken(user),
                    DisplayName = user.DisplayName
               };
          }

          [HttpPost("register")]
          public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
          {
               if (CheckEmailExistsAsync(registerDto.Email).Result.Value)
               {
                    return new BadRequestObjectResult(new ApiValidationErrorResponse { Errors = new[] { "Email address is in use" } });
               }

               var user = new AppUser
               {
                    UserType = registerDto.UserType,
                    DisplayName = registerDto.DisplayName,
                    Email = registerDto.Email,
                    UserName = registerDto.Email,
               };

               var result = await _userManager.CreateAsync(user, registerDto.Password);

               if (!result.Succeeded) return BadRequest(new ApiResponse(400));

               var userDtoToReturn = new UserDto
               {
                    DisplayName = user.DisplayName,
                    Token = _tokenService.CreateToken(user),
                    Email = user.Email
               };

               //user registered. 
               //depending upon usertype, create other entities
               switch (registerDto.UserType.ToLower())
               {
                    case "candidate":
                         await _userService.CreateCandidateAsync(_mapper.Map<RegisterDto, CandidateToCreateDto>(registerDto));
                         break;
                    case "employee":
                         break;
                    case "customerofficial":
                         break;
                    case "vendorofficial":
                         break;
                    default:
                         break;
               }

               return userDtoToReturn;
          }
     }
}