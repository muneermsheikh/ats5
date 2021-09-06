using System.Collections.Generic;
using System.Threading.Tasks;
using core.Entities;
using core.Entities.MasterEntities;
using core.Params;
using core.ParamsAndDtos;

namespace core.Interfaces
{
    public interface IMastersService
    {
        Task<Category> AddCategory(string categoryName);
        Task<bool> EditCategoryAsync(Category category);
        Task<bool> DeleteCategoryAsync(Category category);
        Task<Pagination<Category>> GetCategoryListAsync(CategorySpecParams categoryParams);

    //industry
        Task<Industry> AddIndustry(string industryName);
        Task<bool> EditIndustryAsync(Industry industry);
        Task<bool> DeleteIndustryyAsync(Industry industry);
        Task<Pagination<Industry>> GetIndustryListAsync(IndustrySpecParams industryParams);
        //Task<IReadOnlyList<Industry>> GetIndustryListOfACategoryAsync(IndustryParams industryParams);
        
    //Qualifications
        Task<Qualification> AddQualification(string qualificationName);
        Task<bool> EditQualificationAsync(Qualification qualification);
        Task<bool> DeleteQualificationAsync(Qualification qualification);
        Task<Pagination<Qualification>> GetQualificationListAsync(QualificationSpecParams specParams);

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

    }
}