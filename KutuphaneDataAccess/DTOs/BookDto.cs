using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KutuphaneDataAccess.DTOs
{
    public class BookCreateDto
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public int CountofPage { get; set; }


        public int AuthorId { get; set; }
        public int CategoryId { get; set; }
    }

    public class BookQueryDto
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public int CountofPage { get; set; }
        public int Id { get; set; }
        public DateTime RecordDate { get; set; }

        public int AuthorId { get; set; }
        public int CategoryId { get; set; }
    }

    public class BookUpdateDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? CountofPage { get; set; }
        public int? AuthorId { get; set; }
        public int? CategoryId { get; set; }
    }
}
