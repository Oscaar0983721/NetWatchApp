using NetWatchApp.Classes.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetWatchApp.Interfaces
{
    public interface IRatingRepository
    {
        Task<Rating> GetByIdAsync(int id);
        Task<IEnumerable<Rating>> GetByUserIdAsync(int userId);
        Task<IEnumerable<Rating>> GetByContentIdAsync(int contentId);
        Task<double> GetAverageRatingForContentAsync(int contentId);
        Task<bool> AddAsync(Rating rating);
        Task<bool> UpdateAsync(Rating rating);
        Task<bool> DeleteAsync(int id);
    }
}