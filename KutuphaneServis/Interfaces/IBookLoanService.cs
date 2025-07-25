using KutuphaneCore.Entities;
using KutuphaneDataAccess.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KutuphaneServis.Interfaces
{
    public interface IBookLoanService
    {
        Task<IResponse<BookLoanCreateDto>> BorrowBookAsync(int bookId, int userId);
        IResponse<IEnumerable<BookLoanDto>> GetActiveLoans(int userId);
        IResponse<IEnumerable<BookLoanDto>> ListAll();
        Task<IResponse<BookLoanDto>> ReturnBook(int loanId);
    }
}
