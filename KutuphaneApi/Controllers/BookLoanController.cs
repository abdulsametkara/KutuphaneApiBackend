using KutuphaneDataAccess.DTOs;
using KutuphaneServis.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace KutuphaneApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BookLoanController : ControllerBase
    {
        private readonly IBookLoanService _bookLoanService;
        public BookLoanController(IBookLoanService bookLoanService)
        {
            _bookLoanService = bookLoanService;
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("GetAllLoans")]
        public IActionResult GetAllLoans()
        {
            var response = _bookLoanService.ListAll();
            if (!response.IsSuccess)
            {
                return BadRequest(response.Message);
            }
            return Ok(response.Data);
        }

        [HttpPost("BorrowBook")]
        public async Task<IActionResult> BorrowBookAsync([FromBody] BookLoanCreateDto bookLoanCreateDto)
        {
            if (bookLoanCreateDto == null)
            {
                return BadRequest("Kitap ödünç alma bilgileri boş olamaz.");
            }
            var response = await _bookLoanService.BorrowBookAsync(bookLoanCreateDto.BookId, bookLoanCreateDto.UserId);
            if (!response.IsSuccess)
            {
                return BadRequest(response.Message);
            }
            return Ok(response);
        }

        [HttpPut("ReturnBook/{loanId}")]
        public async Task<IActionResult> ReturnBook(int loanId)
        {
            var response = await _bookLoanService.ReturnBook(loanId);
            if (!response.IsSuccess)
            {
                return BadRequest(response.Message);
            }
            return Ok(response);
        }

        [HttpGet("GetUserActiveLoans/{userId}")]
        public IActionResult GetUserActiveLoans(int userId)
        {
            var response = _bookLoanService.GetActiveLoans(userId);
            if (!response.IsSuccess)
            {
                return BadRequest(response.Message);
            }
            return Ok(response);
        }
        
    }
}