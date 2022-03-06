using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using core.Entities;
using core.Entities.MasterEntities;
using core.Interfaces;
using infra.Data;
using Microsoft.EntityFrameworkCore;

namespace infra.Services
{
     public class AssessmentQBankService : IAssessmentQBankService
     {
        private readonly ATSContext _context;
        public AssessmentQBankService(ATSContext context)
        {
            _context = context;
        }

          public async Task<AssessmentQBank> GetAssessmentQBankByCategoryId(int id)
          {
            var x = await _context.AssessmentQBank.Where(x => x.CategoryId == id)
                .Include(x => x.AssessmentQBankItems.OrderBy(x => x.QNo))
                .FirstOrDefaultAsync();
            return x;
          }

          public async Task<ICollection<AssessmentQBank>> GetAssessmentQBanks()
        {
            var x = await _context.AssessmentQBank.Include(x => x.AssessmentQBankItems.OrderBy(x => x.QNo))
                .OrderBy(x => x.CategoryId)
                .ToListAsync();
            return x;
        }

          public async Task<AssessmentQBank> GetAssessmentQsOfACategoryByName(string categoryName)
          {
                var x = await _context.AssessmentQBank.Include(x => x.AssessmentQBankItems.OrderBy(x => x.QNo))
                    .Where(x => x.CategoryName.ToLower() == categoryName.ToLower())
                    .FirstOrDefaultAsync();
                
                return x;
          }

          public async Task<ICollection<Category>> GetExistingCategoriesInAssessmentQBank()
        {
            /*    
            var qry = await _context.AssessmentQBanks
                .Join(_context.Categories,
                    a => a.CategoryId,
                    c => c.Id,
                    (a, c) => new {Category = c})
                .Select(x => x.Category)
                .OrderBy(x => x.Name)
                .ToListAsync();
            */
            var qry = await _context.AssessmentQBank   //.GroupBy(x => x.CategoryName)
                .Include(x => x.AssessmentQBankItems)
                .Select (x => new Category{Id = (int)x.CategoryId, Name = x.CategoryName})
                //.GroupBy(x => x.Name)
                .OrderBy(x => x.Name)
                .ToListAsync();
            return (ICollection<Category>)qry;
        }
     }
}