using NetWatchApp.Classes.Models;
using NetWatchApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetWatchApp.Classes.Repositories
{
    public class ViewingHistoryRepository
    {
        private readonly JsonDataService _jsonDataService;

        public ViewingHistoryRepository()
        {
            _jsonDataService = new JsonDataService();
        }

        public List<ViewingHistory> GetAll()
        {
            return _jsonDataService.GetAllViewingHistories();
        }

        public ViewingHistory GetById(int id)
        {
            return _jsonDataService.GetViewingHistoryById(id);
        }

        public List<ViewingHistory> GetByUser(int userId)
        {
            return _jsonDataService.GetViewingHistoriesByUser(userId);
        }

        public List<ViewingHistory> GetByContent(int contentId)
        {
            return _jsonDataService.GetViewingHistoriesByContent(contentId);
        }

        public ViewingHistory GetByUserAndContent(int userId, int contentId)
        {
            return _jsonDataService.GetViewingHistoryByUserAndContent(userId, contentId);
        }

        public void Add(ViewingHistory viewingHistory)
        {
            _jsonDataService.AddViewingHistory(viewingHistory);
        }

        public void Update(ViewingHistory viewingHistory)
        {
            _jsonDataService.UpdateViewingHistory(viewingHistory);
        }

        public void Delete(int id)
        {
            _jsonDataService.DeleteViewingHistory(id);
        }

        public List<Content> GetRecentlyWatchedContent(int userId, int count = 5)
        {
            return _jsonDataService.GetRecentlyWatchedContent(userId, count);
        }
    }
}

