using KutuphaneServis.Interfaces;
using KutuphaneServis.Response;
using StackExchange.Redis;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;


namespace KutuphaneServis.Service
{

    public class RedisCacheService : IRedisCacheService
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly IDatabase _database;
        private readonly ILogger<RedisCacheService> _logger;
        private readonly IBookService _bookService;
        private const string BookDescriptionKeyFormat = "book:{0}:description";
        public RedisCacheService(
            IConnectionMultiplexer redis,
            ILogger<RedisCacheService> logger, 
            IBookService bookService
            )
        {
            _redis = redis;
            _database = redis.GetDatabase();
            _logger = logger;
            _bookService = bookService;
        }

        public Task<IResponse<string>> GetBookDescriptionAsync(int bookId)
        {
            try
            {
                var key = string.Format(BookDescriptionKeyFormat, bookId);
                var cachedDescription =  _database.StringGet(key);
                if (cachedDescription.HasValue)
                {
                    _logger.LogInformation("Kitap açıklaması önbellekten alındı, kitap ID: {BookId}", bookId);
                    return Task.FromResult<IResponse<string>>(ResponseGeneric<string>.Success(cachedDescription.ToString()));
                }

                var bookResponse =  _bookService.GetById(bookId);
                if (bookResponse.IsSuccess && bookResponse.Data != null)
                {
                    var description = bookResponse.Data.Description ?? "Açıklama bulunamadı.";
                    SetBookDescriptionAsync(bookId, description, 30);
                    return Task.FromResult<IResponse<string>>(ResponseGeneric<string>.Success(description, "Açıklama DB'den getirildi ve cache'lendi."));
                }

                _logger.LogWarning("Kitap açıklaması bulunamadı, kitap ID: {BookId}", bookId);
                return Task.FromResult<IResponse<string>>(ResponseGeneric<string>.Error("Kitap açıklaması bulunamadı."));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kitap açıklaması alınırken bir hata oluştu, kitap ID: {BookId}", bookId);
                return Task.FromResult<IResponse<string>>(ResponseGeneric<string>.Error("Bir hata oluştu."));
            }
        }


        public Task<IResponse<bool>> SetBookDescriptionAsync(int bookId, string description, int expireMinutes = 30)
        {
            try
            {
                if (string.IsNullOrEmpty(description))
                {
                    return Task.FromResult<IResponse<bool>>(ResponseGeneric<bool>.Error("Açıklama boş olamaz."));
                }
                var key = string.Format(BookDescriptionKeyFormat, bookId);
                var expiry = TimeSpan.FromMinutes(expireMinutes);
                _database.StringSet(key, description, expiry);
                return Task.FromResult<IResponse<bool>>(ResponseGeneric<bool>.Success(true, "Açıklama önbelleğe kaydedildi."));
            }
            catch
            {
                return Task.FromResult<IResponse<bool>>(ResponseGeneric<bool>.Error("Açıklama önbelleğe kaydedilirken bir hata oluştu."));
            }
        }
        public Task<IResponse<bool>> ClearBookDescriptionAsync(int bookId)
        {
            try
            {
                var key = string.Format(BookDescriptionKeyFormat, bookId);
                var deleted = _database.KeyDelete(key);
                if (deleted)
                {
                    return Task.FromResult<IResponse<bool>>(ResponseGeneric<bool>.Success(true, "Açıklama önbellekten silindi."));
                }
                return Task.FromResult<IResponse<bool>>(ResponseGeneric<bool>.Error("Açıklama önbellekten silinemedi."));
            }
            catch
            {
                return Task.FromResult<IResponse<bool>>(ResponseGeneric<bool>.Error("Açıklama önbellekten silinirken bir hata oluştu."));
            }
        }


        public Task<IResponse<bool>> ClearAllBookCacheAsync()
        {
            try
            {
                var server = _redis.GetServer(_redis.GetEndPoints().First());
                var keys = server.Keys(pattern: "book:*:description");
                var deletedCount = 0;
                foreach (var key in keys)
                {
                    _database.KeyDelete(key);
                    deletedCount++;
                }
                return Task.FromResult<IResponse<bool>>(ResponseGeneric<bool>.Success(true, $"{deletedCount} adet kitap açıklaması önbellekten silindi."));
            }
            catch
            {
                return Task.FromResult<IResponse<bool>>(ResponseGeneric<bool>.Error("Tüm kitap açıklamaları önbellekten silinirken bir hata oluştu."));
            }
        }


        public Task<IResponse<bool>> ExistsAsync(int bookId)
        {
            try
            {
                var key = string.Format(BookDescriptionKeyFormat, bookId);
                var exists = _database.KeyExists(key);
                if (exists)
                {
                    return Task.FromResult<IResponse<bool>>(ResponseGeneric<bool>.Success(true, "Kitap açıklaması önbellekte mevcut."));
                }
                return Task.FromResult<IResponse<bool>>(ResponseGeneric<bool>.Success(false, "Kitap açıklaması önbellekte mevcut değil."));
            }
            catch
            {
                return Task.FromResult<IResponse<bool>>(ResponseGeneric<bool>.Error("Kitap açıklaması önbellekte kontrol edilirken bir hata oluştu."));
            }
        }
    }
}
