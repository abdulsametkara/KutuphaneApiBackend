using KutuphaneServis.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace KutuphaneApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IUploadService _uploadService;
        public UploadController(IUploadService uploadService)
        {
            _uploadService = uploadService;
        }

        [HttpPost("upload")]

        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            var result = await _uploadService.UploadFile(file);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }

        [HttpGet("GetFile/{fileKey}")]
        public async Task<IActionResult> GetFile(string fileKey)
        {
            if (string.IsNullOrEmpty(fileKey))
            {
                return BadRequest("FileKey boş bırakılamaz");
            }
            var response = await _uploadService.GetFile(fileKey);
            if (!response.IsSuccess)
            {
                return NotFound(response.Message);
            }
            return Ok(response);
        }
    }
}
