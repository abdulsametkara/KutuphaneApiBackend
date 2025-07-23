using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KutuphaneCore.Entities
{
    public class UploadedFiles : BaseEntity
    {
        public string FileKey { get; set; }
        public string Base64String { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }

    }
}
