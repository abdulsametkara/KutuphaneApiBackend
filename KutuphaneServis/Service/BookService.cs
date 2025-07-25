using AutoMapper;
using KutuphaneCore.Entities;
using KutuphaneDataAccess.DTOs;
using KutuphaneDataAccess.Repository;
using KutuphaneServis.Interfaces;
using KutuphaneServis.Response;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace KutuphaneServis.Service
{
    public class BookService : IBookService
    {
        private readonly IGenericRepository<Book> _bookRepository;
        private readonly IGenericRepository<UploadedFiles> _uploadedFilesRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<BookService> _logger;

        public BookService(IGenericRepository<Book> bookRepository,IGenericRepository<UploadedFiles> uploadedFilesRepository, IMapper mapper, ILogger<BookService> logger)
        {
            _bookRepository = bookRepository;
            _uploadedFilesRepository = uploadedFilesRepository;
            _mapper = mapper;
            _logger = logger;
        }


        public Task<IResponse<BookCreateDto>> Create(BookCreateDto createBookModel)
        {
            try
            {
                if (createBookModel == null)
                {
                    return Task.FromResult<IResponse<BookCreateDto>>(ResponseGeneric<BookCreateDto>.Error("Kitap bilgileri boş bırakılamaz."));
                }


                var newBook = new Book
                {
                    Title = createBookModel.Title,
                    Description = createBookModel.Description,
                    CountofPage = createBookModel.CountofPage,
                    AuthorId = createBookModel.AuthorId,
                    CategoryId = createBookModel.CategoryId,
                    FileKey = createBookModel.FileKey,
                    TotalCopies = createBookModel.TotalCopies,
                    AvailableCopies = createBookModel.AvailableCopies,
                    RecordDate = DateTime.Now
                };

                //mapleme işlemi yapılacak

                _bookRepository.Create(newBook);
                _bookRepository.Save();

                _logger.LogInformation("Kitap başarıyla oluşturuldu: {Title}", newBook.Title);
                return Task.FromResult<IResponse<BookCreateDto>>(ResponseGeneric<BookCreateDto>.Success(null, "Kitap başarıyla oluşturuldu."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kitap oluşturulurken bir hata oluştu.", createBookModel);
                return Task.FromResult<IResponse<BookCreateDto>>(ResponseGeneric<BookCreateDto>.Error("Bir hata oluştu."));
            }
        }

        public IResponse<BookQueryDto> Delete(int id)
        {
            try
            {
                var book = _bookRepository.GetByIdAsync(id).Result;


                if (book == null)
                {
                    return ResponseGeneric<BookQueryDto>.Error("Kitap bulunamadı.");
                }
                _bookRepository.Delete(book);
                _bookRepository.Save();
                _logger.LogInformation("Kitap başarıyla silindi:", book.Title);

                return ResponseGeneric<BookQueryDto>.Success(null, "Kitap başarıyla silindi.");
            }
            catch
            {
                _logger.LogError("Kitap silinirken bir hata oluştu.",id);
                return ResponseGeneric<BookQueryDto>.Error("Bir hata oluştu.");
            }
        }

        public IResponse<BookQueryDto> GetById(int id)
        {
            try
            {
                var book = _bookRepository.GetByIdAsync(id).Result;
                var bookDto = _mapper.Map<BookQueryDto>(book);


                if (bookDto == null)
                {
                    return ResponseGeneric<BookQueryDto>.Success(null,"Kitap bulunamadı.");
                }

                return ResponseGeneric<BookQueryDto>.Success(bookDto, "Kitap başarıyla bulundu.");
            }
            catch
            {
                return ResponseGeneric<BookQueryDto>.Error("Bir hata oluştu.");
            }
        }

        public IResponse<IEnumerable<BookQueryDto>> GetByName(string name)
        {
            try
            {
                var books = _bookRepository.GetAll().Where(x => x.Title == name).ToList();

                var bookDtos = _mapper.Map<IEnumerable<BookQueryDto>>(books);

                if (bookDtos.ToList().Count == 0 || books == null)
                {
                    return ResponseGeneric<IEnumerable<BookQueryDto>>.Error("Kitap bulunamadı.");
                }

                return ResponseGeneric<IEnumerable<BookQueryDto>>.Success(bookDtos, "Kitaplar döndürüldü.");
            }
            catch
            {
                return ResponseGeneric<IEnumerable<BookQueryDto>>.Error("Bir hata oluştu.");
            }
        }

        public IResponse<IEnumerable<BookQueryDto>> ListAll()
        {
            try
            {
                var bookList = _bookRepository.GetAll().ToList();

                var bookListDto = _mapper.Map<IEnumerable<BookQueryDto>>(bookList);

                if (bookListDto == null || bookListDto.ToList().Count == 0)
                {
                    return ResponseGeneric<IEnumerable<BookQueryDto>>.Error("Kitap bulunamadı.");
                }

                return ResponseGeneric<IEnumerable<BookQueryDto>>.Success(bookListDto, "Kitaplar başarıyla döndürüldü.");
            }
            catch
            {
                return ResponseGeneric<IEnumerable<BookQueryDto>>.Error("Bir hata oluştu.");
            }
        }

        public IResponse<IEnumerable<BookQueryDto>> GetBooksByCategoryId(int categoryId)
        {
            try
            {
                var booksInCategory = _bookRepository.GetAll()
                .Where(b => b.CategoryId == categoryId)
                .ToList();

                var bookDtos = _mapper.Map<IEnumerable<BookQueryDto>>(booksInCategory);
                if (bookDtos == null || bookDtos.ToList().Count == 0)
                {
                    return ResponseGeneric<IEnumerable<BookQueryDto>>.Error("Bu kategoriye ait kitap bulunamadı.");
                }

                return ResponseGeneric<IEnumerable<BookQueryDto>>.Success(bookDtos, "Kategoriye ait kitaplar başarıyla döndürüldü.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Kategori ID'sine göre kitaplar alınırken bir hata oluştu.", categoryId);
                return ResponseGeneric<IEnumerable<BookQueryDto>>.Error("Bir hata oluştu.");            }
        }

        public IResponse<IEnumerable<BookQueryDto>> GetBooksByAuthorId(int authorId)
        {
            try
            {
                var booksByAuthor = _bookRepository.GetAll()
                    .Where(b => b.AuthorId == authorId)
                    .ToList();
                var bookDtos = _mapper.Map<IEnumerable<BookQueryDto>>(booksByAuthor);
                if (bookDtos == null || bookDtos.ToList().Count == 0)
                {
                    return ResponseGeneric<IEnumerable<BookQueryDto>>.Error("Bu yazara ait kitap bulunamadı.");
                }
                return ResponseGeneric<IEnumerable<BookQueryDto>>.Success(bookDtos, "Yazara ait kitaplar başarıyla döndürüldü.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Yazar ID'sine göre kitaplar alınırken bir hata oluştu.", authorId);
                return ResponseGeneric<IEnumerable<BookQueryDto>>.Error("Bir hata oluştu.");
            }
        }



        public Task<IResponse<BookUpdateDto>> Update(BookUpdateDto bookUpdateDto)
        {
            try
            {
                //kitabı dbden bul
                var bookEntity = _bookRepository.GetByIdAsync(bookUpdateDto.Id).Result;

                if (bookEntity == null)
                {
                    return Task.FromResult<IResponse<BookUpdateDto>>(ResponseGeneric<BookUpdateDto>.Error("Kitap bulunamadı."));
                }
                //kitap bilgilerini güncelle
                if (!string.IsNullOrEmpty(bookUpdateDto.Title))
                {
                    bookEntity.Title = bookUpdateDto.Title;
                }
                if (!string.IsNullOrEmpty(bookUpdateDto.Description))
                {
                    bookEntity.Description = bookUpdateDto.Description;
                }
                if (bookUpdateDto.CountofPage != null)
                {
                    bookEntity.CountofPage = bookUpdateDto.CountofPage.Value;
                }
                if (bookUpdateDto.AuthorId != null)
                {
                    bookEntity.AuthorId = bookUpdateDto.AuthorId.Value;
                }
                if (bookUpdateDto.CategoryId != null)
                {
                    bookEntity.CategoryId = bookUpdateDto.CategoryId.Value;
                }
                _bookRepository.Update(bookEntity);
                return Task.FromResult<IResponse<BookUpdateDto>>(ResponseGeneric<BookUpdateDto>.Success(bookUpdateDto, "Kitap başarıyla güncellendi."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kitap güncellenirken bir hata oluştu.", bookUpdateDto);
                return Task.FromResult<IResponse<BookUpdateDto>>(ResponseGeneric<BookUpdateDto>.Error("Bir hata oluştu."));
            }
        }
    }
}
