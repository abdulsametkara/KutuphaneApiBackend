    using KutuphaneCore.DTOs;
using KutuphaneCore.Entities;
using KutuphaneServis.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KutuphaneApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        [AllowAnonymous]
        [HttpGet("ListAll")]

        public IActionResult GetAll()
        {
            var authors = _authorService.ListAll();
            if (!authors.IsSuccess)
            {
                return NotFound(authors.Message);
            }

            return Ok(authors);
        }

        [HttpDelete("Delete")]
        public IActionResult Delete(int id)
        {
            var result = _authorService.Delete(id);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }

        [HttpPost("Create")]
        public IActionResult Create([FromBody] AuthorCreateDto author)
        {
            if (author == null)
            {
                return BadRequest("Yazar bilgileri boş bırakılamaz.");
            }
            var result = _authorService.Create(author);
            if (!result.Result.IsSuccess)
            {
                return BadRequest(result.Result.Message);
            }
            return Ok(result);
        }

        [HttpGet("GetByName")]

        public IActionResult GetByName(string name)
        {
            var result = _authorService.GetByName(name);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }

        [HttpGet("GetById")]
        public IActionResult GetById(int id)
        {
            var author = _authorService.GetById(id);
            if (!author.IsSuccess)
            {
                return NotFound(author.Message);
            }
            return Ok(author);
        }

        [HttpPut("Update")]
        public IActionResult Update([FromBody] AuthorUpdateDto authorUpdate)
        {
            if (authorUpdate == null)
            {
                return BadRequest("Yazar bilgileri boş bırakılamaz.");
            }
            var result = _authorService.Update(authorUpdate);
            if (!result.Result.IsSuccess)
            {
                return BadRequest(result.Result.Message);
            }
            return Ok(result);
        }
    }
}
