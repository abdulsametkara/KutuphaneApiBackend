﻿using KutuphaneCore.Entities;
using KutuphaneDataAccess.DTOs;
using KutuphaneDataAccess.Repository;
using KutuphaneServis.Interfaces;
using KutuphaneServis.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;


namespace KutuphaneServis.Service
{
    public class UploadService : IUploadService
    {
        private readonly IGenericRepository<UploadedFiles> _uploadedFiles;
        private readonly ILogger<UploadService> _logger;

        public UploadService(IGenericRepository<UploadedFiles> uploadedFiles, ILogger<UploadService> logger)
        {
            _uploadedFiles = uploadedFiles;
            _logger = logger;
        }

        public Task<IResponse<UploadFileDto>> UploadFile(IFormFile file)
        {
            try
            {
                if (file == null)
                {
                    return Task.FromResult<IResponse<UploadFileDto>>(ResponseGeneric<UploadFileDto>.Error("Dosya seçilmedi."));
                }

                var existingFile = _uploadedFiles.GetAll().FirstOrDefault(f => f.FileName == file.FileName);
                if (existingFile != null) {
                    return Task.FromResult<IResponse<UploadFileDto>>(ResponseGeneric<UploadFileDto>.Error("Bu dosya daha önce yüklenmiş."));
                }

                var allowedTypes = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                if(!allowedTypes.Contains(Path.GetExtension(file.FileName).ToLower()))
                {
                    return Task.FromResult<IResponse<UploadFileDto>>(ResponseGeneric<UploadFileDto>.Error("Geçersiz dosya türü."));
                }

                if(file.Length > 10 * 1024 * 1024)
                {
                    return Task.FromResult<IResponse<UploadFileDto>>(ResponseGeneric<UploadFileDto>.Error("Dosya boyutu 10 MB'dan büyük olamaz."));
                }

                using var memoryStream = new MemoryStream();
                file.CopyTo(memoryStream);
                var base64String = Convert.ToBase64String(memoryStream.ToArray());

                var uploadedFiles = new UploadedFiles
                {
                    FileName = file.FileName,
                    FileType = file.ContentType, //MIME type olarak kaydedilir
                    FileKey = Guid.NewGuid().ToString(),
                    Base64String = base64String,
                    RecordDate = DateTime.Now
                };

                _uploadedFiles.Create(uploadedFiles);
                _uploadedFiles.Save();

                var responseDto = new UploadFileDto
                {
                    FileKey = uploadedFiles.FileKey,
                    FileName = uploadedFiles.FileName
                };

                _logger.LogInformation("Dosya Yüklendi {FileName}", uploadedFiles.FileName);
                return Task.FromResult<IResponse<UploadFileDto>>(ResponseGeneric<UploadFileDto>.Success(responseDto, "Dosya başarıyla yüklendi."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Dosya yüklenirken hata oluştu");
                return Task.FromResult<IResponse<UploadFileDto>>(ResponseGeneric<UploadFileDto>.Error("Dosya yüklenirken bir hata oluştu."));
            }
        }

        public Task<IResponse<FileResponseDto>> GetFile(string fileKey)
        {
            try
            {
                var file = _uploadedFiles.GetAll().FirstOrDefault(f => f.FileKey == fileKey);
                if (file == null)
                {
                    return Task.FromResult<IResponse<FileResponseDto>>(ResponseGeneric<FileResponseDto>.Error("Dosya bulunamadı."));
                }
                var responseDto = new FileResponseDto
                {
                    FileName = file.FileName,
                    FileType = file.FileType,
                    Base64String = file.Base64String
                };

                return Task.FromResult<IResponse<FileResponseDto>>(ResponseGeneric<FileResponseDto>.Success(responseDto, "Dosya başarıyla bulundu."));
            }
            catch (Exception ex)
            {
                return Task.FromResult<IResponse<FileResponseDto>>(ResponseGeneric<FileResponseDto>.Error("Dosya bulunurken bir hata oluştu."));
            }
        }
    }
}