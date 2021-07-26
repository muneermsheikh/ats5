using System.Threading.Tasks;
using api.Errors;
using core.Entities;
using core.Entities.MasterEntities;
using core.Interfaces;
using core.ParamsAndDtos;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
     public class MastersController : BaseApiController
     {
          private readonly IMastersService _mastersService;
          public MastersController(IMastersService mastersService)
          {
               _mastersService = mastersService;
          }

          [HttpPost("addcategory/{categoryname}")]
          public async Task<ActionResult<Category>> AddCategory(string categoryname)
          {
               var categoryAdded = await _mastersService.AddCategory(categoryname);
               if (categoryAdded==null) return BadRequest(new ApiResponse(400, "failed to add the category"));
               return Ok(categoryAdded);
          }
          [HttpPut("editcategory")]
          public async Task<ActionResult<bool>> EditCategory(Category category)
          {
               var succeeded = await _mastersService.EditCategoryAsync(category);
               if (!succeeded) return BadRequest(new ApiResponse(400, "failed to edit the category"));
               return true;
          }

          [HttpDelete("deletecategory")]
          public async Task<ActionResult<bool>> DeleteCategory(Category category)
          {
               var succeeded = await _mastersService.DeleteCategoryAsync(category);
               if (!succeeded) return BadRequest(new ApiResponse(400, "Failed to delete the category"));
               return true;
          }

          [HttpGet("categorylist")]
          public async Task<ActionResult<Category>> GetCategoryListAsync(CategoryParams categoryParams)
          {
               var lst = await _mastersService.GetCategoryListAsync(categoryParams);
               if (lst == null) return BadRequest(new ApiResponse(400, "no data returned"));
               return Ok(lst);
          }
     //industry
          [HttpPost("addindustry/{industryname}")]
          public async Task<ActionResult<Industry>> AddIndustry(string industryname)
          {
               var industryAdded = await _mastersService.AddIndustry(industryname);
               if (industryAdded==null) return BadRequest(new ApiResponse(400, "failed to add the industry"));
               return Ok(industryAdded);
          }
          [HttpPut("editindustry")]
          public async Task<ActionResult<bool>> EditIndustry(Industry industry)
          {
               var succeeded = await _mastersService.EditIndustryAsync(industry);
               if (!succeeded) return BadRequest(new ApiResponse(400, "failed to edit the industry"));
               return true;
          }

          [HttpDelete("deleteindustry")]
          public async Task<ActionResult<bool>> DeleteCategory(Industry industry)
          {
               var succeeded = await _mastersService.DeleteIndustryyAsync(industry);
               if (!succeeded) return BadRequest(new ApiResponse(400, "Failed to delete the industry"));
               return true;
          }

          [HttpGet("industrylist")]
          public async Task<ActionResult<Industry>> GetIndustryListAsync(IndustryParams industryParams)
          {
               var lst = await _mastersService.GetIndustryListAsync(industryParams);
               if (lst == null) return BadRequest(new ApiResponse(400, "no data returned"));
               return Ok(lst);
          }

     //qualifications
          [HttpPost("addqualification/{qualificationname}")]
          public async Task<ActionResult<Qualification>> AddQualification(string qualificationname)
          {
               var qualificationAdded = await _mastersService.AddCategory(qualificationname);
               if (qualificationAdded==null) return BadRequest(new ApiResponse(400, "failed to add the qualification"));
               return Ok(qualificationAdded);
          }
          [HttpPut("editqualification")]
          public async Task<ActionResult<bool>> EditQualification(Qualification qualification)
          {
               var succeeded = await _mastersService.EditQualificationAsync(qualification);
               if (!succeeded) return BadRequest(new ApiResponse(400, "failed to edit the qualification"));
               return true;
          }

          [HttpDelete("deletequalification")]
          public async Task<ActionResult<bool>> DeleteCategory(Qualification qualification)
          {
               var succeeded = await _mastersService.DeleteQualificationAsync(qualification);
               if (!succeeded) return BadRequest(new ApiResponse(400, "Failed to delete the qualification"));
               return true;
          }

          [HttpGet("qualificationlist")]
          public async Task<ActionResult<Qualification>> GetQualificationListAsync()
          {
               var lst = await _mastersService.GetQualificationListAsync();
               if (lst == null) return BadRequest(new ApiResponse(400, "no data returned"));
               return Ok(lst);
          }

          /*
        
        
    //Qualifications
        Task<Category> AddQualification(string qualificationName);
        Task<bool> EditQualificationAsync(Qualification qualification);
        Task<bool> DeleteQualificationAsync(Qualification qualification);
        Task<IReadOnlyList<Category>> GetQualificationListAsync();

    //ReviewItemStatus.Description for Contract REVIEW RESULTS
        Task<ReviewItemData> AddReviewItemData(string reviewDescriptionName);
        Task<bool> EditReviewItemDataAsync(ReviewItemData reviewItemData);
        Task<bool> DeleteQualificationAsync(ReviewItemData reviewItemData);
        Task<IReadOnlyList<ReviewItemData>> GetReviewItemDataDescriptionListAsync();

 //ReviewItemStatus.Description for Contract REVIEW RESULTS
        Task<ReviewItemStatus> AddReviewItemStatus(string reviewDescriptionName);
        Task<bool> EditReviewItemStatusAsync(ReviewItemStatus ReviewItemStatus);
        Task<bool> DeleteReviewItemStatusAsync(ReviewItemStatus ReviewItemStatus);
        Task<IReadOnlyList<ReviewItemStatus>> GetReviewItemStatusListAsync();

    //ReviewItemData.Description for Contract REVIEW RESULTS
        Task<SkillData> AddSkillData(string skillname);
        Task<bool> EditSkillDataAsync(SkillData skillData);
        Task<bool> DeleteSkillDataAsync(SkillData skillData);
        Task<IReadOnlyList<SkillData>> GetSkillDataListAsync();

    //checklistHRData - job card for HR Executives
        Task<ChecklistHRData> AddChecklistHRParameter(string checklist);
        Task<bool> EditChecklistHRDataAsync(ChecklistHRData checklistHRData);
        Task<bool> DeleteChecklistHRDataAsync(ChecklistHRData checklistHRData);
        Task<IReadOnlyList<ChecklistHRData>> GetChecklistHRDataListAsync();

          */
     }
}