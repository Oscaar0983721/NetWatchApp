using NetWatchApp.Classes.Models;
using System.Collections.Generic;

namespace NetWatchApp.Interfaces
{
    public interface IRatingRepository
    {
        List<Rating> GetAll();
        Rating GetById(int id);
        List<Rating> GetByUser(int userId);
        List<Rating> GetByContent(int contentId);
        Rating GetByUserAndContent(int userId, int contentId);
        void Add(Rating rating);
        void Update(Rating rating);
        void Delete(int id);
        double GetAverageRatingForContent(int contentId);
    }
}

