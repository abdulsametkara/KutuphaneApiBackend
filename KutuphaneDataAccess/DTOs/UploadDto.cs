using System;

namespace KutuphaneDataAccess.DTOs
{
    public class UploadFileDto
    {
        public string FileKey { get; set; }
        public string FileName { get; set; }
    }

    public class FileResponseDto
    {
        public string FileName { get; set; }
        public string Base64String { get; set; }
        public string FileType { get; set; }
    }

}