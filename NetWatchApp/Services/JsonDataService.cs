using NetWatchApp.Classes.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Diagnostics;

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

        // Cached data collections
        private Dictionary<int, Content> _contentCache;
        private Dictionary<int, User> _userCache;
        private Dictionary<int, Rating> _ratingCache;
        private Dictionary<int, ViewingHistory> _viewingHistoryCache;

        // Auto-increment counters for IDs
        private int _nextContentId = 1;
        private int _nextUserId = 1;
        private int _nextRatingId = 1;
        private int _nextViewingHistoryId = 1;
        private int _nextEpisodeId = 1;

        public JsonDataService()
        {
            try
            {
                Debug.WriteLine("Initializing JsonDataService...");

                // Set up base directory in the application folder
                _baseDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "JsonData");
                _contentDirectory = Path.Combine(_baseDirectory, "Content");
                _userDirectory = Path.Combine(_baseDirectory, "Users");
                _ratingDirectory = Path.Combine(_baseDirectory, "Ratings");
                _viewingHistoryDirectory = Path.Combine(_baseDirectory, "ViewingHistory");

                // Create directories if they don't exist
                EnsureDirectoriesExist();

                // Configure JSON serializer options
                _jsonOptions = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    ReferenceHandler = ReferenceHandler.Preserve
                };

                // Initialize caches
                _contentCache = new Dictionary<int, Content>();
                _userCache = new Dictionary<int, User>();
                _ratingCache = new Dictionary<int, Rating>();
                _viewingHistoryCache = new Dictionary<int, ViewingHistory>();

                // Load all data from JSON files
                LoadAllData();

                Debug.WriteLine("JsonDataService initialized successfully");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error initializing JsonDataService: {ex.Message}");
                throw new Exception($"Error initializing JsonDataService: {ex.Message}", ex);
            }
        }

        private void EnsureDirectoriesExist()
        {
            try
            {
                Directory.CreateDirectory(_baseDirectory);
                Directory.CreateDirectory(_contentDirectory);
                Directory.CreateDirectory(_userDirectory);
                Directory.CreateDirectory(_ratingDirectory);
                Directory.CreateDirectory(_viewingHistoryDirectory);
                Debug.WriteLine("JSON directories created or verified");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error creating JSON directories: {ex.Message}");
                throw new Exception($"Error creating JSON directories: {ex.Message}", ex);
            }
        }

        private void LoadAllData()
        {
            try
            {
                Debug.WriteLine("Loading all data from JSON files...");

                // Load users
                LoadUsers();

                // Load content
                LoadContent();

                // Load ratings
                LoadRatings();

                // Load viewing histories
                LoadViewingHistories();

                Debug.WriteLine("All data loaded successfully");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading data: {ex.Message}");
                throw new Exception($"Error loading data: {ex.Message}", ex);
            }
        }

        #region User Methods

        private void LoadUsers()
        {
            try
            {
                _userCache.Clear();
                string indexPath = Path.Combine(_userDirectory, "user_index.json");

                if (File.Exists(indexPath))
                {
                    string jsonIndex = File.ReadAllText(indexPath);
                    var userIndex = JsonSerializer.Deserialize<Dictionary<int, UserIndexItem>>(jsonIndex, _jsonOptions);

                    if (userIndex != null)
                    {
                        foreach (var item in userIndex)
                        {
                            string filePath = Path.Combine(_userDirectory, $"user_{item.Key}.json");
                            if (File.Exists(filePath))
                            {
                                string jsonUser = File.ReadAllText(filePath);
                                var user = JsonSerializer.Deserialize<User>(jsonUser, _jsonOptions);

                                if (user != null)
                                {
                                    _userCache[user.Id] = user;
                                    _nextUserId = Math.Max(_nextUserId, user.Id + 1);
                                }
                            }
                        }
                    }
                }

                // If no users exist, create admin user
                if (_userCache.Count == 0)
                {
                    var adminUser = new User
                    {
                        Id = _nextUserId++,
                        IdentificationNumber = "ADMIN001",
                        FirstName = "Admin",
                        LastName = "User",
                        Email = "admin@netwatch.com",
                        Password = "admin123",
                        IsAdmin = true,
                        RegistrationDate = DateTime.Now.AddMonths(-6)
                    };

                    _userCache[adminUser.Id] = adminUser;
                    SaveUserToJson(adminUser);
                }

                Debug.WriteLine($"Loaded {_userCache.Count} users");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading users: {ex.Message}");
                throw new Exception($"Error loading users: {ex.Message}", ex);
            }
        }

        public List<User> GetAllUsers()
        {
            return _userCache.Values.ToList();
        }

        public User GetUserById(int id)
        {
            return _userCache.TryGetValue(id, out var user) ? user : null;
        }

        public User GetUserByEmail(string email)
        {
            return _userCache.Values.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
        }

        public User GetUserByIdentificationNumber(string identificationNumber)
        {
            return _userCache.Values.FirstOrDefault(u => u.IdentificationNumber.Equals(identificationNumber, StringComparison.OrdinalIgnoreCase));
        }

        public User AuthenticateUser(string email, string password)
        {
            return _userCache.Values.FirstOrDefault(u =>
                u.Email.Equals(email, StringComparison.OrdinalIgnoreCase) &&
                u.Password == password);
        }

        public User AddUser(User user)
        {
            // Check if email already exists
            if (_userCache.Values.Any(u => u.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase)))
            {
                throw new Exception("Email already exists.");
            }

            // Check if identification number already exists
            if (_userCache.Values.Any(u => u.IdentificationNumber.Equals(user.IdentificationNumber, StringComparison.OrdinalIgnoreCase)))
            {
                throw new Exception("Identification number already exists.");
            }

            // Assign new ID
            user.Id = _nextUserId++;

            // Initialize collections
            user.Ratings = new List<Rating>();
            user.ViewingHistories = new List<ViewingHistory>();

            // Add to cache
            _userCache[user.Id] = user;

            // Save to JSON
            SaveUserToJson(user);

            return user;
        }

        public User UpdateUser(User user)
        {
            if (!_userCache.ContainsKey(user.Id))
            {
                throw new Exception($"User with ID {user.Id} not found.");
            }

            // Check if email already exists for another user
            if (_userCache.Values.Any(u => u.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase) && u.Id != user.Id))
            {
                throw new Exception("Email already exists.");
            }

            // Check if identification number already exists for another user
            if (_userCache.Values.Any(u => u.IdentificationNumber.Equals(user.IdentificationNumber, StringComparison.OrdinalIgnoreCase) && u.Id != user.Id))
            {
                throw new Exception("Identification number already exists.");
            }

            // Preserve collections
            var existingUser = _userCache[user.Id];
            user.Ratings = existingUser.Ratings;
            user.ViewingHistories = existingUser.ViewingHistories;

            // Update cache
            _userCache[user.Id] = user;

            // Save to JSON
            SaveUserToJson(user);

            return user;
        }

        public void DeleteUser(int id)
        {
            if (_userCache.ContainsKey(id))
            {
                // Remove from cache
                _userCache.Remove(id);

                // Delete JSON file
                string filePath = Path.Combine(_userDirectory, $"user_{id}.json");
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                // Update index
                UpdateUserIndex();
            }
        }

        private void SaveUserToJson(User user)
        {
            try
            {
                Debug.WriteLine($"Saving user to JSON: {user.FirstName} {user.LastName}");

                // Create user file path
                string filePath = Path.Combine(_userDirectory, $"user_{user.Id}.json");

                // Serialize user to JSON
                string jsonUser = JsonSerializer.Serialize(user, _jsonOptions);

                // Write to file
                File.WriteAllText(filePath, jsonUser);

                // Update user index
                UpdateUserIndex();

                Debug.WriteLine($"User saved to JSON: {filePath}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error saving user to JSON: {ex.Message}");
                throw new Exception($"Error saving user to JSON: {ex.Message}", ex);
            }
        }

        private void UpdateUserIndex()
        {
            try
            {
                string indexPath = Path.Combine(_userDirectory, "user_index.json");
                var userIndex = new Dictionary<int, UserIndexItem>();

                foreach (var user in _userCache.Values)
                {
                    userIndex[user.Id] = new UserIndexItem
                    {
                        Id = user.Id,
                        FullName = $"{user.FirstName} {user.LastName}",
                        Email = user.Email,
                        IdentificationNumber = user.IdentificationNumber,
                        IsAdmin = user.IsAdmin,
                        LastUpdated = DateTime.Now
                    };
                }

                // Save updated index
                string updatedIndex = JsonSerializer.Serialize(userIndex, _jsonOptions);
                File.WriteAllText(indexPath, updatedIndex);

                Debug.WriteLine($"User index updated: {indexPath}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating user index: {ex.Message}");
                throw new Exception($"Error updating user index: {ex.Message}", ex);
            }
        }

        #endregion

        #region Content Methods

        private void LoadContent()
        {
            try
            {
                _contentCache.Clear();
                string indexPath = Path.Combine(_contentDirectory, "content_index.json");

                if (File.Exists(indexPath))
                {
                    string jsonIndex = File.ReadAllText(indexPath);
                    var contentIndex = JsonSerializer.Deserialize<Dictionary<int, ContentIndexItem>>(jsonIndex, _jsonOptions);

                    if (contentIndex != null)
                    {
                        foreach (var item in contentIndex)
                        {
                            string filePath = Path.Combine(_contentDirectory, $"content_{item.Key}.json");
                            if (File.Exists(filePath))
                            {
                                string jsonContent = File.ReadAllText(filePath);
                                var content = JsonSerializer.Deserialize<Content>(jsonContent, _jsonOptions);

                                if (content != null)
                                {
                                    _contentCache[content.Id] = content;
                                    _nextContentId = Math.Max(_nextContentId, content.Id + 1);

                                    // Update episode ID counter
                                    foreach (var episode in content.Episodes)
                                    {
                                        _nextEpisodeId = Math.Max(_nextEpisodeId, episode.Id + 1);
                                    }
                                }
                            }
                        }
                    }
                }

                Debug.WriteLine($"Loaded {_contentCache.Count} content items");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading content: {ex.Message}");
                throw new Exception($"Error loading content: {ex.Message}", ex);
            }
        }

        public List<Content> GetAllContent()
        {
            return _contentCache.Values.ToList();
        }

        public Content GetContentById(int id)
        {
            return _contentCache.TryGetValue(id, out var content) ? content : null;
        }

        public List<Content> SearchContent(string searchTerm = null, string genre = null, string type = null)
        {
            var query = _contentCache.Values.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                query = query.Where(c =>
                    c.Title.ToLower().Contains(searchTerm) ||
                    c.Description.ToLower().Contains(searchTerm));
            }

            if (!string.IsNullOrWhiteSpace(genre))
            {
                query = query.Where(c => c.Genre == genre);
            }

            if (!string.IsNullOrWhiteSpace(type))
            {
                query = query.Where(c => c.Type == type);
            }

            return query.ToList();
        }

        public Content AddContent(Content content)
        {
            // Assign new ID
            content.Id = _nextContentId++;

            // Initialize collections
            content.Episodes = content.Episodes ?? new List<Episode>();
            content.Ratings = new List<Rating>();
            content.ViewingHistories = new List<ViewingHistory>();

            // Assign IDs to episodes
            foreach (var episode in content.Episodes)
            {
                episode.Id = _nextEpisodeId++;
                episode.ContentId = content.Id;
                episode.Content = content;
            }

            // Add to cache
            _contentCache[content.Id] = content;

            // Save to JSON
            SaveContentToJson(content);

            return content;
        }

        public Content UpdateContent(Content content)
        {
            if (!_contentCache.ContainsKey(content.Id))
            {
                throw new Exception($"Content with ID {content.Id} not found.");
            }

            var existingContent = _contentCache[content.Id];

            // Preserve collections
            content.Ratings = existingContent.Ratings;
            content.ViewingHistories = existingContent.ViewingHistories;

            // Handle episodes
            if (content.Type == "Movie")
            {
                // Remove all episodes if content is now a movie
                content.Episodes.Clear();
            }
            else
            {
                // Update episodes
                var updatedEpisodes = new List<Episode>();

                foreach (var episode in content.Episodes)
                {
                    if (episode.Id == 0)
                    {
                        // New episode
                        episode.Id = _nextEpisodeId++;
                        episode.ContentId = content.Id;
                        episode.Content = content;
                    }
                    else
                    {
                        // Existing episode
                        var existingEpisode = existingContent.Episodes.FirstOrDefault(e => e.Id == episode.Id);
                        if (existingEpisode != null)
                        {
                            episode.ContentId = content.Id;
                            episode.Content = content;
                        }
                    }

                    updatedEpisodes.Add(episode);
                }

                content.Episodes = updatedEpisodes;
            }

            // Update cache
            _contentCache[content.Id] = content;

            // Save to JSON
            SaveContentToJson(content);

            return content;
        }

        public void DeleteContent(int id)
        {
            if (_contentCache.ContainsKey(id))
            {
                // Remove from cache
                _contentCache.Remove(id);

                // Delete JSON file
                string filePath = Path.Combine(_contentDirectory, $"content_{id}.json");
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                // Update index
                UpdateContentIndex();

                // Delete related ratings and viewing histories
                var ratingsToDelete = _ratingCache.Values.Where(r => r.ContentId == id).ToList();
                foreach (var rating in ratingsToDelete)
                {
                    DeleteRating(rating.Id);
                }

                var viewingHistoriesToDelete = _viewingHistoryCache.Values.Where(vh => vh.ContentId == id).ToList();
                foreach (var viewingHistory in viewingHistoriesToDelete)
                {
                    DeleteViewingHistory(viewingHistory.Id);
                }
            }
        }

        public List<string> GetAllGenres()
        {
            return _contentCache.Values
                .Select(c => c.Genre)
                .Distinct()
                .OrderBy(g => g)
                .ToList();
        }

        public List<string> GetAllPlatforms()
        {
            return _contentCache.Values
                .Select(c => c.Platform)
                .Distinct()
                .OrderBy(p => p)
                .ToList();
        }

        private void SaveContentToJson(Content content)
        {
            try
            {
                Debug.WriteLine($"Saving content to JSON: {content.Title}");

                // Create content file path
                string filePath = Path.Combine(_contentDirectory, $"content_{content.Id}.json");

                // Serialize content to JSON
                string jsonContent = JsonSerializer.Serialize(content, _jsonOptions);

                // Write to file
                File.WriteAllText(filePath, jsonContent);

                // Update content index
                UpdateContentIndex();

                Debug.WriteLine($"Content saved to JSON: {filePath}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error saving content to JSON: {ex.Message}");
                throw new Exception($"Error saving content to JSON: {ex.Message}", ex);
            }
        }

        // Método para compatibilidad con el código existente
        public async Task SaveContentToJsonAsync(Content content)
        {
            SaveContentToJson(content);
            await Task.CompletedTask;
        }

        private void UpdateContentIndex()
        {
            try
            {
                string indexPath = Path.Combine(_contentDirectory, "content_index.json");
                var contentIndex = new Dictionary<int, ContentIndexItem>();

                foreach (var content in _contentCache.Values)
                {
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
                }

                // Save updated index
                string updatedIndex = JsonSerializer.Serialize(contentIndex, _jsonOptions);
                File.WriteAllText(indexPath, updatedIndex);

                Debug.WriteLine($"Content index updated: {indexPath}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating content index: {ex.Message}");
                throw new Exception($"Error updating content index: {ex.Message}", ex);
            }
        }

        #endregion

        #region Rating Methods

        private void LoadRatings()
        {
            try
            {
                _ratingCache.Clear();
                string indexPath = Path.Combine(_ratingDirectory, "rating_index.json");

                if (File.Exists(indexPath))
                {
                    string jsonIndex = File.ReadAllText(indexPath);
                    var ratingIndex = JsonSerializer.Deserialize<Dictionary<int, RatingIndexItem>>(jsonIndex, _jsonOptions);

                    if (ratingIndex != null)
                    {
                        foreach (var item in ratingIndex)
                        {
                            string filePath = Path.Combine(_ratingDirectory, $"rating_{item.Key}.json");
                            if (File.Exists(filePath))
                            {
                                string jsonRating = File.ReadAllText(filePath);
                                var rating = JsonSerializer.Deserialize<Rating>(jsonRating, _jsonOptions);

                                if (rating != null)
                                {
                                    _ratingCache[rating.Id] = rating;
                                    _nextRatingId = Math.Max(_nextRatingId, rating.Id + 1);

                                    // Link to user and content
                                    if (_userCache.TryGetValue(rating.UserId, out var user))
                                    {
                                        rating.User = user;
                                        user.Ratings.Add(rating);
                                    }

                                    if (_contentCache.TryGetValue(rating.ContentId, out var content))
                                    {
                                        rating.Content = content;
                                        content.Ratings.Add(rating);
                                    }
                                }
                            }
                        }
                    }
                }

                Debug.WriteLine($"Loaded {_ratingCache.Count} ratings");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading ratings: {ex.Message}");
                throw new Exception($"Error loading ratings: {ex.Message}", ex);
            }
        }

        public List<Rating> GetAllRatings()
        {
            return _ratingCache.Values.ToList();
        }

        public Rating GetRatingById(int id)
        {
            return _ratingCache.TryGetValue(id, out var rating) ? rating : null;
        }

        public List<Rating> GetRatingsByUser(int userId)
        {
            return _ratingCache.Values.Where(r => r.UserId == userId).ToList();
        }

        public List<Rating> GetRatingsByContent(int contentId)
        {
            return _ratingCache.Values.Where(r => r.ContentId == contentId).ToList();
        }

        public Rating GetRatingByUserAndContent(int userId, int contentId)
        {
            return _ratingCache.Values.FirstOrDefault(r => r.UserId == userId && r.ContentId == contentId);
        }

        public Rating AddRating(Rating rating)
        {
            // Check if user already rated this content
            var existingRating = GetRatingByUserAndContent(rating.UserId, rating.ContentId);
            if (existingRating != null)
            {
                throw new Exception("User has already rated this content.");
            }

            // Assign new ID
            rating.Id = _nextRatingId++;

            // Link to user and content
            if (_userCache.TryGetValue(rating.UserId, out var user))
            {
                rating.User = user;
                user.Ratings.Add(rating);
            }

            if (_contentCache.TryGetValue(rating.ContentId, out var content))
            {
                rating.Content = content;
                content.Ratings.Add(rating);
            }

            // Add to cache
            _ratingCache[rating.Id] = rating;

            // Save to JSON
            SaveRatingToJson(rating);

            return rating;
        }

        public Rating UpdateRating(Rating rating)
        {
            if (!_ratingCache.ContainsKey(rating.Id))
            {
                throw new Exception($"Rating with ID {rating.Id} not found.");
            }

            var existingRating = _ratingCache[rating.Id];

            // Update properties
            existingRating.Score = rating.Score;
            existingRating.Comment = rating.Comment;
            existingRating.RatingDate = rating.RatingDate;

            // Save to JSON
            SaveRatingToJson(existingRating);

            return existingRating;
        }

        public void DeleteRating(int id)
        {
            if (_ratingCache.TryGetValue(id, out var rating))
            {
                // Remove from user and content
                if (_userCache.TryGetValue(rating.UserId, out var user))
                {
                    user.Ratings.Remove(rating);
                }

                if (_contentCache.TryGetValue(rating.ContentId, out var content))
                {
                    content.Ratings.Remove(rating);
                }

                // Remove from cache
                _ratingCache.Remove(id);

                // Delete JSON file
                string filePath = Path.Combine(_ratingDirectory, $"rating_{id}.json");
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                // Update index
                UpdateRatingIndex();
            }
        }

        public double GetAverageRatingForContent(int contentId)
        {
            var ratings = GetRatingsByContent(contentId);
            if (ratings.Count == 0)
                return 0;

            return Math.Round(ratings.Average(r => r.Score), 1);
        }

        private void SaveRatingToJson(Rating rating)
        {
            try
            {
                Debug.WriteLine($"Saving rating to JSON: ID {rating.Id}");

                // Create rating file path
                string filePath = Path.Combine(_ratingDirectory, $"rating_{rating.Id}.json");

                // Serialize rating to JSON
                string jsonRating = JsonSerializer.Serialize(rating, _jsonOptions);

                // Write to file
                File.WriteAllText(filePath, jsonRating);

                // Update rating index
                UpdateRatingIndex();

                Debug.WriteLine($"Rating saved to JSON: {filePath}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error saving rating to JSON: {ex.Message}");
                throw new Exception($"Error saving rating to JSON: {ex.Message}", ex);
            }
        }

        // Método para compatibilidad con el código existente
        public async Task SaveRatingToJsonAsync(Rating rating)
        {
            SaveRatingToJson(rating);
            await Task.CompletedTask;
        }

        private void UpdateRatingIndex()
        {
            try
            {
                string indexPath = Path.Combine(_ratingDirectory, "rating_index.json");
                var ratingIndex = new Dictionary<int, RatingIndexItem>();

                foreach (var rating in _ratingCache.Values)
                {
                    ratingIndex[rating.Id] = new RatingIndexItem
                    {
                        Id = rating.Id,
                        UserId = rating.UserId,
                        ContentId = rating.ContentId,
                        Score = rating.Score,
                        RatingDate = rating.RatingDate,
                        LastUpdated = DateTime.Now
                    };
                }

                // Save updated index
                string updatedIndex = JsonSerializer.Serialize(ratingIndex, _jsonOptions);
                File.WriteAllText(indexPath, updatedIndex);

                Debug.WriteLine($"Rating index updated: {indexPath}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating rating index: {ex.Message}");
                throw new Exception($"Error updating rating index: {ex.Message}", ex);
            }
        }

        #endregion

        #region ViewingHistory Methods

        private void LoadViewingHistories()
        {
            try
            {
                _viewingHistoryCache.Clear();
                string indexPath = Path.Combine(_viewingHistoryDirectory, "viewing_history_index.json");

                if (File.Exists(indexPath))
                {
                    string jsonIndex = File.ReadAllText(indexPath);
                    var viewingHistoryIndex = JsonSerializer.Deserialize<Dictionary<int, ViewingHistoryIndexItem>>(jsonIndex, _jsonOptions);

                    if (viewingHistoryIndex != null)
                    {
                        foreach (var item in viewingHistoryIndex)
                        {
                            string filePath = Path.Combine(_viewingHistoryDirectory, $"viewing_history_{item.Key}.json");
                            if (File.Exists(filePath))
                            {
                                string jsonViewingHistory = File.ReadAllText(filePath);
                                var viewingHistory = JsonSerializer.Deserialize<ViewingHistory>(jsonViewingHistory, _jsonOptions);

                                if (viewingHistory != null)
                                {
                                    _viewingHistoryCache[viewingHistory.Id] = viewingHistory;
                                    _nextViewingHistoryId = Math.Max(_nextViewingHistoryId, viewingHistory.Id + 1);

                                    // Link to user and content
                                    if (_userCache.TryGetValue(viewingHistory.UserId, out var user))
                                    {
                                        viewingHistory.User = user;
                                        user.ViewingHistories.Add(viewingHistory);
                                    }

                                    if (_contentCache.TryGetValue(viewingHistory.ContentId, out var content))
                                    {
                                        viewingHistory.Content = content;
                                        content.ViewingHistories.Add(viewingHistory);
                                    }
                                }
                            }
                        }
                    }
                }

                Debug.WriteLine($"Loaded {_viewingHistoryCache.Count} viewing histories");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading viewing histories: {ex.Message}");
                throw new Exception($"Error loading viewing histories: {ex.Message}", ex);
            }
        }

        public List<ViewingHistory> GetAllViewingHistories()
        {
            return _viewingHistoryCache.Values.ToList();
        }

        public ViewingHistory GetViewingHistoryById(int id)
        {
            return _viewingHistoryCache.TryGetValue(id, out var viewingHistory) ? viewingHistory : null;
        }

        public List<ViewingHistory> GetViewingHistoriesByUser(int userId)
        {
            return _viewingHistoryCache.Values.Where(vh => vh.UserId == userId).ToList();
        }

        public List<ViewingHistory> GetViewingHistoriesByContent(int contentId)
        {
            return _viewingHistoryCache.Values.Where(vh => vh.ContentId == contentId).ToList();
        }

        public ViewingHistory GetViewingHistoryByUserAndContent(int userId, int contentId)
        {
            return _viewingHistoryCache.Values.FirstOrDefault(vh => vh.UserId == userId && vh.ContentId == contentId);
        }

        public ViewingHistory AddViewingHistory(ViewingHistory viewingHistory)
        {
            // Assign new ID
            viewingHistory.Id = _nextViewingHistoryId++;

            // Link to user and content
            if (_userCache.TryGetValue(viewingHistory.UserId, out var user))
            {
                viewingHistory.User = user;
                user.ViewingHistories.Add(viewingHistory);
            }

            if (_contentCache.TryGetValue(viewingHistory.ContentId, out var content))
            {
                viewingHistory.Content = content;
                content.ViewingHistories.Add(viewingHistory);
            }

            // Add to cache
            _viewingHistoryCache[viewingHistory.Id] = viewingHistory;

            // Save to JSON
            SaveViewingHistoryToJson(viewingHistory);

            return viewingHistory;
        }

        public ViewingHistory UpdateViewingHistory(ViewingHistory viewingHistory)
        {
            if (!_viewingHistoryCache.ContainsKey(viewingHistory.Id))
            {
                throw new Exception($"Viewing history with ID {viewingHistory.Id} not found.");
            }

            var existingViewingHistory = _viewingHistoryCache[viewingHistory.Id];

            // Update properties
            existingViewingHistory.WatchDate = viewingHistory.WatchDate;
            existingViewingHistory.WatchedEpisodes = viewingHistory.WatchedEpisodes;

            // Save to JSON
            SaveViewingHistoryToJson(existingViewingHistory);

            return existingViewingHistory;
        }

        public void DeleteViewingHistory(int id)
        {
            if (_viewingHistoryCache.TryGetValue(id, out var viewingHistory))
            {
                // Remove from user and content
                if (_userCache.TryGetValue(viewingHistory.UserId, out var user))
                {
                    user.ViewingHistories.Remove(viewingHistory);
                }

                if (_contentCache.TryGetValue(viewingHistory.ContentId, out var content))
                {
                    content.ViewingHistories.Remove(viewingHistory);
                }

                // Remove from cache
                _viewingHistoryCache.Remove(id);

                // Delete JSON file
                string filePath = Path.Combine(_viewingHistoryDirectory, $"viewing_history_{id}.json");
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                // Update index
                UpdateViewingHistoryIndex();
            }
        }

        public List<Content> GetRecentlyWatchedContent(int userId, int count = 5)
        {
            return _viewingHistoryCache.Values
                .Where(vh => vh.UserId == userId)
                .OrderByDescending(vh => vh.WatchDate)
                .Take(count)
                .Select(vh => vh.Content)
                .ToList();
        }

        private void SaveViewingHistoryToJson(ViewingHistory viewingHistory)
        {
            try
            {
                Debug.WriteLine($"Saving viewing history to JSON: ID {viewingHistory.Id}");

                // Create viewing history file path
                string filePath = Path.Combine(_viewingHistoryDirectory, $"viewing_history_{viewingHistory.Id}.json");

                // Serialize viewing history to JSON
                string jsonViewingHistory = JsonSerializer.Serialize(viewingHistory, _jsonOptions);

                // Write to file
                File.WriteAllText(filePath, jsonViewingHistory);

                // Update viewing history index
                UpdateViewingHistoryIndex();

                Debug.WriteLine($"Viewing history saved to JSON: {filePath}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error saving viewing history to JSON: {ex.Message}");
                throw new Exception($"Error saving viewing history to JSON: {ex.Message}", ex);
            }
        }

        // Método para compatibilidad con el código existente
        public async Task SaveViewingHistoryToJsonAsync(ViewingHistory viewingHistory)
        {
            SaveViewingHistoryToJson(viewingHistory);
            await Task.CompletedTask;
        }

        private void UpdateViewingHistoryIndex()
        {
            try
            {
                string indexPath = Path.Combine(_viewingHistoryDirectory, "viewing_history_index.json");
                var viewingHistoryIndex = new Dictionary<int, ViewingHistoryIndexItem>();

                foreach (var viewingHistory in _viewingHistoryCache.Values)
                {
                    viewingHistoryIndex[viewingHistory.Id] = new ViewingHistoryIndexItem
                    {
                        Id = viewingHistory.Id,
                        UserId = viewingHistory.UserId,
                        ContentId = viewingHistory.ContentId,
                        WatchDate = viewingHistory.WatchDate,
                        HasWatchedEpisodes = !string.IsNullOrEmpty(viewingHistory.WatchedEpisodes),
                        LastUpdated = DateTime.Now
                    };
                }

                // Save updated index
                string updatedIndex = JsonSerializer.Serialize(viewingHistoryIndex, _jsonOptions);
                File.WriteAllText(indexPath, updatedIndex);

                Debug.WriteLine($"Viewing history index updated: {indexPath}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating viewing history index: {ex.Message}");
                throw new Exception($"Error updating viewing history index: {ex.Message}", ex);
            }
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

