using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using core.Entities.Admin;
using core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{

     public class CustomerReviewController : BaseApiController
     {
          private readonly ICustomerReviewService _customerReviewService;
          private readonly ICommonServices _commonService;
          public CustomerReviewController(ICustomerReviewService customerReviewService, ICommonServices commonService)
          {
               _commonService = commonService;
               _customerReviewService = customerReviewService;
          }

          [HttpGet("{customerId}")]
          public async Task<CustomerReview> GetCustomerReview (int customerId)
          {
               var rvw = await _customerReviewService.GetOrAddCustomerReview(customerId);
               if (string.IsNullOrEmpty(rvw.CustomerName)) rvw.CustomerName = await _commonService.CustomerNameFromCustomerId(rvw.CustomerId);
               return rvw;
          } 

          [HttpGet("customerReviewData")]
          public async Task<ICollection<CustomerReviewData>> GetCustomerReviewStatusData()
          {
               var data = await _customerReviewService.GetCustomerReviewStatusData();
               return data;
          }
          
          [HttpPut]
          public async Task<bool> UpdateCustomerReview(CustomerReview customerReview)
          {
               return await _customerReviewService.EditCustomerReview(customerReview);
          }
     }
}