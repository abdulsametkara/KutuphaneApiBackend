using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KutuphaneServis.Interfaces
{
    public interface IRedisCacheService
    {
        Task<IResponse<string>> GetBookDescriptionAsync(int bookId);
        Task<IResponse<bool>> SetBookDescriptionAsync(int bookId, string description, int expireMinutes = 30);
        Task<IResponse<bool>> ClearBookDescriptionAsync(int bookId);
        Task<IResponse<bool>> ClearAllBookCacheAsync();
        Task<IResponse<bool>> ExistsAsync(int bookId);
    }
}