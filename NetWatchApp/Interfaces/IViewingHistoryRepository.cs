using NetWatchApp.Classes.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetWatchApp.Interfaces
{
    public interface IViewingHistoryRepository
    {
        Task<ViewingHistory> GetByIdAsync(int id);
        Task<IEnumerable<ViewingHistory>> GetByUserIdAsync(int userId);
        Task<IEnumerable<ViewingHistory>> GetByContentIdAsync(int contentId);
        Task<bool> AddAsync(ViewingHistory viewingHistory);
        Task<bool> UpdateAsync(ViewingHistory viewingHistory);
        Task<bool> DeleteAsync(int id);
        Task<int> GetTotalWatchTimeForUserAsync(int userId);
    }
}