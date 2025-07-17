using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KutuphaneCore.DTOs
{
    public class AuthorCreateDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PlaceofBirth { get; set; }
        public int YearofBirth { get; set; } 

    }

    public class AuthorQueryDto
    {
        public int Id { get; set; }
        public DateTime RecordDate { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PlaceofBirth { get; set; }
        public int YearofBirth { get; set; }
    }

    public class AuthorUpdateDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? PlaceofBirth { get; set; }
        public int? YearofBirth { get; set; }
    }
}
