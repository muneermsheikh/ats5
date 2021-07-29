using System.Threading.Tasks;
using core.Interfaces;
using core.ParamsAndDtos;
using infra.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using core.Entities.Process;
using System.Collections.Generic;
using System;
using core.Entities.HR;

namespace infra.Services
{
     public class CommonServices : ICommonServices
     {
          private readonly ATSContext _context;
          public CommonServices(ATSContext context)
          {
               _context = context;
          }

          public async Task<string> CategoryNameFromCategoryId(int categoryId)
          {
               return await _context.Categories.Where(x => x.Id == categoryId).Select(x => x.Name).FirstOrDefaultAsync();
          }

          public async Task<string> CustomerNameFromOrderDetailId(int orderDetailId)
          {
               var qry = await (from r in _context.OrderItems where r.Id == orderDetailId
                    join o in _context.Orders on r.OrderId equals o.Id
                    join c in _context.Customers on o.CustomerId equals c.Id
                    select c.CustomerName)
                    .FirstOrDefaultAsync();
               return qry;
          }

          public async Task<CommonDataDto> CommonDataFromCVRefId(int cvrefid)
          {
               var qry = await (from r in _context.CVRefs where r.Id == cvrefid
                    join i in _context.OrderItems on r.OrderItemId equals i.Id 
                    join cat in _context.Categories on i.CategoryId equals cat.Id
                    join ordr in _context.Orders on i.OrderId equals ordr.Id 
                    join c in _context.Customers on ordr.CustomerId equals c.Id
                    join cand in _context.Candidates on r.CandidateId equals cand.Id
                    select (new CommonDataDto {
                         ApplicationNo = cand.ApplicationNo,
                         CandidateName = cand.FullName,
                         CustomerName = c.CustomerName, 
                         CategoryName = cat.Name, 
                         OrderNo = ordr.OrderNo
                    })).FirstOrDefaultAsync();

               return qry; 
          }


          public async Task<CommonDataDto> CommonDataFromOrderDetailIdAndCandidateId(int orderDetailId, int candidateId)
          {
               var qry = await (from i in _context.OrderItems where i.Id == orderDetailId
                    join cat in _context.Categories on i.CategoryId equals cat.Id
                    join ordr in _context.Orders on i.OrderId equals ordr.Id 
                    join c in _context.Customers on ordr.CustomerId equals c.Id
                    select (new {
                         CustomerName = c.CustomerName, 
                         CategoryName = cat.Name, 
                         CategoryId = i.CategoryId,
                         OrderNo = ordr.OrderNo
                    })).FirstOrDefaultAsync();
               var qry2 = await _context.Candidates.Where(x => x.Id == candidateId)
                    .Select(x => new {x.ApplicationNo, x.FullName}).FirstOrDefaultAsync();
               
               return new CommonDataDto{
                    CustomerName = qry.CustomerName, CategoryName = qry.CategoryName, OrderNo = qry.OrderNo,
                    ApplicationNo = qry2.ApplicationNo, CandidateName = qry2.FullName, CategoryId = qry.CategoryId
               };
               
          }

          public async Task<ICollection<SelectionDecisionToRegisterDto>> PopulateSelectionDecisionsToRegisterDto(ICollection<SelectionDecisionToRegisterDto> dto)
          {
               var data = dto.Select(x => new {x.SelectionStatusId, x.DecisionDate}).FirstOrDefault();
               int selectionStatusId = data.SelectionStatusId;
               DateTime dt = data.DecisionDate.Date;
               var qry = await (from c in _context.CVRefs where dto.Select(x => x.CVRefId).Contains(c.Id)
                    /*join d in dto on cvref.Id equals d.CVRefId
                    join i in _context.OrderItems on cvref.OrderItemId equals i.Id
                    join cat in _context.Categories on i.CategoryId equals cat.Id
                    join ordr in _context.Orders on i.OrderId equals ordr.Id 
                    join c in _context.Customers on ordr.CustomerId equals c.Id
                    join cand in _context.Candidates on cvref.CandidateId equals cand.Id
                    */
                    select (new SelectionDecisionToRegisterDto {
                         CVRefId = c.Id,
                         OrderItemId = c.OrderItemId, 
                         OrderId = c.OrderId,
                         OrderNo = c.OrderNo,
                         CategoryId = c.CategoryId,
                         CategoryName = c.CategoryName,
                         CandidateId = c.CandidateId,
                         ApplicationNo = c.ApplicationNo,
                         CandidateName = c.CandidateName,
                         SelectionStatusId= selectionStatusId,
                         DecisionDate= dt
                    })).ToListAsync();
               return qry;               
          }

          public async Task<Employment> PopulateEmploymentFromCVRefId(int cvrefid, int salary, int charges, DateTime selectedOn)
          {
               var qry = await (from c in _context.CVRefs where c.Id == cvrefid
                    join i in _context.OrderItems on c.OrderItemId equals i.Id
                    select (new Employment {
                         CVRefId = c.Id,
                         OrderItemId = c.OrderItemId, 
                         OrderId = c.OrderId,
                         OrderNo = c.OrderNo,
                         CustomerName = c.CustomerName,
                         CategoryId = c.CategoryId,
                         CategoryName = c.CategoryName,
                         CandidateId = c.CandidateId,
                         ApplicationNo = c.ApplicationNo,
                         CandidateName = c.CandidateName,
                         SalaryCurrency = i.Remuneration.SalaryCurrency,
                         Salary = salary,
                         ContractPeriodInMonths = i.Remuneration.ContractPeriodInMonths,
                         HousingProvidedFree = i.Remuneration.HousingProvidedFree,
                         HousingAllowance = i.Remuneration.HousingAllowance,
                         FoodProvidedFree = i.Remuneration.FoodProvidedFree,
                         FoodAllowance = i.Remuneration.FoodAllowance,
                         TransportProvidedFree = i.Remuneration.TransportProvidedFree,
                         TransportAllowance = i.Remuneration.TransportAllowance,
                         OtherAllowance = i.Remuneration.OtherAllowance,
                         LeavePerYearInDays = i.Remuneration.LeavePerYearInDays,
                         LeaveAirfareEntitlementAfterMonths = i.Remuneration.LeaveAirfareEntitlementAfterMonths,
                         SelectedOn = selectedOn,
                         Charges = charges
                    })).FirstOrDefaultAsync();
               
               return qry;
          }
          public async Task<CommonDataDto> PendingDeployments()
          {
               /* var tempQury =  from SPListItem customers in _customerList
               group customers by customers["ContractNumber"] into gby 
               select gby.First(); */

               var tempQuery =  from d in _context.Deploys
                    group d by d.CVRefId into dTop 
                    orderby dTop.Key descending
                    select new {
                         Key = dTop.First(),
                         Status = dTop.First()
                    };

               var qry = await (from r in _context.CVRefs 
                    join d in tempQuery on r.Id equals d.Key.CVRefId
                    join i in _context.OrderItems on r.OrderItemId equals i.Id 
                    join cat in _context.Categories on i.CategoryId equals cat.Id
                    join ordr in _context.Orders on i.OrderId equals ordr.Id 
                    join c in _context.Customers on ordr.CustomerId equals c.Id
                    join cand in _context.Candidates on r.CandidateId equals cand.Id
                    select (new CommonDataDto {
                         ApplicationNo = cand.ApplicationNo,
                         CandidateName = cand.FullName,
                         CustomerName = c.CustomerName, 
                         CategoryName = cat.Name, 
                         OrderNo = ordr.OrderNo,
                         DeployStatus = d.Status.StatusId
                    })).FirstOrDefaultAsync();

               return qry;
               
          }
     }
}