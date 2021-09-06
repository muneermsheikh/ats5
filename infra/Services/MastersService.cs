using System.Collections.Generic;
using System.Threading.Tasks;
using core.Entities;
using core.Entities.MasterEntities;
using core.Interfaces;
using core.Params;
using core.ParamsAndDtos;
using core.Specifications;
using infra.Data;
using Microsoft.EntityFrameworkCore;

namespace infra.Services
{
     public class MastersService : IMastersService
     {
          private readonly IUnitOfWork _unitOfWork;

          private readonly ATSContext _context;
          public MastersService(ATSContext context, IUnitOfWork unitOfWork)
          {
               _unitOfWork = unitOfWork;
               _context = context;
          }

          public async Task<Category> AddCategory(string categoryName)
          {
               var categoryEntity = new Category(categoryName);
               _unitOfWork.Repository<Category>().Add(categoryEntity);
               if (await _unitOfWork.Complete() > 0) return categoryEntity;
               return null;
          }


          public async Task<Industry> AddIndustry(string industryName)
          {
               var industryEntity = new Industry(industryName);
               _unitOfWork.Repository<Industry>().Add(industryEntity);
               if (await _unitOfWork.Complete() > 0) return industryEntity;
               return null;
          }

          public async Task<Qualification> AddQualification(string qualificationName)
          {
               var entity = new Qualification(qualificationName);
               _unitOfWork.Repository<Qualification>().Add(entity);
               if (await _unitOfWork.Complete() > 0) return entity;
               return null;
          }

          public async Task<ReviewItemData> AddReviewItemData(string reviewDescriptionName)
          {
               var srno = await _context.ReviewItemDatas.MaxAsync(x => x.SrNo) + 1;
               var entity = new ReviewItemData(srno, reviewDescriptionName);

               _unitOfWork.Repository<ReviewItemData>().Add(entity);
               if (await _unitOfWork.Complete() > 0) return entity;
               return null;
          }

          public async Task<ReviewItemStatus> AddReviewItemStatus(string reviewDescriptionName)
          {
               var entity = new ReviewItemStatus(reviewDescriptionName);
               _unitOfWork.Repository<ReviewItemStatus>().Add(entity);
               if (await _unitOfWork.Complete() > 0) return entity;
               return null;
          }

          public async Task<SkillData> AddSkillData(string skillname)
          {
               var entity = new SkillData(skillname);
               _unitOfWork.Repository<SkillData>().Add(entity);
               if (await _unitOfWork.Complete() > 0) return entity;
               return null;

          }

          public async Task<bool> DeleteCategoryAsync(Category category)
          {
               _unitOfWork.Repository<Category>().Delete(category);
               return (await _unitOfWork.Complete() > 0);
          }

          public async Task<bool> DeleteIndustryyAsync(Industry industry)
          {
               _unitOfWork.Repository<Industry>().Delete(industry);
               return (await _unitOfWork.Complete() > 0);
          }

          public async Task<bool> DeleteQualificationAsync(Qualification qualification)
          {
               _unitOfWork.Repository<Qualification>().Delete(qualification);
               return (await _unitOfWork.Complete() > 0);
          }

          public async Task<bool> DeleteReviewItemDataAsync(ReviewItemData reviewItemData)
          {
               _unitOfWork.Repository<ReviewItemData>().Delete(reviewItemData);
               return (await _unitOfWork.Complete() > 0);
          }

          public async Task<bool> DeleteReviewItemStatusAsync(ReviewItemStatus reviewItemStatus)
          {
               _unitOfWork.Repository<ReviewItemStatus>().Delete(reviewItemStatus);
               return (await _unitOfWork.Complete() > 0);
          }

          public async Task<bool> DeleteSkillDataAsync(SkillData skillData)
          {
               _unitOfWork.Repository<SkillData>().Delete(skillData);
               return (await _unitOfWork.Complete() > 0);
          }

          public async Task<bool> EditCategoryAsync(Category category)
          {
               _unitOfWork.Repository<Category>().Update(category);
               return (await _unitOfWork.Complete() > 0);
          }

          public async Task<bool> EditIndustryAsync(Industry industry)
          {
               _unitOfWork.Repository<Industry>().Update(industry);
               return (await _unitOfWork.Complete() > 0);
          }

          public async Task<bool> EditQualificationAsync(Qualification qualification)
          {
               _unitOfWork.Repository<Qualification>().Update(qualification);
               return (await _unitOfWork.Complete() > 0);
          }

          public async Task<bool> EditReviewItemDataAsync(ReviewItemData reviewItemData)
          {
               _unitOfWork.Repository<ReviewItemData>().Update(reviewItemData);
               return (await _unitOfWork.Complete() > 0);
          }

          public async Task<bool> EditReviewItemStatusAsync(ReviewItemStatus reviewItemStatus)
          {
               _unitOfWork.Repository<ReviewItemStatus>().Update(reviewItemStatus);
               return (await _unitOfWork.Complete() > 0);
          }

          public async Task<bool> EditSkillDataAsync(SkillData skillData)
          {
               _unitOfWork.Repository<SkillData>().Update(skillData);
               return (await _unitOfWork.Complete() > 0);
          }

          public async Task<Pagination<Category>> GetCategoryListAsync(CategorySpecParams specParams)
          {
               var spec = new CategorySpecs(specParams);
               var specCount = new CategoryForCountSpecs(specParams);
               var totalCount = await _unitOfWork.Repository<Category>().CountAsync(specCount);
               var lst = await _unitOfWork.Repository<Category>().ListAsync(spec);
               //var data = _mapper.Map<IReadOnlyList<CategoryToReturnDto>>(lst);

               return new Pagination<Category>(specParams.PageIndex, specParams.PageSize, totalCount, lst);
          }

          public async Task<Pagination<Industry>> GetIndustryListAsync(IndustrySpecParams specParams)
          {
               var spec = new IndustrySpecs(specParams);
               var specCount = new IndustryForCountSpecs(specParams);
               var totalCount = await _unitOfWork.Repository<Industry>().CountAsync(specCount);
               var lst = await _unitOfWork.Repository<Industry>().ListAsync(spec);
               //var data = _mapper.Map<IReadOnlyList<IndustryToReturnDto>>(lst);
 
               return new Pagination<Industry>(specParams.PageIndex, specParams.PageSize, totalCount, lst);
          }

          public async Task<Pagination<Qualification>> GetQualificationListAsync(QualificationSpecParams specParams)
          {
               var spec = new QualificationSpecs(specParams);
               var specCount = new QualificationForCountSpecs(specParams);
               var totalCount = await _unitOfWork.Repository<Qualification>().CountAsync(specCount);
               var lst = await _unitOfWork.Repository<Qualification>().ListAsync(spec);

               return new Pagination<Qualification>(specParams.PageIndex, specParams.PageSize, totalCount, lst);
          }

          public async Task<IReadOnlyList<ReviewItemData>> GetReviewItemDataDescriptionListAsync()
          {
               return await _unitOfWork.Repository<ReviewItemData>().ListAllAsync();
          }

          public async Task<IReadOnlyList<ReviewItemStatus>> GetReviewItemStatusListAsync()
          {
               return await _unitOfWork.Repository<ReviewItemStatus>().ListAllAsync();
          }

          public async Task<IReadOnlyList<SkillData>> GetSkillDataListAsync()
          {
               return await _unitOfWork.Repository<SkillData>().ListAllAsync();
          }
     }
}