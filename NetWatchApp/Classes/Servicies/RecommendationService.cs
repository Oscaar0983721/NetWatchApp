using Microsoft.EntityFrameworkCore;
using NetWatchApp.Classes.Models;
using NetWatchApp.Data.EntityFramework;
using NetWatchApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetWatchApp.Classes.Services
{
    public class RecommendationService : IRecommendationService
    {
        private readonly NetWatchDbContext _context;

        public RecommendationService(NetWatchDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Content>> GetRecommendationsForUserAsync(int userId, int count = 10)
        {
            // Get user's viewing history
            var userViewingHistory = await _context.ViewingHistories
                .Where(vh => vh.UserId == userId)
                .Include(vh => vh.Content)
                .ToListAsync();

            // Get user's ratings
            var userRatings = await _context.Ratings
                .Where(r => r.UserId == userId)
                .Include(r => r.Content)
                .ToListAsync();

            // Get all content
            var allContent = await _context.Contents
                .Include(c => c.Episodes)
                .ToListAsync();

            // Get content IDs that the user has already watched
            var watchedContentIds = userViewingHistory.Select(vh => vh.ContentId).Distinct().ToList();

            // Get content that the user hasn't watched yet
            var unwatchedContent = allContent.Where(c => !watchedContentIds.Contains(c.Id)).ToList();

            // If user has no history, return popular content
            if (!userViewingHistory.Any())
            {
                return await GetPopularContentAsync(count);
            }

            // Get user's preferred genres based on viewing history and ratings
            var genreScores = new Dictionary<string, double>();

            // Add scores from viewing history
            foreach (var vh in userViewingHistory)
            {
                string genre = vh.Content.Genre;
                if (!string.IsNullOrEmpty(genre))
                {
                    if (!genreScores.ContainsKey(genre))
                        genreScores[genre] = 0;

                    genreScores[genre] += 1;
                }
            }

            // Add scores from ratings (higher weight for highly rated content)
            foreach (var rating in userRatings)
            {
                string genre = rating.Content.Genre;
                if (!string.IsNullOrEmpty(genre))
                {
                    if (!genreScores.ContainsKey(genre))
                        genreScores[genre] = 0;

                    genreScores[genre] += rating.Score * 0.5; // Weight ratings by score
                }
            }

            // Calculate a score for each unwatched content based on genre preference
            var contentScores = new Dictionary<Content, double>();

            foreach (var content in unwatchedContent)
            {
                double score = 0;

                // Add score based on genre preference
                if (!string.IsNullOrEmpty(content.Genre) && genreScores.ContainsKey(content.Genre))
                {
                    score += genreScores[content.Genre];
                }

                // Add score based on release year (newer content gets higher score)
                score += (content.ReleaseYear - 2000) * 0.1;

                // Add score based on average rating
                var avgRating = await _context.Ratings
                    .Where(r => r.ContentId == content.Id)
                    .Select(r => (double)r.Score)
                    .DefaultIfEmpty(0)
                    .AverageAsync();

                score += avgRating * 2;

                contentScores[content] = score;
            }

            // Sort content by score and return top recommendations
            return contentScores
                .OrderByDescending(cs => cs.Value)
                .Select(cs => cs.Key)
                .Take(count)
                .ToList();
        }

        public async Task<IEnumerable<Content>> GetSimilarContentAsync(int contentId, int count = 5)
        {
            // Get the target content
            var targetContent = await _context.Contents
                .FindAsync(contentId);

            if (targetContent == null)
                return new List<Content>();

            // Get all content except the target
            var allOtherContent = await _context.Contents
                .Where(c => c.Id != contentId)
                .ToListAsync();

            // Calculate similarity scores
            var similarityScores = new Dictionary<Content, double>();

            foreach (var content in allOtherContent)
            {
                double score = 0;

                // Same genre
                if (content.Genre == targetContent.Genre)
                    score += 3;

                // Same type (movie/series)
                if (content.Type == targetContent.Type)
                    score += 1;

                // Similar release year
                int yearDifference = Math.Abs(content.ReleaseYear - targetContent.ReleaseYear);
                if (yearDifference <= 5)
                    score += (5 - yearDifference) * 0.2;

                // Same platform
                if (content.Platform == targetContent.Platform)
                    score += 1;

                similarityScores[content] = score;
            }

            // Return the most similar content
            return similarityScores
                .OrderByDescending(ss => ss.Value)
                .Select(ss => ss.Key)
                .Take(count)
                .ToList();
        }

        public async Task<IEnumerable<Content>> GetPopularContentAsync(int count = 10)
        {
            // Get all content with their view counts and average ratings
            var contentWithStats = await _context.Contents
                .Select(c => new
                {
                    Content = c,
                    ViewCount = _context.ViewingHistories.Count(vh => vh.ContentId == c.Id),
                    AvgRating = _context.Ratings
                        .Where(r => r.ContentId == c.Id)
                        .Select(r => (double)r.Score)
                        .DefaultIfEmpty(0)
                        .Average()
                })
                .ToListAsync();

            // Calculate a popularity score based on views and ratings
            var popularityScores = new Dictionary<Content, double>();

            foreach (var item in contentWithStats)
            {
                double score = item.ViewCount * 0.7 + item.AvgRating * 3;
                popularityScores[item.Content] = score;
            }

            // Return the most popular content
            return popularityScores
                .OrderByDescending(ps => ps.Value)
                .Select(ps => ps.Key)
                .Take(count)
                .ToList();
        }
    }
}