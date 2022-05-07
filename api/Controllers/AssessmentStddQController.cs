using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using core.Entities.HR;
using core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace api.Controllers
{
    public class AssessmentStddQController : BaseApiController
    {
        private readonly ILogger<AssessmentStddQController> _logger;
          private readonly IAssessmentStandardQService _service;

        public AssessmentStddQController(ILogger<AssessmentStddQController> logger, IAssessmentStandardQService service)
        {
               _service = service;
            _logger = logger;
        }

        [HttpGet("byid/{id}")]
        public async Task<AssessmentStandardQ> GetStandardQ(int id)
        {
            return await _service.GetStandardAssessmentQ(id);
        }   

        [HttpGet]
        public async Task<ICollection<AssessmentStandardQ>> GetStandardQs()
        {
            return await _service.GetStandardAssessmentQs();
        }   

        [HttpDelete("{id}")]
        public async Task<bool> deleteStddQ(int id) 
        {
            return await _service.DeleteStandardAssessmentQ(id);
        }

        [HttpPost]
        public async Task<AssessmentStandardQ> PostNewStddQ(AssessmentStandardQ stddQ)
        {
            return await _service.CreateStandardAssessmentQ(stddQ);
        }

        [HttpPut]
        public async Task<bool> UpdateNewStddQ(ICollection<AssessmentStandardQ> stddqs)
        {
            return await _service.EditStandardAssessmentQ(stddqs);
        }
        
    }
}