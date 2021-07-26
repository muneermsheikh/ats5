using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using core.Entities.MasterEntities;
using core.Entities.Orders;
using core.Interfaces;
using core.ParamsAndDtos;
using core.Specifications;
using infra.Data;
using Microsoft.EntityFrameworkCore;

namespace infra.Services
{
     public class ContractReviewService : IContractReviewService
     {
          private readonly IUnitOfWork _unitOfWork;
          private readonly ATSContext _context;
          private readonly IMapper _mapper;
          public ContractReviewService(IUnitOfWork unitOfWork, ATSContext context, IMapper mapper)
          {
               _mapper = mapper;
               _context = context;
               _unitOfWork = unitOfWork;
          }

          public void AddReviewItemStatus(string reviewItemStatusName)
          {
               var status = new ReviewItemStatus(reviewItemStatusName);
               _unitOfWork.Repository<ReviewItemStatus>().Add(status);
          }

          public void AddReviewStatus(string reviewStatusName)
          {
               var status = new ReviewStatus { Status = reviewStatusName };
               _unitOfWork.Repository<ReviewStatus>().Add(status);
          }

          public async Task<bool> CreateContractReview(ContractReview cReview)
          {
               _unitOfWork.Repository<ContractReview>().Add(cReview);
               return (await _unitOfWork.Complete() > 0);
          }
          public void EditContractReview(ContractReview model)
          {
               //thanks to @slauma of stackoverflow
               var existingObj = _context.ContractReviews
                  .Where(p => p.Id == model.Id)
                  .AsNoTracking()
                  .SingleOrDefault();

               if (existingObj == null) return;

               _context.Entry(existingObj).CurrentValues.SetValues(model);   //saves only the parent, not children

               //Delete children that exist in existing record, but not in the new model order
               foreach (var existingItem in existingObj.ContractReviewItems.ToList())
               {
                    if (!model.ContractReviewItems.Any(c => c.Id == existingItem.Id && c.Id != default(int)))
                    {
                         _context.ContractReviewItems.Remove(existingItem);
                         _context.Entry(existingItem).State = EntityState.Deleted;
                    }
               }

               //children that are not deleted, are either updated or new ones to be added
               foreach (var item in model.ContractReviewItems)
               {
                    var existingItem = existingObj.ContractReviewItems.Where(c => c.Id == item.Id && c.Id != default(int)).SingleOrDefault();

                    if (existingItem != null)       // record exists, update it
                    {
                         _context.Entry(existingItem).CurrentValues.SetValues(item);
                         _context.Entry(existingItem).State = EntityState.Modified;
                    }
                    else            //record does not exist, insert a new record
                    {
                         var newItem = new ContractReviewItem(item.OrderItemId, item.OrderId, item.CategoryName, item.Quantity);
                         existingObj.ContractReviewItems.Add(newItem);
                         _context.Entry(newItem).State = EntityState.Added;
                    }
               }

               _context.Entry(existingObj).State = EntityState.Modified;
          }

          public async Task<ContractReview> GetContractReview(int orderId)
          {
               return await _context.ContractReviews.Where(x => x.OrderId == orderId).FirstOrDefaultAsync();
          }

          public async Task<IReadOnlyList<ContractReviewItemDto>> GetContractReviewItemsAsync(ContractReviewSpecParams cReviewParam)
          {
               var items = await _unitOfWork.Repository<ContractReviewItem>()
                   .ListAsync(new ContractReviewSpecs(cReviewParam));
               return _mapper.Map<IReadOnlyList<ContractReviewItem>, IReadOnlyList<ContractReviewItemDto>>(items);
          }

          public async Task<IReadOnlyList<ContractReviewItemDto>> GetContractReviewItemsByOrderIdAsync(int orderid)
          {
               var items = await _unitOfWork.Repository<ContractReviewItem>().ListAsync(new ContractReviewSpecs(orderid, 0));
               return _mapper.Map<IReadOnlyList<ContractReviewItem>, IReadOnlyList<ContractReviewItemDto>>(items);
          }

          public async Task<bool> DeleteContractReview(int orderid)
          {
               var items = await _context.ContractReviewItems.Where(x => x.OrderId == orderid).ToListAsync();
               foreach (var item in items)
               {
                    _unitOfWork.Repository<ContractReviewItem>().Delete(item);
               }
               return (await _unitOfWork.Complete() > 0);
          }

          public async Task<ContractReviewItemDto> GetContractReviewItemWithOrderDetails(int orderItemId)
          {
               var items = await _unitOfWork.Repository<ContractReviewItem>().GetEntityWithSpec(new ContractReviewSpecs(orderItemId));
               return _mapper.Map<ContractReviewItem, ContractReviewItemDto>(items);
          }

          public async Task<bool> DeleteContractReviewItem(int orderitemid)
          {
               var item = await _context.ContractReviewItems.Where(x => x.OrderItemId == orderitemid).FirstOrDefaultAsync();
               _unitOfWork.Repository<ContractReviewItem>().Delete(item);
               return (await _unitOfWork.Complete() > 0);
          }

          public async Task<ICollection<ReviewStatus>> GetReviewStatus()
          {
               return await _context.ReviewStatuses.ToListAsync();
          }

          public async Task<IReadOnlyList<ReviewItemStatus>> GetReviewItemStatus()
          {
               return await _unitOfWork.Repository<ReviewItemStatus>().ListAllAsync();
          }
     }
}