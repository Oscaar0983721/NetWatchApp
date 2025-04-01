using Microsoft.EntityFrameworkCore;
using NetWatchApp.Classes.Models;
using NetWatchApp.Data.EntityFramework;
using NetWatchApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetWatchApp.Classes.Repositories
{
    public class ViewingHistoryRepository : IViewingHistoryRepository
    {
        private readonly NetWatchDbContext _context;

        public ViewingHistoryRepository(NetWatchDbContext context)
        {
            _context = context;
        }

        public async Task<ViewingHistory> GetByIdAsync(int id)
        {
            return await _context.ViewingHistories
                .Include(vh => vh.Content)
                .Include(vh => vh.Episode)
                .FirstOrDefaultAsync(vh => vh.Id == id);
        }

        public async Task<IEnumerable<ViewingHistory>> GetByUserIdAsync(int userId)
        {
            return await _context.ViewingHistories
                .Where(vh => vh.UserId == userId)
                .Include(vh => vh.Content)
                .Include(vh => vh.Episode)
                .OrderByDescending(vh => vh.ViewDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<ViewingHistory>> GetByContentIdAsync(int contentId)
        {
            return await _context.ViewingHistories
                .Where(vh => vh.ContentId == contentId)
                .Include(vh => vh.User)
                .Include(vh => vh.Episode)
                .OrderByDescending(vh => vh.ViewDate)
                .ToListAsync();
        }

        public async Task<bool> AddAsync(ViewingHistory viewingHistory)
        {
            try
            {
                await _context.ViewingHistories.AddAsync(viewingHistory);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(ViewingHistory viewingHistory)
        {
            try
            {
                _context.ViewingHistories.Update(viewingHistory);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var viewingHistory = await _context.ViewingHistories.FindAsync(id);
                if (viewingHistory == null)
                    return false;

                _context.ViewingHistories.Remove(viewingHistory);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<int> GetTotalWatchTimeForUserAsync(int userId)
        {
            return await _context.ViewingHistories
                .Where(vh => vh.UserId == userId)
                .SumAsync(vh => vh.WatchedMinutes);
        }
    }
}