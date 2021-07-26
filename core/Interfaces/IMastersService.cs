using System.Collections.Generic;
using System.Threading.Tasks;
using core.Entities;
using core.Entities.MasterEntities;
using core.ParamsAndDtos;

namespace core.Interfaces
{
    public interface IMastersService
    {
        Task<Category> AddCategory(string categoryName);
        Task<bool> EditCategoryAsync(Category category);
        Task<bool> DeleteCategoryAsync(Category category);
        Task<IReadOnlyList<Category>> GetCategoryListAsync(CategoryParams categoryParams);

    //industry
        Task<Industry> AddIndustry(string industryName);
        Task<bool> EditIndustryAsync(Industry industry);
        Task<bool> DeleteIndustryyAsync(Industry industry);
        Task<IReadOnlyList<Industry>> GetIndustryListAsync(IndustryParams industryParams);
        //Task<IReadOnlyList<Industry>> GetIndustryListOfACategoryAsync(IndustryParams industryParams);
        
    //Qualifications
        Task<Qualification> AddQualification(string qualificationName);
        Task<bool> EditQualificationAsync(Qualification qualification);
        Task<bool> DeleteQualificationAsync(Qualification qualification);
        Task<IReadOnlyList<Qualification>> GetQualificationListAsync();

    //ReviewItemStatus.Description for Contract REVIEW RESULTS
        Task<ReviewItemData> AddReviewItemData(string reviewDescriptionName);
        Task<bool> EditReviewItemDataAsync(ReviewItemData reviewItemData);
        Task<bool> DeleteReviewItemDataAsync(ReviewItemData reviewItemData);
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

    }
}