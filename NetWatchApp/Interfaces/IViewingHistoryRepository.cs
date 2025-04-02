using NetWatchApp.Classes.Models;
using System.Collections.Generic;

namespace NetWatchApp.Interfaces
{
    public interface IViewingHistoryRepository
    {
        List<ViewingHistory> GetAll();
        ViewingHistory GetById(int id);
        List<ViewingHistory> GetByUser(int userId);
        List<ViewingHistory> GetByContent(int contentId);
        ViewingHistory GetByUserAndContent(int userId, int contentId);
        void Add(ViewingHistory viewingHistory);
        void Update(ViewingHistory viewingHistory);
        void Delete(int id);
        List<Content> GetRecentlyWatchedContent(int userId, int count = 5);
    }
}

