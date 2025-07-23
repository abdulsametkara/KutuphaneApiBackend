using KutuphaneCore.Entities;
using KutuphaneDataAccess.DTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KutuphaneServis.Interfaces
{
    public interface IUploadService
    {
        public Task<IResponse<UploadFileDto>> UploadFile(IFormFile file);
        public Task<IResponse<FileResponseDto>> GetFile(string fileKey);
    }
}
