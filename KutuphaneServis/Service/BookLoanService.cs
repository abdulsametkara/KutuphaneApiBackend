using Azure;
using KutuphaneCore.Entities;
using KutuphaneDataAccess.DTOs;
using KutuphaneDataAccess.Repository;
using KutuphaneServis.Interfaces;
using KutuphaneServis.Response;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KutuphaneServis.Service
{
    public class BookLoanService : IBookLoanService
    {
        private readonly IGenericRepository<BookLoan> _bookLoanRepository;
        private readonly IGenericRepository<Book> _bookRepository;
        private readonly IGenericRepository<User> _userRepository;
        private readonly ILogger<BookLoanService> _logger;

        public BookLoanService(
            IGenericRepository<BookLoan> bookLoanRepository,
            IGenericRepository<Book> bookRepository,
            IGenericRepository<User> userRepository,
            ILogger<BookLoanService> logger
            )
        {
            _bookLoanRepository = bookLoanRepository;
            _bookRepository = bookRepository;
            _userRepository = userRepository;
            _logger = logger;
        }
        public async Task<IResponse<BookLoanCreateDto>> BorrowBookAsync(int bookId, int userId)
        {
            try
            {
                var book = await _bookRepository.GetByIdAsync(bookId);
                if (book == null)
                {
                    return ResponseGeneric<BookLoanCreateDto>.Error("Kitap bulunamadı.");
                }

                if (book.AvailableCopies <= 0)
                {
                    return ResponseGeneric<BookLoanCreateDto>.Error("Kitap mevcut değil.");
                }

                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    return ResponseGeneric<BookLoanCreateDto>.Error("Kullanıcı bulunamadı.");
                }

                var existingLoan = _bookLoanRepository.GetAll()
                    .FirstOrDefault(loan => loan.BookId == bookId && loan.UserId == userId && !loan.IsReturned);
                if (existingLoan != null)
                {
                    return ResponseGeneric<BookLoanCreateDto>.Error("Kullanıcı zaten bu kitabı ödünç almış.");
                }

                var newBookLoan = new BookLoan
                {
                    BookId = bookId,
                    UserId = userId,
                    LoanDate = DateTime.Now,
                    ExpectedReturnDate = DateTime.Now.AddDays(14),
                    BookTitle = book.Title,
                    UserName = user.Username,
                    IsReturned = false,
                };

                _bookLoanRepository.Create(newBookLoan);
                book.AvailableCopies--;
                await _bookLoanRepository.Save();

                var bookLoanCreateDto = new BookLoanCreateDto
                {
                    BookId = newBookLoan.BookId,
                    UserId = newBookLoan.UserId,
                    Notes = newBookLoan.Notes
                };

                return ResponseGeneric<BookLoanCreateDto>.Success(bookLoanCreateDto, "Kitap başarıyla ödünç alındı.");
            }
            catch
            {
                return ResponseGeneric<BookLoanCreateDto>.Error("Kitap ödünç alma işlemi sırasında bir hata oluştu.");
            }
        }

        public IResponse<IEnumerable<BookLoanDto>> GetActiveLoans(int userId)
        {
            try
            {
              
                var user = _userRepository.GetByIdAsync(userId).Result; 
                if (user == null)
                {
                    return ResponseGeneric<IEnumerable<BookLoanDto>>.Error("Kullanıcı bulunamadı.");
                }

                var activeLoans = _bookLoanRepository.GetAll()
                    .Where(loan => loan.UserId == userId && !loan.IsReturned)
                    .Select(loan => new BookLoanDto
                    {
                        Id = loan.Id,
                        BookId = loan.BookId,
                        UserId = loan.UserId,
                        BookTitle = loan.BookTitle,
                        UserName = loan.UserName,
                        LoanDate = loan.LoanDate,
                        ExpectedReturnDate = loan.ExpectedReturnDate,
                        ActualReturnDate = loan.ActualReturnDate,
                        IsReturned = loan.IsReturned,
                        Notes = loan.Notes,
                        IsOverdue = loan.ExpectedReturnDate < DateTime.Now
                    }).ToList();

                return ResponseGeneric<IEnumerable<BookLoanDto>>.Success(activeLoans, "Aktif ödünç kitaplar başarıyla listelendi.");
            }
            catch
            {
                return ResponseGeneric<IEnumerable<BookLoanDto>>.Error("Aktif ödünç kitaplar listelenirken bir hata oluştu.");
            }
        }

        public IResponse<IEnumerable<BookLoanDto>> ListAll()
        {
            try
            {
                var loans = _bookLoanRepository.GetAll()
                    .Select(loan => new BookLoanDto
                    {
                        Id = loan.Id,
                        BookId = loan.BookId,
                        UserId = loan.UserId,
                        BookTitle = loan.BookTitle,
                        UserName = loan.UserName,
                        LoanDate = loan.LoanDate,
                        ExpectedReturnDate = loan.ExpectedReturnDate,
                        ActualReturnDate = loan.ActualReturnDate,
                        IsReturned = loan.IsReturned,
                        Notes = loan.Notes,
                        IsOverdue = loan.ExpectedReturnDate < DateTime.Now
                    }).ToList();
                return ResponseGeneric<IEnumerable<BookLoanDto>>.Success(loans, "Tüm ödünç kitaplar başarıyla listelendi.");
            }
            catch
            {
                return ResponseGeneric<IEnumerable<BookLoanDto>>.Error("Ödünç kitaplar listelenirken bir hata oluştu.");
            }
        }

        public async Task<IResponse<BookLoanDto>> ReturnBook(int loanId)
        {
            try
            {
                var loan = _bookLoanRepository.GetAll().FirstOrDefault(l => l.Id == loanId && !l.IsReturned);
                if (loan == null)
                {
                    return ResponseGeneric<BookLoanDto>.Error("Ödünç kaydı bulunamadı.");
                }
                if (loan.IsReturned)
                {
                    return ResponseGeneric<BookLoanDto>.Error("Kitap zaten iade edilmiş.");
                }
                loan.IsReturned = true;
                loan.ActualReturnDate = DateTime.Now;
                _bookLoanRepository.Update(loan);
                var book = _bookRepository.GetByIdAsync(loan.BookId).Result;
                if (book != null)
                {
                    book.AvailableCopies++;
                    _bookRepository.Update(book);
                }
                await _bookLoanRepository.Save();

                var loanDto = new BookLoanDto
                {
                    Id = loan.Id,
                    BookId = loan.BookId,
                    UserId = loan.UserId,
                    BookTitle = loan.BookTitle,
                    UserName = loan.UserName,
                    LoanDate = loan.LoanDate,
                    ExpectedReturnDate = loan.ExpectedReturnDate,
                    ActualReturnDate = loan.ActualReturnDate,
                    IsReturned = loan.IsReturned,
                    Notes = loan.Notes,
                    IsOverdue = loan.ExpectedReturnDate < DateTime.Now
                };

                return ResponseGeneric<BookLoanDto>.Success(loanDto, "Kitap başarıyla iade edildi.");
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Kitap iade işlemi sırasında bir hata oluştu.");
                return ResponseGeneric<BookLoanDto>.Error("Kitap iade işlemi sırasında bir hata oluştu.");
            }
        }
    }
}
