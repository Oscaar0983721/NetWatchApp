using Microsoft.EntityFrameworkCore;
using NetWatchApp.Classes.Models;
using NetWatchApp.Data.EntityFramework;
using NetWatchApp.Interfaces;
using NetWatchApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetWatchApp.Classes.Repositories
{
    public class ViewingHistoryRepository : IViewingHistoryRepository
    {
        private readonly NetWatchDbContext _context;
        private readonly JsonDataService _jsonDataService;

        public ViewingHistoryRepository(NetWatchDbContext context)
        {
            _context = context;
            _jsonDataService = new JsonDataService();
        }

        public List<ViewingHistory> GetAll()
        {
            return _context.ViewingHistories
                .Include(vh => vh.User)
                .Include(vh => vh.Content)
                .ToList();
        }

        public ViewingHistory GetById(int id)
        {
            return _context.ViewingHistories
                .Include(vh => vh.User)
                .Include(vh => vh.Content)
                .FirstOrDefault(vh => vh.Id == id);
        }

        public List<ViewingHistory> GetByUser(int userId)
        {
            return _context.ViewingHistories
                .Include(vh => vh.Content)
                .Where(vh => vh.UserId == userId)
                .ToList();
        }

        public List<ViewingHistory> GetByContent(int contentId)
        {
            return _context.ViewingHistories
                .Include(vh => vh.User)
                .Where(vh => vh.ContentId == contentId)
                .ToList();
        }

        public ViewingHistory GetByUserAndContent(int userId, int contentId)
        {
            return _context.ViewingHistories
                .FirstOrDefault(vh => vh.UserId == userId && vh.ContentId == contentId);
        }

        public void Add(ViewingHistory viewingHistory)
        {
            _context.ViewingHistories.Add(viewingHistory);
            _context.SaveChanges();

            // Save to JSON
            Task.Run(async () => await _jsonDataService.SaveViewingHistoryToJsonAsync(viewingHistory))
                .ConfigureAwait(false);
        }

        public void Update(ViewingHistory viewingHistory)
        {
            var existingHistory = _context.ViewingHistories.Find(viewingHistory.Id);
            if (existingHistory == null)
            {
                throw new Exception($"Viewing history with ID {viewingHistory.Id} not found.");
            }

            existingHistory.WatchDate = viewingHistory.WatchDate;
            existingHistory.WatchedEpisodes = viewingHistory.WatchedEpisodes;

            _context.SaveChanges();

            // Save to JSON after update
            Task.Run(async () => await _jsonDataService.SaveViewingHistoryToJsonAsync(existingHistory))
                .ConfigureAwait(false);
        }

        public void Delete(int id)
        {
            var viewingHistory = _context.ViewingHistories.Find(id);
            if (viewingHistory != null)
            {
                _context.ViewingHistories.Remove(viewingHistory);
                _context.SaveChanges();

                // Note: We don't delete the JSON file to maintain a history
            }
        }

        public List<Content> GetRecentlyWatchedContent(int userId, int count = 5)
        {
            return _context.ViewingHistories
                .Include(vh => vh.Content)
                .ThenInclude(c => c.Episodes)
                .Where(vh => vh.UserId == userId)
                .OrderByDescending(vh => vh.WatchDate)
                .Take(count)
                .Select(vh => vh.Content)
                .ToList();
        }
    }
}

