using KutuphaneCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KutuphaneServis.Response;
using KutuphaneServis.Interfaces;
using KutuphaneDataAccess.DTOs;
namespace KutuphaneServis.Interfaces
{
    public interface ICategoryService
    {
        IResponse<IEnumerable<CategoryQueryDto>> ListAll();
        IResponse<CategoryQueryDto> GetById(int id);
        Task<IResponse<CategoryCreateDto>> Create(CategoryCreateDto category);
        Task<IResponse<CategoryUpdateDto>> Update(CategoryUpdateDto categoryUpdateDto);
        IResponse<CategoryQueryDto> Delete(int id);

        IResponse<IEnumerable<CategoryQueryDto>> GetByName(string name);
    }
}
