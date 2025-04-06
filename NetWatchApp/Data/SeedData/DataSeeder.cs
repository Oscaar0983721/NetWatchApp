using NetWatchApp.Classes.Models;
using NetWatchApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace NetWatchApp.Data.SeedData
{
    public class DataSeeder
    {
        private readonly JsonDataService _jsonDataService;
        private readonly Random _random = new Random();

        public DataSeeder()
        {
            _jsonDataService = new JsonDataService();
        }

        public void Seed()
        {
            try
            {
                // Check if there's already data
                var users = _jsonDataService.GetAllUsers();
                var contents = _jsonDataService.GetAllContent();

                if (users.Any() && contents.Any())
                {
                    Console.WriteLine("Data already seeded. Skipping seed operation.");
                    return; // Data already seeded
                }

                Console.WriteLine("Starting data seeding...");

                // Seed users in smaller batches to avoid timeout
                SeedUsers(100);
                Console.WriteLine("Users seeded successfully.");

                // Seed movies in smaller batches
                SeedMovies(50);
                Console.WriteLine("Movies seeded successfully.");

                // Seed series in smaller batches
                SeedSeries(50);
                Console.WriteLine("Series seeded successfully.");

                // Seed ratings and viewing history
                SeedRatingsAndViewingHistory();
                Console.WriteLine("Ratings and viewing history seeded successfully.");

                Console.WriteLine("Data seeding completed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during data seeding: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
                throw; // Re-throw to make sure the error is visible
            }
        }

        private void SeedUsers(int count)
        {
            Console.WriteLine($"Seeding {count} users...");

            // Add admin user if not exists
            var adminUser = _jsonDataService.GetUserByEmail("admin@netwatch.com");
            if (adminUser == null)
            {
                adminUser = new User
                {
                    IdentificationNumber = "ADMIN001",
                    FirstName = "Admin",
                    LastName = "User",
                    Email = "admin@netwatch.com",
                    Password = "admin123", // In a real app, this would be hashed
                    IsAdmin = true,
                    RegistrationDate = DateTime.Now.AddMonths(-6)
                };
                _jsonDataService.AddUser(adminUser);
            }
            Console.WriteLine("Admin user added.");

            // Add regular users in batches
            var firstNames = new List<string> { "John", "Jane", "Michael", "Emily", "David", "Sarah", "Robert", "Lisa", "Daniel", "Emma",
                "William", "Olivia", "James", "Sophia", "Benjamin", "Isabella", "Jacob", "Mia", "Samuel", "Charlotte" };

            var lastNames = new List<string> { "Smith", "Johnson", "Brown", "Jones", "Garcia", "Miller", "Davis", "Rodriguez", "Martinez", "Wilson",
                "Anderson", "Taylor", "Thomas", "Moore", "Jackson", "Martin", "Lee", "Thompson", "White", "Harris" };

            // Add users in batches of 20 to avoid timeouts
            int batchSize = 20;
            for (int batch = 0; batch < count / batchSize; batch++)
            {
                for (int i = 0; i < batchSize; i++)
                {
                    int userIndex = batch * batchSize + i;
                    var firstName = firstNames[_random.Next(firstNames.Count)];
                    var lastName = lastNames[_random.Next(lastNames.Count)];
                    var userId = $"USER{(userIndex + 1).ToString("D3")}";

                    var user = new User
                    {
                        IdentificationNumber = userId,
                        FirstName = firstName,
                        LastName = lastName,
                        Email = $"{firstName.ToLower()}.{lastName.ToLower()}{_random.Next(1, 100)}@example.com",
                        Password = "password123", // In a real app, this would be hashed
                        IsAdmin = false,
                        RegistrationDate = DateTime.Now.AddDays(-_random.Next(1, 365))
                    };

                    _jsonDataService.AddUser(user);
                }

                Console.WriteLine($"Added batch {batch + 1} of users ({batchSize} users)");
            }
        }

        private void SeedMovies(int count)
        {
            Console.WriteLine($"Seeding {count} movies...");

            var genres = new List<string> {
                "Action", "Adventure", "Animation", "Comedy", "Crime", "Documentary",
                "Drama", "Fantasy", "Horror", "Mystery", "Romance", "Sci-Fi",
                "Thriller", "Western", "Biography", "Family", "History", "Musical"
            };

            var platforms = new List<string> {
                "Netflix", "Amazon Prime", "Disney+", "HBO Max", "Hulu", "Apple TV+",
                "Peacock", "Paramount+", "Crunchyroll", "YouTube Premium"
            };

            var movieTitles = new List<string> {
                "The Last Frontier", "Midnight Echo", "Eternal Sunshine", "Crimson Tide", "Shadow Hunter",
                "Golden Hour", "Starlight Express", "Ocean's Depth", "Silent Whisper", "Neon Nights",
                "Desert Storm", "Frozen Heart", "Infinite Galaxy", "Mountain Peak", "Jungle Adventure",
                "City Lights", "Broken Mirror", "Diamond Heist", "Electric Dreams", "Fire and Ice",
                "Ghost Protocol", "Hidden Figures", "Iron Will", "Jumping Jack", "Kingdom Falls",
                "Lost in Time", "Mystic River", "Night Watch", "Open Season", "Phantom Menace",
                "Quantum Field", "Red October", "Silver Lining", "Thunder Road", "Underwater World",
                "Valley of Death", "Winter Soldier", "X-Factor", "Yellow Submarine", "Zero Hour",
                "Alien Covenant", "Blue Sky", "Crystal Cave", "Dark Knight", "Echo Chamber",
                "Flying High", "Green Mile", "Hollow Earth", "Island Paradise", "Jurassic Age"
            };

            // Add movies in batches of 10
            int batchSize = 10;
            for (int batch = 0; batch < count / batchSize; batch++)
            {
                for (int i = 0; i < batchSize; i++)
                {
                    int movieIndex = batch * batchSize + i;
                    var title = movieTitles[movieIndex % movieTitles.Count] + (_random.Next(2) == 0 ? "" : " " + (_random.Next(1, 4)).ToString());
                    var genre = genres[_random.Next(genres.Count)];
                    var platform = platforms[_random.Next(platforms.Count)];
                    var releaseYear = _random.Next(1980, 2023);
                    var duration = _random.Next(75, 180);

                    var movie = new Content
                    {
                        Title = title,
                        Description = $"A {genre.ToLower()} movie about {GetRandomDescription(genre)}",
                        ReleaseYear = releaseYear,
                        Genre = genre,
                        Type = "Movie",
                        Platform = platform,
                        Duration = duration,
                        // Image URL from placeholder service with movie poster dimensions (typically 2:3 ratio)
                        ImagePath = $"https://picsum.photos/300/450?random={movieIndex + 1}"
                    };

                    _jsonDataService.AddContent(movie);
                }

                Console.WriteLine($"Added batch {batch + 1} of movies ({batchSize} movies)");
            }
        }

        private void SeedSeries(int count)
        {
            Console.WriteLine($"Seeding {count} series...");

            var genres = new List<string> {
                "Action", "Adventure", "Animation", "Comedy", "Crime", "Documentary",
                "Drama", "Fantasy", "Horror", "Mystery", "Romance", "Sci-Fi",
                "Thriller", "Western", "Biography", "Family", "History", "Musical"
            };

            var platforms = new List<string> {
                "Netflix", "Amazon Prime", "Disney+", "HBO Max", "Hulu", "Apple TV+",
                "Peacock", "Paramount+", "Crunchyroll", "YouTube Premium"
            };

            var seriesTitles = new List<string> {
                "Breaking Point", "Crown Jewels", "Dark Matter", "Epic Quest", "Fatal Attraction",
                "Gravity Falls", "Hidden Secrets", "Infinite Loop", "Justice League", "Kingdom Come",
                "Lost City", "Midnight Club", "New World Order", "Ocean's Deep", "Phantom Force",
                "Quantum Leap", "Rising Sun", "Supernatural", "Time Travelers", "Urban Legend",
                "Vampire Diaries", "Westworld", "X-Files", "Yellow Brick Road", "Zero Hour",
                "American Dream", "Beyond Reality", "Cosmic Journey", "Dangerous Minds", "Eternal Life",
                "Fargo", "Golden Age", "Heroes Rising", "Insidious", "Jungle Book",
                "Knight Rider", "Last Chance", "Mystery Zone", "Northern Exposure", "Outlander",
                "Prison Break", "Quantum Physics", "River Valley", "Survivor's Tale", "Twin Peaks"
            };

            // Add series in batches of 10
            int batchSize = 10;
            for (int batch = 0; batch < count / batchSize; batch++)
            {
                for (int i = 0; i < batchSize; i++)
                {
                    int seriesIndex = batch * batchSize + i;
                    var title = seriesTitles[seriesIndex % seriesTitles.Count] + (_random.Next(2) == 0 ? "" : " " + (_random.Next(1, 4)).ToString());
                    var genre = genres[_random.Next(genres.Count)];
                    var platform = platforms[_random.Next(platforms.Count)];
                    var releaseYear = _random.Next(1990, 2023);

                    var series = new Content
                    {
                        Title = title,
                        Description = $"A {genre.ToLower()} series about {GetRandomDescription(genre)}",
                        ReleaseYear = releaseYear,
                        Genre = genre,
                        Type = "Series",
                        Platform = platform,
                        Duration = 0,
                        // Image URL from placeholder service with TV show poster dimensions (typically 2:3 ratio)
                        ImagePath = $"https://picsum.photos/300/450?random={seriesIndex + 100}", // Use different random numbers than movies
                        Episodes = new List<Episode>()
                    };

                    // Add 3-8 episodes to each series
                    int episodeCount = _random.Next(3, 9);
                    for (int j = 1; j <= episodeCount; j++)
                    {
                        series.Episodes.Add(new Episode
                        {
                            EpisodeNumber = j,
                            Title = $"Episode {j}: {GetRandomEpisodeTitle()}",
                            Duration = _random.Next(20, 61) // 20-60 minutes
                        });
                    }

                    _jsonDataService.AddContent(series);
                }

                Console.WriteLine($"Added batch {batch + 1} of series ({batchSize} series)");
            }
        }

        private void SeedRatingsAndViewingHistory()
        {
            Console.WriteLine("Seeding ratings and viewing history...");

            var users = _jsonDataService.GetAllUsers().Where(u => !u.IsAdmin).Take(20).ToList(); // Limit to 20 users for performance
            var contents = _jsonDataService.GetAllContent().Take(30).ToList(); // Limit to 30 contents for performance

            Console.WriteLine($"Seeding ratings and viewing history for {users.Count} users and {contents.Count} contents...");

            // Each user rates and watches some content
            foreach (var user in users)
            {
                // Rate 1-5 random content items
                int ratingsCount = _random.Next(1, 6);
                var contentsToRate = GetRandomSubset(contents, ratingsCount);

                foreach (var content in contentsToRate)
                {
                    var rating = new Rating
                    {
                        UserId = user.Id,
                        ContentId = content.Id,
                        Score = _random.Next(1, 6), // 1-5 stars
                        Comment = GetRandomComment(content.Type),
                        RatingDate = DateTime.Now.AddDays(-_random.Next(1, 180))
                    };

                    _jsonDataService.AddRating(rating);
                }

                // Watch 1-8 random content items
                int watchCount = _random.Next(1, 9);
                var contentsToWatch = GetRandomSubset(contents, watchCount);

                foreach (var content in contentsToWatch)
                {
                    string watchedEpisodes = "";

                    // If it's a series, randomly select which episodes were watched
                    if (content.Type == "Series" && content.Episodes.Any())
                    {
                        var episodeNumbers = content.Episodes
                            .Select(e => e.EpisodeNumber)
                            .OrderBy(n => n)
                            .ToList();

                        // Watch 1 to all episodes
                        int episodesToWatch = _random.Next(1, episodeNumbers.Count + 1);
                        var watchedEpisodeNumbers = GetRandomSubset(episodeNumbers, episodesToWatch);

                        watchedEpisodes = string.Join(",", watchedEpisodeNumbers);
                    }

                    var viewingHistory = new ViewingHistory
                    {
                        UserId = user.Id,
                        ContentId = content.Id,
                        WatchDate = DateTime.Now.AddDays(-_random.Next(1, 365)),
                        WatchedEpisodes = watchedEpisodes
                    };

                    _jsonDataService.AddViewingHistory(viewingHistory);
                }

                Console.WriteLine($"Added ratings and viewing history for user {user.Id}");
            }
        }

        private T GetRandomElement<T>(List<T> list)
        {
            return list[_random.Next(list.Count)];
        }

        private List<T> GetRandomSubset<T>(List<T> list, int count)
        {
            return list.OrderBy(x => _random.Next()).Take(count).ToList();
        }

        private string GetRandomDescription(string genre)
        {
            var descriptions = new Dictionary<string, List<string>>
            {
                { "Action", new List<string> {
                    "a police officer fighting crime",
                    "an epic battle between good and evil",
                    "a special forces team on a dangerous mission",
                    "a martial arts master seeking revenge"
                }},
                { "Adventure", new List<string> {
                    "an explorer searching for a lost city",
                    "a journey through uncharted territories",
                    "a treasure hunt across multiple continents",
                    "a perilous expedition to an unknown land"
                }},
                { "Comedy", new List<string> {
                    "a family vacation gone wrong",
                    "office workers dealing with an eccentric boss",
                    "friends getting into hilarious situations",
                    "a fish out of water scenario in a new culture"
                }},
                { "Drama", new List<string> {
                    "a family dealing with loss",
                    "complicated relationships between colleagues",
                    "overcoming personal struggles",
                    "ethical dilemmas in modern society"
                }},
                { "Sci-Fi", new List<string> {
                    "an alien invasion of Earth",
                    "time travelers altering history",
                    "a future society with advanced technology",
                    "space exploration and discovery"
                }},
                { "Horror", new List<string> {
                    "a haunted house terrorizing its new residents",
                    "a serial killer stalking a small town",
                    "supernatural entities invading our world",
                    "surviving a zombie apocalypse"
                }}
            };

            // Default description for genres not in the dictionary
            if (!descriptions.ContainsKey(genre))
            {
                return "an engaging story with twists and turns";
            }

            return GetRandomElement(descriptions[genre]);
        }

        private string GetRandomEpisodeTitle()
        {
            var titles = new List<string> {
                "The Beginning", "New Horizons", "Dark Secrets", "Lost and Found",
                "Rising Tide", "Breaking Point", "Redemption", "Sacrifice",
                "Unexpected Journey", "Hidden Truth", "Last Stand", "Final Countdown",
                "Revelation", "Point of No Return", "Twist of Fate", "Aftermath",
                "Crossroads", "The Reunion", "Fresh Start", "End Game"
            };

            return GetRandomElement(titles);
        }

        private string GetRandomComment(string contentType)
        {
            var positiveComments = new List<string> {
                $"One of the best {contentType.ToLower()}s I've watched!",
                "Absolutely loved it!",
                "Great story and amazing characters.",
                "Highly recommend to everyone.",
                "Can't wait to watch more like this!"
            };

            var neutralComments = new List<string> {
                "It was okay, had its moments.",
                "Decent watch but nothing special.",
                "Some good parts, some not so good.",
                "Worth watching once.",
                "It was entertaining enough."
            };

            var negativeComments = new List<string> {
                "Not what I expected.",
                "Could have been better.",
                "Disappointing story.",
                "Wouldn't recommend.",
                "Had potential but fell short."
            };

            // 60% chance of positive, 30% neutral, 10% negative
            int rand = _random.Next(100);
            if (rand < 60)
                return GetRandomElement(positiveComments);
            else if (rand < 90)
                return GetRandomElement(neutralComments);
            else
                return GetRandomElement(negativeComments);
        }
    }
}

