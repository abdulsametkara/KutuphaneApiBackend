﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KutuphaneCore.Entities
{
    public class Book : BaseEntity
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public int CountofPage { get; set; }

        public string? FileKey { get; set; }

        public int AuthorId { get; set; }
        public int CategoryId { get; set; }

        public int TotalCopies { get; set; }
        public int AvailableCopies { get; set; }
    }
}
