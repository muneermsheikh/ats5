using System.Collections.Generic;
using System.Threading.Tasks;
using api.Errors;
using core.Entities;
using core.Interfaces;
using infra.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{    
     [Authorize(Policy = "Employee")]
     public class CategoriesController : BaseApiController
     {
          private readonly IUnitOfWork _unitOfWork;
          public CategoriesController(IUnitOfWork unitOfWork)
          {
               _unitOfWork = unitOfWork;
           }

          [HttpGet]
          [ProducesResponseType(StatusCodes.Status200OK)]
          [ProducesResponseType(StatusCodes.Status404NotFound)]
          public async Task<ActionResult<ICollection<Category>>> GetCategories()
          {
               var cats = await _unitOfWork.Repository<Category>().ListAllAsync();
               if (cats == null) return NotFound(new ApiResponse(404));
               return Ok(cats);
          }

          [HttpGet("{id}")]
          [ProducesResponseType(StatusCodes.Status200OK)]
          [ProducesResponseType(StatusCodes.Status404NotFound)]
          public async Task<ActionResult<Category>> GetCategory(int id)
          {
               var cat = await _unitOfWork.Repository<Category>().GetByIdAsync(id);
               if (cat == null) return NotFound(new ApiResponse(404));
               return cat;
          }
     }
}