using KutuphaneCore.Entities;
using KutuphaneServis.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KutuphaneDataAccess.Repository;
using KutuphaneServis.Response;
using KutuphaneCore.DTOs;
using AutoMapper;

namespace KutuphaneServis.Service
{
    public class AuthorService : IAuthorService
    {
        private readonly IGenericRepository<Author> _authorRepository;
        private readonly IMapper _mapper;

        public AuthorService(IGenericRepository<Author> authorRepository, IMapper mapper)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
        }


        public Task<IResponse<Author>> Create(AuthorCreateDto authorCreateDto)
        {
            try
            {
                if (authorCreateDto == null)
                {
                    return Task.FromResult<IResponse<Author>>(ResponseGeneric<Author>.Error("Yazar bilgileri boş olamaz."));
                }
                
                var newAuthor = _mapper.Map<Author>(authorCreateDto);

                _authorRepository.Create(newAuthor);
                newAuthor.RecordDate = DateTime.Now;
                
                return Task.FromResult<IResponse<Author>>(ResponseGeneric<Author>.Success(newAuthor, "Yazar başarıyla oluşturuldu."));
            }
            catch
            {
                return Task.FromResult<IResponse<Author>>(ResponseGeneric<Author>.Error("Bir hata oluştu."));

            }
        }
        public IResponse<Author> Delete(int id)
        {
            try
            {
                //Önce Entity varmı onu bul
                var author = _authorRepository.GetByIdAsync(id).Result;

                if (author == null)
                {
                    return ResponseGeneric<Author>.Error("Yazar bulunamadı.");
                }
                //Entity varsa sil
                _authorRepository.Delete(author);
                return ResponseGeneric<Author>.Success(null, "Yazar başarıyla silindi.");
            }
            catch
            {
                return ResponseGeneric<Author>.Error("Bir hata oluştu.");
            }

        }

        public IResponse<AuthorQueryDto> GetById(int authorId)
        {
            try
            {
                var author = _authorRepository.GetByIdAsync(authorId).Result;

                var authorQueryDto = _mapper.Map<AuthorQueryDto>(author);

                if (author == null)
                {
                    return ResponseGeneric<AuthorQueryDto>.Success(null, "Yazar bulunamadı.");
                }
                return ResponseGeneric<AuthorQueryDto>.Success(authorQueryDto, "Yazar başarıyla bulundu");
            }
            catch
            {
                return ResponseGeneric<AuthorQueryDto>.Error("Bir hata oluştu.");
            }
        }

        public IResponse<IEnumerable<AuthorQueryDto>> GetByName(string name)
        {
            try
            {
                var authorList = _authorRepository.GetAll().Where(x => x.Name == name).ToList();
                var authorQueryDtos = _mapper.Map<IEnumerable<AuthorQueryDto>>(authorList);

                if (authorQueryDtos == null || authorQueryDtos.Count() == 0)
                {
                    return ResponseGeneric<IEnumerable<AuthorQueryDto>>.Success(new List<AuthorQueryDto>(), "Yazar bulunamadı");
                }

                return ResponseGeneric<IEnumerable<AuthorQueryDto>>.Success(authorQueryDtos, "Yazar başarıyla bulundu.");
            }
            catch
            {
                return ResponseGeneric<IEnumerable<AuthorQueryDto>>.Error("Bir hata oluştu.");
            }
        }

        public IResponse<IEnumerable<AuthorQueryDto>> ListAll()
        {
            try
            {
                var allAuthors = _authorRepository.GetAll().ToList();

                var authorQueryDtos = _mapper.Map<IEnumerable<AuthorQueryDto>>(allAuthors); 

                if (allAuthors.Count == 0 || allAuthors == null)
                {
                    return ResponseGeneric<IEnumerable<AuthorQueryDto>>.Error("Yazarlar bulunamadı");
                }
                return ResponseGeneric<IEnumerable<AuthorQueryDto>>.Success(authorQueryDtos, "Yazarlar listelendi.");
            }
            catch
            {
                return ResponseGeneric<IEnumerable<AuthorQueryDto>>.Error("Bir hata oluştu.");
            }
        }

        public Task<IResponse<AuthorUpdateDto>> Update(AuthorUpdateDto authorUpdate)
        {
            try
            {
                var author = _authorRepository.GetByIdAsync(authorUpdate.Id).Result;
                
                if (author == null)
                {
                    return Task.FromResult<IResponse<AuthorUpdateDto>>(ResponseGeneric<AuthorUpdateDto>.Error("Yazar bulunamadı."));
                }
                
                if (authorUpdate.Name != null)
                {
                    author.Name = authorUpdate.Name;
                }
                
                if (authorUpdate.Surname != null)
                {
                    author.Surname = authorUpdate.Surname;
                }
                
                if (authorUpdate.PlaceofBirth != null)
                {
                    author.PlaceofBirth = authorUpdate.PlaceofBirth;
                }
                
                if (authorUpdate.YearofBirth != null)
                {
                    author.YearofBirth = authorUpdate.YearofBirth.Value;
                }
                _authorRepository.Update(author);
                return Task.FromResult<IResponse<AuthorUpdateDto>>(ResponseGeneric<AuthorUpdateDto>.Success(authorUpdate, "Yazar başarıyla güncellendi."));
            }
            catch
            {
                return Task.FromResult<IResponse<AuthorUpdateDto>>(ResponseGeneric<AuthorUpdateDto>.Error("Bir hata oluştu."));
            }
        }
    }
}
