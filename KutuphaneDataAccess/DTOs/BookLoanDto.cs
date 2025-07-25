using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KutuphaneDataAccess.DTOs
{
    public class BookLoanDto
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int UserId { get; set; }
        public string BookTitle { get; set; }
        public string UserName { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime? ExpectedReturnDate { get; set; }
        public DateTime? ActualReturnDate { get; set; }
        public bool IsReturned { get; set; }
        public string? Notes { get; set; }

        // Calculated fields
        public int DaysRemaining { get; set; }
        public bool IsOverdue { get; set; }
    }
    public class BookLoanCreateDto
    {
        public int BookId { get; set; }
        public int UserId { get; set; }
        public string? Notes { get; set; }
    }

    public class BookAvailabilityDto
    {
        public int BookId { get; set; }
        public string BookTitle { get; set; }
        public int TotalCopies { get; set; }
        public int AvailableCopies { get; set; }
        public int BorrowedCopies { get; set; }
        public bool IsAvailable { get; set; }
        public string AvailabilityMessage { get; set; }
    }
}
