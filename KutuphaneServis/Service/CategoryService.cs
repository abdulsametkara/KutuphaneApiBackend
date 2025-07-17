using KutuphaneCore.Entities;
using KutuphaneServis.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KutuphaneDataAccess.Repository;
using KutuphaneServis.Response;
using AutoMapper;
using KutuphaneDataAccess.DTOs;

namespace KutuphaneServis.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly IGenericRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;
        public CategoryService(IGenericRepository<Category> categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }
        public async Task<IResponse<CategoryCreateDto>> Create(CategoryCreateDto category)
        {
            try
            {
                if (category == null)
                {
                    return ResponseGeneric<CategoryCreateDto>.Error("Kategori bilgileri boş bırakılamaz.");
                }

                //DTO yu entityye dönüştür
                var categoryEntity = _mapper.Map<Category>(category);

                categoryEntity.RecordDate = DateTime.Now;
                _categoryRepository.Create(categoryEntity);
                _categoryRepository.Save();


                return ResponseGeneric<CategoryCreateDto>.Success(null, "Kategori başarıyla oluşturuldu.");
            }
            catch
            {
                return ResponseGeneric<CategoryCreateDto>.Error("Bir hata oluştu.");
            }
        }

        public IResponse<CategoryQueryDto> Delete(int id)
        {
            try
            {
                var category = _categoryRepository.GetByIdAsync(id).Result;
                if (category == null)
                {
                    return ResponseGeneric<CategoryQueryDto>.Error("Kategori bulunamadı.");
                }

                _categoryRepository.Delete(category);
                _categoryRepository.Save();
                return ResponseGeneric<CategoryQueryDto>.Success(null, "Kategori başarıyla silindi");
            }
            catch
            {
                return ResponseGeneric<CategoryQueryDto>.Error("Bir hata oluştu.");
            }
        }

        public IResponse<CategoryQueryDto> GetById(int id)
        {
            try
            {
                var category = _categoryRepository.GetByIdAsync(id).Result;

                var categoryDto = _mapper.Map<CategoryQueryDto>(category);

                if (categoryDto == null)
                {
                    return ResponseGeneric<CategoryQueryDto>.Success(null, "Kategori bulunamadı.");
                }


                return ResponseGeneric<CategoryQueryDto>.Success(categoryDto, "Kategori başarıyla bulundu.");
            }
            catch
            {
                return ResponseGeneric<CategoryQueryDto>.Error("Bir hata oluştu.");
            }
        }

        public IResponse<IEnumerable<CategoryQueryDto>> GetByName(string name)
        {
            try
            {
                var categories = _categoryRepository.GetAll().Where(x => x.Name.ToLower().Contains(name.ToLower())).ToList();
                var categoryDtos = _mapper.Map<IEnumerable<CategoryQueryDto>>(categories);
                if (categoryDtos == null || categoryDtos.ToList().Count == 0)
                {
                    return ResponseGeneric<IEnumerable<CategoryQueryDto>>.Error("Kategori bulunamadı.");
                }

                return ResponseGeneric<IEnumerable<CategoryQueryDto>>.Success(categoryDtos , "Kategori başarıyla bulundu.");
            }
            catch
            {
                return ResponseGeneric<IEnumerable<CategoryQueryDto>>.Error("Bir hata oluştu");

            }
        }

        public IResponse<IEnumerable<CategoryQueryDto>> ListAll()
        {
            try
            {
                var categories = _categoryRepository.GetAll().ToList();
                var categoryDtos = _mapper.Map<IEnumerable<CategoryQueryDto>>(categories);

                if (categoryDtos == null || categoryDtos.ToList().Count == 0)
                {
                    return ResponseGeneric<IEnumerable<CategoryQueryDto>>.Error("Kategori bulunamadı.");
                }

                return ResponseGeneric<IEnumerable<CategoryQueryDto>>.Success(categoryDtos, "Kategoriler başarıyla döndürüldü.");
            }
            catch
            {
                return ResponseGeneric<IEnumerable<CategoryQueryDto>>.Error("Bir hata oluştu.");
            }
        }

        public Task<IResponse<CategoryUpdateDto>> Update(CategoryUpdateDto categoryUpdateDto)
        {
            try
            {
                var category = _categoryRepository.GetByIdAsync(categoryUpdateDto.Id).Result;
                if (category == null)
                {
                    return Task.FromResult<IResponse<CategoryUpdateDto>>(ResponseGeneric<CategoryUpdateDto>.Error("Kategori bulunamadı."));
                }

                if (!string.IsNullOrEmpty(categoryUpdateDto.Name))
                {
                    category.Name = categoryUpdateDto.Name;
                }
                if (!string.IsNullOrEmpty(categoryUpdateDto.Description))
                {
                    category.Description = categoryUpdateDto.Description;
                }

                _categoryRepository.Update(category);
                return Task.FromResult<IResponse<CategoryUpdateDto>>(ResponseGeneric<CategoryUpdateDto>.Success(null, "Kategori başarıyla güncellendi."));
            }
            catch
            {
                
                return Task.FromResult<IResponse<CategoryUpdateDto>>(ResponseGeneric<CategoryUpdateDto>.Error("Bir hata oluştu."));
            }
        }
    }
}
