using KutuphaneDataAccess.DTOs;
using KutuphaneServis.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace KutuphaneApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [EnableRateLimiting("fixed")]
        [HttpGet("ListAll")]
        public IActionResult GetAll()
        {
            var books = _bookService.ListAll();
            if (!books.IsSuccess)
            {
                return NotFound(books.Message);
            }
            return Ok(books);
        }

        [HttpGet("GetById")]
        public IActionResult GetById(int id)
        {
            var book = _bookService.GetById(id);
            if (!book.IsSuccess)
            {
                return NotFound(book.Message);
            }
            return Ok(book);
        }

        [HttpPost("Create")]
        public IActionResult Create([FromBody] BookCreateDto book)
        {
            if (book == null)
            {
                return BadRequest("Kitap bilgileri boş bırakılamaz.");
            }
            var result = _bookService.Create(book);
            if (!result.Result.IsSuccess)
            {
                return BadRequest(result.Result.Message);
            }
            return Ok(result);
        }


        [HttpDelete("Delete")]
        public IActionResult Delete(int id)
        {
            var result = _bookService.Delete(id);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }

        [HttpGet("GetBooksByCategoryId")]
        public IActionResult GetBooksByCategoryId(int categoryId)
        {
            var books = _bookService.GetBooksByCategoryId(categoryId);
            if (!books.IsSuccess)
            {
                return NotFound(books.Message);
            }
            return Ok(books);
        }

        [HttpGet("GetBooksByAuthorId")]
        public IActionResult GetBooksByAuthorId(int authorId)
        {
            var books = _bookService.GetBooksByAuthorId(authorId);
            if (!books.IsSuccess)
            {
                return NotFound(books.Message);
            }
            return Ok(books);
        }

        [HttpPut("Update")]
        public IActionResult Update([FromBody] BookUpdateDto bookUpdateDto)
        {
            if (bookUpdateDto == null)
            {
                return BadRequest("Güncelleme bilgileri boş bırakılamaz.");
            }
            var result = _bookService.Update(bookUpdateDto);
            if (!result.Result.IsSuccess)
            {
                return BadRequest(result.Result.Message);
            }
            return Ok(result);
        }
    }
}
