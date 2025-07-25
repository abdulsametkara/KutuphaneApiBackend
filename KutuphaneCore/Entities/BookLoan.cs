using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KutuphaneCore.Entities
{
    public class BookLoan : BaseEntity
    {
        public int BookId { get; set; }
        public int UserId { get; set; }
        public string BookTitle { get; set; }
        public string UserName { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime? ExpectedReturnDate { get; set; }
        public DateTime? ActualReturnDate { get; set; }
        public bool IsReturned { get; set; } = false;
        public string? Notes { get; set; }

    }
}
