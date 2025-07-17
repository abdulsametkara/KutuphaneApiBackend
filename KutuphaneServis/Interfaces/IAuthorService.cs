using KutuphaneCore.DTOs;
using KutuphaneCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KutuphaneServis.Interfaces
{
    public interface IAuthorService
    {
        IResponse<IEnumerable<AuthorQueryDto>> ListAll();
        IResponse<AuthorQueryDto> GetById(int authorId);
        Task<IResponse<Author>> Create(AuthorCreateDto author);
        Task<IResponse<AuthorUpdateDto>> Update(AuthorUpdateDto authorUpdate);
        IResponse<Author> Delete(int id);
        IResponse<IEnumerable<AuthorQueryDto>> GetByName(string name);

    }
}
