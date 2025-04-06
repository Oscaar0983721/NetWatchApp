using NetWatchApp.Classes.Models;
using NetWatchApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetWatchApp.Classes.Repositories
{
    public class RatingRepository
    {
        private readonly JsonDataService _jsonDataService;

        public RatingRepository()
        {
            _jsonDataService = new JsonDataService();
        }

        public List<Rating> GetAll()
        {
            return _jsonDataService.GetAllRatings();
        }

        public Rating GetById(int id)
        {
            return _jsonDataService.GetRatingById(id);
        }

        public List<Rating> GetByUser(int userId)
        {
            return _jsonDataService.GetRatingsByUser(userId);
        }

        public List<Rating> GetByContent(int contentId)
        {
            return _jsonDataService.GetRatingsByContent(contentId);
        }

        public Rating GetByUserAndContent(int userId, int contentId)
        {
            return _jsonDataService.GetRatingByUserAndContent(userId, contentId);
        }

        public void Add(Rating rating)
        {
            _jsonDataService.AddRating(rating);
        }

        public void Update(Rating rating)
        {
            _jsonDataService.UpdateRating(rating);
        }

        public void Delete(int id)
        {
            _jsonDataService.DeleteRating(id);
        }

        public double GetAverageRatingForContent(int contentId)
        {
            return _jsonDataService.GetAverageRatingForContent(contentId);
        }
    }
}

