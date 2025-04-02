using Microsoft.EntityFrameworkCore;
using NetWatchApp.Classes.Models;
using NetWatchApp.Data.EntityFramework;
using NetWatchApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetWatchApp.Classes.Repositories
{
    public class RatingRepository : IRatingRepository
    {
        private readonly NetWatchDbContext _context;

        public RatingRepository(NetWatchDbContext context)
        {
            _context = context;
        }

        public List<Rating> GetAll()
        {
            return _context.Ratings
                .Include(r => r.User)
                .Include(r => r.Content)
                .ToList();
        }

        public Rating GetById(int id)
        {
            return _context.Ratings
                .Include(r => r.User)
                .Include(r => r.Content)
                .FirstOrDefault(r => r.Id == id);
        }

        public List<Rating> GetByUser(int userId)
        {
            return _context.Ratings
                .Include(r => r.Content)
                .Where(r => r.UserId == userId)
                .ToList();
        }

        public List<Rating> GetByContent(int contentId)
        {
            return _context.Ratings
                .Include(r => r.User)
                .Where(r => r.ContentId == contentId)
                .ToList();
        }

        public Rating GetByUserAndContent(int userId, int contentId)
        {
            return _context.Ratings
                .FirstOrDefault(r => r.UserId == userId && r.ContentId == contentId);
        }

        public void Add(Rating rating)
        {
            // Check if user already rated this content
            var existingRating = GetByUserAndContent(rating.UserId, rating.ContentId);
            if (existingRating != null)
            {
                throw new Exception("User has already rated this content.");
            }

            _context.Ratings.Add(rating);
            _context.SaveChanges();
        }

        public void Update(Rating rating)
        {
            var existingRating = _context.Ratings.Find(rating.Id);
            if (existingRating == null)
            {
                throw new Exception($"Rating with ID {rating.Id} not found.");
            }

            existingRating.Score = rating.Score;
            existingRating.Comment = rating.Comment;
            existingRating.RatingDate = rating.RatingDate;

            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var rating = _context.Ratings.Find(id);
            if (rating != null)
            {
                _context.Ratings.Remove(rating);
                _context.SaveChanges();
            }
        }

        public double GetAverageRatingForContent(int contentId)
        {
            var ratings = GetByContent(contentId);
            if (ratings.Count == 0)
                return 0;

            return Math.Round(ratings.Average(r => r.Score), 1);
        }
    }
}

