using NetWatchApp.Classes.Models;
using NetWatchApp.Classes.Repositories;
using NetWatchApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetWatchApp.Classes.Services
{
    public class RecommendationService : IRecommendationService
    {
        private readonly ContentRepository _contentRepository;
        private readonly ViewingHistoryRepository _viewingHistoryRepository;
        private readonly RatingRepository _ratingRepository;

        public RecommendationService(
            ContentRepository contentRepository,
            ViewingHistoryRepository viewingHistoryRepository,
            RatingRepository ratingRepository)
        {
            _contentRepository = contentRepository;
            _viewingHistoryRepository = viewingHistoryRepository;
            _ratingRepository = ratingRepository;
        }

        public List<Content> GetRecommendedContent(int userId, int count = 5)
        {
            var recommendations = new List<Content>();

            // Get user's viewing history
            var viewingHistory = _viewingHistoryRepository.GetByUser(userId);

            // Get user's ratings
            var ratings = _ratingRepository.GetByUser(userId);

            // Get all content
            var allContent = _contentRepository.GetAll();

            // Filter out content the user has already watched
            var watchedContentIds = viewingHistory.Select(vh => vh.ContentId).ToList();
            var unwatchedContent = allContent.Where(c => !watchedContentIds.Contains(c.Id)).ToList();

            // If user has no viewing history, return top-rated content
            if (viewingHistory.Count == 0)
            {
                return GetTopRatedContent(count);
            }

            // Get user's preferred genres based on viewing history and ratings
            var preferredGenres = GetPreferredGenres(userId);

            // Get content matching preferred genres
            var genreBasedRecommendations = unwatchedContent
                .Where(c => preferredGenres.Contains(c.Genre))
                .OrderByDescending(c => c.AverageRating)
                .Take(count)
                .ToList();

            recommendations.AddRange(genreBasedRecommendations);

            // If we don't have enough recommendations, add some top-rated content
            if (recommendations.Count < count)
            {
                var remainingCount = count - recommendations.Count;
                var topRatedContent = GetTopRatedContent(remainingCount * 2); // Get more than needed to filter

                // Filter out content already in recommendations
                var recommendedIds = recommendations.Select(c => c.Id).ToList();
                topRatedContent = topRatedContent
                    .Where(c => !recommendedIds.Contains(c.Id) && !watchedContentIds.Contains(c.Id))
                    .Take(remainingCount)
                    .ToList();

                recommendations.AddRange(topRatedContent);
            }

            return recommendations.Take(count).ToList();
        }

        public List<Content> GetTopRatedContent(int count = 5)
        {
            return _contentRepository.GetAll()
                .OrderByDescending(c => c.AverageRating)
                .Take(count)
                .ToList();
        }

        public List<Content> GetSimilarContent(int contentId, int count = 5)
        {
            var content = _contentRepository.GetById(contentId);
            if (content == null)
            {
                return new List<Content>();
            }

            // Get content with the same genre
            return _contentRepository.GetAll()
                .Where(c => c.Id != contentId && c.Genre == content.Genre)
                .OrderByDescending(c => c.AverageRating)
                .Take(count)
                .ToList();
        }

        private List<string> GetPreferredGenres(int userId)
        {
            var viewingHistory = _viewingHistoryRepository.GetByUser(userId);
            var ratings = _ratingRepository.GetByUser(userId);

            // Get content IDs from viewing history
            var watchedContentIds = viewingHistory.Select(vh => vh.ContentId).ToList();

            // Get content IDs from highly rated content (4-5 stars)
            var highlyRatedContentIds = ratings
                .Where(r => r.Score >= 4)
                .Select(r => r.ContentId)
                .ToList();

            // Combine both lists
            var relevantContentIds = watchedContentIds.Union(highlyRatedContentIds).ToList();

            // Get content objects
            var relevantContent = _contentRepository.GetAll()
                .Where(c => relevantContentIds.Contains(c.Id))
                .ToList();

            // Count genres
            var genreCounts = new Dictionary<string, int>();
            foreach (var content in relevantContent)
            {
                if (genreCounts.ContainsKey(content.Genre))
                {
                    genreCounts[content.Genre]++;
                }
                else
                {
                    genreCounts[content.Genre] = 1;
                }
            }

            // Return top 3 genres
            return genreCounts
                .OrderByDescending(kv => kv.Value)
                .Take(3)
                .Select(kv => kv.Key)
                .ToList();
        }
    }
}

