using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using core.Entities;
using core.Entities.MasterEntities;
using core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace api.Controllers
{
    
    public class AssessmentQBankController : BaseApiController
    {
        private readonly ILogger<AssessmentQBankController> _logger;
          private readonly IAssessmentQBankService _service;

        public AssessmentQBankController(ILogger<AssessmentQBankController> logger, 
            IAssessmentQBankService service)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("existingqbankcategories")]
        public async Task<ICollection<Category>> ExistingQBankCategories()
        {
            return await _service.GetExistingCategoriesInAssessmentQBank();
        }

        [HttpGet("assessmentbankqs")]
        public async Task<ICollection<AssessmentQBank>> GetAssessmentBankQs()
        {
            return await _service.GetAssessmentQBanks();
        }

        [HttpGet("catqs/{categoryName}")]
        public async Task<AssessmentQBank> GetAssessmentQsOfCategoryByName(string categoryName)
        {
            return await _service.GetAssessmentQsOfACategoryByName(categoryName);
        }

        [HttpGet("qbank/{id}")]
        public async Task<AssessmentQBank> GetAssessmentQBankByCategoryId(int id)
        {
            return await _service.GetAssessmentQBankByCategoryId(id);
        }

    }
}