using KutuphaneDataAccess.DTOs;
using KutuphaneServis.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KutuphaneApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("ListAll")]
        public IActionResult ListAll()
        {
            var category = _categoryService.ListAll();
            if (!category.IsSuccess)
            {
                return NotFound(category.Message);
            }
            return Ok(category);
        }

        [HttpGet("GetById/{id}")]
        public IActionResult GetById(int id)
        {
            var response = _categoryService.GetById(id);
            if (!response.IsSuccess)
            {
                return NotFound(response.Message);
            }
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("Create")]
        public IActionResult Create([FromBody] CategoryCreateDto category)
        {
            if (category == null)
            {
                return BadRequest("Kategori bilgileri boş bırakılamaz.");
            }
            var result = _categoryService.Create(category);

            if (!result.Result.IsSuccess)
            {
                return BadRequest(result.Result.Message);
            }
            return Ok(result);
        }


        [Authorize(Roles = "Admin")]
        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            var result = _categoryService.Delete(id);
            if (!result.IsSuccess)
            {
                return NotFound(result.Message);
            }
            return Ok(result);
        }

        [HttpGet("GetByName/{name}")]
        public IActionResult GetByName(string name)
        {
            var response = _categoryService.GetByName(name);
            if (!response.IsSuccess)
            {
                return NotFound(response.Message);
            }
            return Ok(response);
        }



        [Authorize(Roles = "Admin")]
        [HttpPut("Update")]
        public IActionResult Update([FromBody] CategoryUpdateDto categoryUpdateDto)
        {
            if (categoryUpdateDto == null)
            {
                return BadRequest("Güncelleme bilgileri boş bırakılamaz.");
            }
            var result = _categoryService.Update(categoryUpdateDto).Result;
            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }
    }
}
