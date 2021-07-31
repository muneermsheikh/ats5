using System.Threading.Tasks;
using api.Errors;
using api.Helpers;
using AutoMapper;
using core.Entities.Identity;
using core.Entities.Users;
using core.Interfaces;
using core.Params;
using core.ParamsAndDtos;
using core.Specifications;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
     public class CandidateController : BaseApiController
     {
          private readonly IUnitOfWork _unitOfWork;
          private readonly IMapper _mapper;
          private readonly UserManager<AppUser> _userManager;
          private readonly SignInManager<AppUser> _signInManager;
          private readonly IUserService _userService;
          private readonly IGenericRepository<Candidate> _candRepo;
          public CandidateController(IUnitOfWork unitOfWork, IMapper mapper, 
               IGenericRepository<Candidate> candRepo,
               UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
               IUserService userService)
          {
               _candRepo = candRepo;
               _userService = userService;
               _signInManager = signInManager;
               _userManager = userManager;
               _mapper = mapper;
               _unitOfWork = unitOfWork;
          }

     
     [HttpGet("canidatelist")]
     public async Task<ActionResult<Pagination<Candidate>>> GetCandidateListAsync(CandidateSpecParams candidateParam)
     {
          var spec = new CandidateSpecs(candidateParam);
          var countSpec = new CandidateForCountSpecs(candidateParam);

          var totalItems = await _candRepo.CountAsync(countSpec);
          var cands = await _candRepo.ListAsync(spec);

          //var data = _mapper.Map<IReadOnlyList<CandidateToReturnDto>>(cands);

          return Ok(new Pagination<Candidate>(candidateParam.PageIndex,
               candidateParam.PageSize, totalItems, cands));
     }

     [HttpGet("candidatebyid/{userid}")]
     public async Task<ActionResult<Candidate>> GetCandidatebyUserId(int userid)
     {
          return await _userService.GetCandidateByIdAsync(userid);
     }

     [HttpGet("candidatebyappuserid/{appuserid}")]
     public async Task<ActionResult<Candidate>> GetCandidateByAppUserid(string appUserId)
     {
         var cands = await _userService.GetCandidateBySpecsIdentityIdAsync(appUserId);
         if (cands == null) return NotFound(new ApiResponse(404));
         return Ok(cands);
     }

     
     [HttpGet("emailexists")]
     public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email)
     {
          return await _userManager.FindByEmailAsync(email) != null;
     }

     

}

}