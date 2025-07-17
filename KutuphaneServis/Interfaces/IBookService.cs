using KutuphaneCore.Entities;
using KutuphaneDataAccess.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KutuphaneServis.Interfaces
{
    public interface IBookService
    {
        IResponse<IEnumerable<BookQueryDto>> ListAll();
        IResponse<BookQueryDto> GetById(int id);
        Task<IResponse<BookCreateDto>> Create(BookCreateDto book);
        Task<IResponse<BookUpdateDto>> Update(BookUpdateDto bookUpdateDto);
        IResponse<BookQueryDto> Delete(int id);
        IResponse<IEnumerable<BookQueryDto>> GetByName(string book);
        IResponse<IEnumerable<BookQueryDto>> GetBooksByCategoryId(int categoryId);
        IResponse<IEnumerable<BookQueryDto>> GetBooksByAuthorId(int authorId);

    }
}
