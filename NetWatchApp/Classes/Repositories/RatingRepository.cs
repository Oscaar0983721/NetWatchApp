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
    public class RatingRepository : IRatingRepository
    {
        private readonly NetWatchDbContext _context;

        public RatingRepository(NetWatchDbContext context)
        {
            _context = context;
        }

        public async Task<Rating> GetByIdAsync(int id)
        {
            return await _context.Ratings
                .Include(r => r.User)
                .Include(r => r.Content)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Rating>> GetByUserIdAsync(int userId)
        {
            return await _context.Ratings
                .Where(r => r.UserId == userId)
                .Include(r => r.Content)
                .OrderByDescending(r => r.RatingDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Rating>> GetByContentIdAsync(int contentId)
        {
            return await _context.Ratings
                .Where(r => r.ContentId == contentId)
                .Include(r => r.User)
                .OrderByDescending(r => r.RatingDate)
                .ToListAsync();
        }

        public async Task<double> GetAverageRatingForContentAsync(int contentId)
        {
            var ratings = await _context.Ratings
                .Where(r => r.ContentId == contentId)
                .ToListAsync();

            if (!ratings.Any())
                return 0;

            return ratings.Average(r => r.Score);
        }

        public async Task<bool> AddAsync(Rating rating)
        {
            try
            {
                // Check if user already rated this content
                var existingRating = await _context.Ratings
                    .FirstOrDefaultAsync(r => r.UserId == rating.UserId && r.ContentId == rating.ContentId);

                if (existingRating != null)
                {
                    // Update existing rating
                    existingRating.Score = rating.Score;
                    existingRating.Comment = rating.Comment;
                    existingRating.RatingDate = DateTime.Now;

                    _context.Ratings.Update(existingRating);
                }
                else
                {
                    // Add new rating
                    await _context.Ratings.AddAsync(rating);
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(Rating rating)
        {
            try
            {
                _context.Ratings.Update(rating);
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
                var rating = await _context.Ratings.FindAsync(id);
                if (rating == null)
                    return false;

                _context.Ratings.Remove(rating);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}