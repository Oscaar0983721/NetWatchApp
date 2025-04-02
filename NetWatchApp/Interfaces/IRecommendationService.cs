using NetWatchApp.Classes.Models;
using System.Collections.Generic;

namespace NetWatchApp.Interfaces
{
    public interface IRecommendationService
    {
        List<Content> GetRecommendedContent(int userId, int count = 5);
        List<Content> GetTopRatedContent(int count = 5);
        List<Content> GetSimilarContent(int contentId, int count = 5);
    }
}

