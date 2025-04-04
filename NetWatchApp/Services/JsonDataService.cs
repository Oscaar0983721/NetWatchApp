using NetWatchApp.Classes.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace NetWatchApp.Services
{
    public class JsonDataService
    {
        private readonly string _baseDirectory;
        private readonly string _contentDirectory;
        private readonly string _userDirectory;
        private readonly string _ratingDirectory;
        private readonly string _viewingHistoryDirectory;
        private readonly JsonSerializerOptions _jsonOptions;

        public JsonDataService()
        {
            // Set up base directory in the application folder
            _baseDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "JsonData");
            _contentDirectory = Path.Combine(_baseDirectory, "Content");
            _userDirectory = Path.Combine(_baseDirectory, "Users");
            _ratingDirectory = Path.Combine(_baseDirectory, "Ratings");
            _viewingHistoryDirectory = Path.Combine(_baseDirectory, "ViewingHistory");

            // Create directories if they don't exist
            Directory.CreateDirectory(_baseDirectory);
            Directory.CreateDirectory(_contentDirectory);
            Directory.CreateDirectory(_userDirectory);
            Directory.CreateDirectory(_ratingDirectory);
            Directory.CreateDirectory(_viewingHistoryDirectory);

            // Configure JSON serializer options
            _jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve
            };
        }

        #region Content Methods

        public async Task SaveContentToJsonAsync(Content content)
        {
            try
            {
                // Create content file path
                string filePath = Path.Combine(_contentDirectory, $"content_{content.Id}.json");

                // Serialize content to JSON
                string jsonContent = JsonSerializer.Serialize(content, _jsonOptions);

                // Write to file asynchronously
                await File.WriteAllTextAsync(filePath, jsonContent);

                // Update content index
                await UpdateContentIndexAsync(content);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving content to JSON: {ex.Message}");
                // In a real application, you might want to log this error
            }
        }

        private async Task UpdateContentIndexAsync(Content content)
        {
            try
            {
                string indexPath = Path.Combine(_contentDirectory, "content_index.json");
                Dictionary<int, ContentIndexItem> contentIndex;

                // Load existing index or create new one
                if (File.Exists(indexPath))
                {
                    string jsonIndex = await File.ReadAllTextAsync(indexPath);
                    contentIndex = JsonSerializer.Deserialize<Dictionary<int, ContentIndexItem>>(jsonIndex, _jsonOptions)
                        ?? new Dictionary<int, ContentIndexItem>();
                }
                else
                {
                    contentIndex = new Dictionary<int, ContentIndexItem>();
                }

                // Update or add index entry
                contentIndex[content.Id] = new ContentIndexItem
                {
                    Id = content.Id,
                    Title = content.Title,
                    Type = content.Type,
                    Genre = content.Genre,
                    ReleaseYear = content.ReleaseYear,
                    Platform = content.Platform,
                    LastUpdated = DateTime.Now
                };

                // Save updated index
                string updatedIndex = JsonSerializer.Serialize(contentIndex, _jsonOptions);
                await File.WriteAllTextAsync(indexPath, updatedIndex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating content index: {ex.Message}");
            }
        }

        #endregion

        #region User Methods

        public async Task SaveUserToJsonAsync(User user)
        {
            try
            {
                // Create user file path
                string filePath = Path.Combine(_userDirectory, $"user_{user.Id}.json");

                // Create a copy of the user without the password for security
                var userForJson = new
                {
                    user.Id,
                    user.FirstName,
                    user.LastName,
                    user.Email,
                    user.IdentificationNumber,
                    user.IsAdmin,
                    user.RegistrationDate,
                    PasswordHash = "********" // Don't store actual password in JSON
                };

                // Serialize user to JSON
                string jsonUser = JsonSerializer.Serialize(userForJson, _jsonOptions);

                // Write to file asynchronously
                await File.WriteAllTextAsync(filePath, jsonUser);

                // Update user index
                await UpdateUserIndexAsync(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving user to JSON: {ex.Message}");
            }
        }

        private async Task UpdateUserIndexAsync(User user)
        {
            try
            {
                string indexPath = Path.Combine(_userDirectory, "user_index.json");
                Dictionary<int, UserIndexItem> userIndex;

                // Load existing index or create new one
                if (File.Exists(indexPath))
                {
                    string jsonIndex = await File.ReadAllTextAsync(indexPath);
                    userIndex = JsonSerializer.Deserialize<Dictionary<int, UserIndexItem>>(jsonIndex, _jsonOptions)
                        ?? new Dictionary<int, UserIndexItem>();
                }
                else
                {
                    userIndex = new Dictionary<int, UserIndexItem>();
                }

                // Update or add index entry
                userIndex[user.Id] = new UserIndexItem
                {
                    Id = user.Id,
                    FullName = $"{user.FirstName} {user.LastName}",
                    Email = user.Email,
                    IdentificationNumber = user.IdentificationNumber,
                    IsAdmin = user.IsAdmin,
                    LastUpdated = DateTime.Now
                };

                // Save updated index
                string updatedIndex = JsonSerializer.Serialize(userIndex, _jsonOptions);
                await File.WriteAllTextAsync(indexPath, updatedIndex);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating user index: {ex.Message}");
            }
        }

        #endregion

        #region Rating Methods

        public async Task SaveRatingToJsonAsync(Rating rating)
        {
            try
            {
                // Create rating file path
                string filePath = Path.Combine(_ratingDirectory, $"rating_{rating.Id}.json");

                // Serialize rating to JSON
                string jsonRating = JsonSerializer.Serialize(rating, _jsonOptions);

                // Write to file asynchronously
                await File.WriteAllTextAsync(filePath, jsonRating);

                // Update rating indices
                await UpdateRatingIndicesAsync(rating);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving rating to JSON: {ex.Message}");
            }
        }

        private async Task UpdateRatingIndicesAsync(Rating rating)
        {
            try
            {
                // Update main rating index
                await UpdateRatingMainIndexAsync(rating);

                // Update content-specific rating index
                await UpdateContentRatingIndexAsync(rating);

                // Update user-specific rating index
                await UpdateUserRatingIndexAsync(rating);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating rating indices: {ex.Message}");
            }
        }

        private async Task UpdateRatingMainIndexAsync(Rating rating)
        {
            string indexPath = Path.Combine(_ratingDirectory, "rating_index.json");
            Dictionary<int, RatingIndexItem> ratingIndex;

            // Load existing index or create new one
            if (File.Exists(indexPath))
            {
                string jsonIndex = await File.ReadAllTextAsync(indexPath);
                ratingIndex = JsonSerializer.Deserialize<Dictionary<int, RatingIndexItem>>(jsonIndex, _jsonOptions)
                    ?? new Dictionary<int, RatingIndexItem>();
            }
            else
            {
                ratingIndex = new Dictionary<int, RatingIndexItem>();
            }

            // Update or add index entry
            ratingIndex[rating.Id] = new RatingIndexItem
            {
                Id = rating.Id,
                UserId = rating.UserId,
                ContentId = rating.ContentId,
                Score = rating.Score,
                RatingDate = rating.RatingDate,
                LastUpdated = DateTime.Now
            };

            // Save updated index
            string updatedIndex = JsonSerializer.Serialize(ratingIndex, _jsonOptions);
            await File.WriteAllTextAsync(indexPath, updatedIndex);
        }

        private async Task UpdateContentRatingIndexAsync(Rating rating)
        {
            string indexPath = Path.Combine(_ratingDirectory, $"content_ratings_{rating.ContentId}.json");
            Dictionary<int, RatingIndexItem> contentRatingIndex;

            // Load existing index or create new one
            if (File.Exists(indexPath))
            {
                string jsonIndex = await File.ReadAllTextAsync(indexPath);
                contentRatingIndex = JsonSerializer.Deserialize<Dictionary<int, RatingIndexItem>>(jsonIndex, _jsonOptions)
                    ?? new Dictionary<int, RatingIndexItem>();
            }
            else
            {
                contentRatingIndex = new Dictionary<int, RatingIndexItem>();
            }

            // Update or add index entry
            contentRatingIndex[rating.Id] = new RatingIndexItem
            {
                Id = rating.Id,
                UserId = rating.UserId,
                ContentId = rating.ContentId,
                Score = rating.Score,
                RatingDate = rating.RatingDate,
                LastUpdated = DateTime.Now
            };

            // Save updated index
            string updatedIndex = JsonSerializer.Serialize(contentRatingIndex, _jsonOptions);
            await File.WriteAllTextAsync(indexPath, updatedIndex);
        }

        private async Task UpdateUserRatingIndexAsync(Rating rating)
        {
            string indexPath = Path.Combine(_ratingDirectory, $"user_ratings_{rating.UserId}.json");
            Dictionary<int, RatingIndexItem> userRatingIndex;

            // Load existing index or create new one
            if (File.Exists(indexPath))
            {
                string jsonIndex = await File.ReadAllTextAsync(indexPath);
                userRatingIndex = JsonSerializer.Deserialize<Dictionary<int, RatingIndexItem>>(jsonIndex, _jsonOptions)
                    ?? new Dictionary<int, RatingIndexItem>();
            }
            else
            {
                userRatingIndex = new Dictionary<int, RatingIndexItem>();
            }

            // Update or add index entry
            userRatingIndex[rating.Id] = new RatingIndexItem
            {
                Id = rating.Id,
                UserId = rating.UserId,
                ContentId = rating.ContentId,
                Score = rating.Score,
                RatingDate = rating.RatingDate,
                LastUpdated = DateTime.Now
            };

            // Save updated index
            string updatedIndex = JsonSerializer.Serialize(userRatingIndex, _jsonOptions);
            await File.WriteAllTextAsync(indexPath, updatedIndex);
        }

        #endregion

        #region ViewingHistory Methods

        public async Task SaveViewingHistoryToJsonAsync(ViewingHistory viewingHistory)
        {
            try
            {
                // Create viewing history file path
                string filePath = Path.Combine(_viewingHistoryDirectory, $"viewing_history_{viewingHistory.Id}.json");

                // Serialize viewing history to JSON
                string jsonViewingHistory = JsonSerializer.Serialize(viewingHistory, _jsonOptions);

                // Write to file asynchronously
                await File.WriteAllTextAsync(filePath, jsonViewingHistory);

                // Update viewing history indices
                await UpdateViewingHistoryIndicesAsync(viewingHistory);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving viewing history to JSON: {ex.Message}");
            }
        }

        private async Task UpdateViewingHistoryIndicesAsync(ViewingHistory viewingHistory)
        {
            try
            {
                // Update main viewing history index
                await UpdateViewingHistoryMainIndexAsync(viewingHistory);

                // Update content-specific viewing history index
                await UpdateContentViewingHistoryIndexAsync(viewingHistory);

                // Update user-specific viewing history index
                await UpdateUserViewingHistoryIndexAsync(viewingHistory);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating viewing history indices: {ex.Message}");
            }
        }

        private async Task UpdateViewingHistoryMainIndexAsync(ViewingHistory viewingHistory)
        {
            string indexPath = Path.Combine(_viewingHistoryDirectory, "viewing_history_index.json");
            Dictionary<int, ViewingHistoryIndexItem> viewingHistoryIndex;

            // Load existing index or create new one
            if (File.Exists(indexPath))
            {
                string jsonIndex = await File.ReadAllTextAsync(indexPath);
                viewingHistoryIndex = JsonSerializer.Deserialize<Dictionary<int, ViewingHistoryIndexItem>>(jsonIndex, _jsonOptions)
                    ?? new Dictionary<int, ViewingHistoryIndexItem>();
            }
            else
            {
                viewingHistoryIndex = new Dictionary<int, ViewingHistoryIndexItem>();
            }

            // Update or add index entry
            viewingHistoryIndex[viewingHistory.Id] = new ViewingHistoryIndexItem
            {
                Id = viewingHistory.Id,
                UserId = viewingHistory.UserId,
                ContentId = viewingHistory.ContentId,
                WatchDate = viewingHistory.WatchDate,
                HasWatchedEpisodes = !string.IsNullOrEmpty(viewingHistory.WatchedEpisodes),
                LastUpdated = DateTime.Now
            };

            // Save updated index
            string updatedIndex = JsonSerializer.Serialize(viewingHistoryIndex, _jsonOptions);
            await File.WriteAllTextAsync(indexPath, updatedIndex);
        }

        private async Task UpdateContentViewingHistoryIndexAsync(ViewingHistory viewingHistory)
        {
            string indexPath = Path.Combine(_viewingHistoryDirectory, $"content_viewing_history_{viewingHistory.ContentId}.json");
            Dictionary<int, ViewingHistoryIndexItem> contentViewingHistoryIndex;

            // Load existing index or create new one
            if (File.Exists(indexPath))
            {
                string jsonIndex = await File.ReadAllTextAsync(indexPath);
                contentViewingHistoryIndex = JsonSerializer.Deserialize<Dictionary<int, ViewingHistoryIndexItem>>(jsonIndex, _jsonOptions)
                    ?? new Dictionary<int, ViewingHistoryIndexItem>();
            }
            else
            {
                contentViewingHistoryIndex = new Dictionary<int, ViewingHistoryIndexItem>();
            }

            // Update or add index entry
            contentViewingHistoryIndex[viewingHistory.Id] = new ViewingHistoryIndexItem
            {
                Id = viewingHistory.Id,
                UserId = viewingHistory.UserId,
                ContentId = viewingHistory.ContentId,
                WatchDate = viewingHistory.WatchDate,
                HasWatchedEpisodes = !string.IsNullOrEmpty(viewingHistory.WatchedEpisodes),
                LastUpdated = DateTime.Now
            };

            // Save updated index
            string updatedIndex = JsonSerializer.Serialize(contentViewingHistoryIndex, _jsonOptions);
            await File.WriteAllTextAsync(indexPath, updatedIndex);
        }

        private async Task UpdateUserViewingHistoryIndexAsync(ViewingHistory viewingHistory)
        {
            string indexPath = Path.Combine(_viewingHistoryDirectory, $"user_viewing_history_{viewingHistory.UserId}.json");
            Dictionary<int, ViewingHistoryIndexItem> userViewingHistoryIndex;

            // Load existing index or create new one
            if (File.Exists(indexPath))
            {
                string jsonIndex = await File.ReadAllTextAsync(indexPath);
                userViewingHistoryIndex = JsonSerializer.Deserialize<Dictionary<int, ViewingHistoryIndexItem>>(jsonIndex, _jsonOptions)
                    ?? new Dictionary<int, ViewingHistoryIndexItem>();
            }
            else
            {
                userViewingHistoryIndex = new Dictionary<int, ViewingHistoryIndexItem>();
            }

            // Update or add index entry
            userViewingHistoryIndex[viewingHistory.Id] = new ViewingHistoryIndexItem
            {
                Id = viewingHistory.Id,
                UserId = viewingHistory.UserId,
                ContentId = viewingHistory.ContentId,
                WatchDate = viewingHistory.WatchDate,
                HasWatchedEpisodes = !string.IsNullOrEmpty(viewingHistory.WatchedEpisodes),
                LastUpdated = DateTime.Now
            };

            // Save updated index
            string updatedIndex = JsonSerializer.Serialize(userViewingHistoryIndex, _jsonOptions);
            await File.WriteAllTextAsync(indexPath, updatedIndex);
        }

        #endregion

        #region Index Item Classes

        private class ContentIndexItem
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Type { get; set; }
            public string Genre { get; set; }
            public int ReleaseYear { get; set; }
            public string Platform { get; set; }
            public DateTime LastUpdated { get; set; }
        }

        private class UserIndexItem
        {
            public int Id { get; set; }
            public string FullName { get; set; }
            public string Email { get; set; }
            public string IdentificationNumber { get; set; }
            public bool IsAdmin { get; set; }
            public DateTime LastUpdated { get; set; }
        }

        private class RatingIndexItem
        {
            public int Id { get; set; }
            public int UserId { get; set; }
            public int ContentId { get; set; }
            public int Score { get; set; }
            public DateTime RatingDate { get; set; }
            public DateTime LastUpdated { get; set; }
        }

        private class ViewingHistoryIndexItem
        {
            public int Id { get; set; }
            public int UserId { get; set; }
            public int ContentId { get; set; }
            public DateTime WatchDate { get; set; }
            public bool HasWatchedEpisodes { get; set; }
            public DateTime LastUpdated { get; set; }
        }

        #endregion
    }
}

