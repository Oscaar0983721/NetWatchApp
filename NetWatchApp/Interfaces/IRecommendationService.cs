using NetWatchApp.Classes.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetWatchApp.Interfaces
{
    public interface IRecommendationService
    {
        Task<IEnumerable<Content>> GetRecommendationsForUserAsync(int userId, int count = 10);
        Task<IEnumerable<Content>> GetSimilarContentAsync(int contentId, int count = 5);
        Task<IEnumerable<Content>> GetPopularContentAsync(int count = 10);
    }
}