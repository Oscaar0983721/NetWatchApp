using NetWatchApp.Classes.Models;
using NetWatchApp.Data.EntityFramework;
using System;
using System.Linq;

namespace NetWatchApp.Data.SeedData
{
    public class DataSeeder
    {
        private readonly NetWatchDbContext _context;

        public DataSeeder(NetWatchDbContext context)
        {
            _context = context;
        }

        public void Seed()
        {
            // Create database if it doesn't exist
            _context.Database.EnsureCreated();

            // Check if there's already data
            if (_context.Users.Any() || _context.Contents.Any())
            {
                return; // Database already seeded
            }

            // Seed users
            SeedUsers();

            // Seed content
            SeedContent();

            // Seed ratings
            SeedRatings();

            // Seed viewing history
            SeedViewingHistory();

            // Save all changes
            _context.SaveChanges();
        }

        private void SeedUsers()
        {
            // Add admin user
            var adminUser = new User
            {
                IdentificationNumber = "ADMIN001",
                FirstName = "Admin",
                LastName = "User",
                Email = "admin@netwatch.com",
                Password = "admin123", 
                IsAdmin = true,
                RegistrationDate = DateTime.Now.AddMonths(-6)
            };

            // Add regular users
            var user1 = new User
            {
                IdentificationNumber = "USER001",
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Password = "password123", 
                IsAdmin = false,
                RegistrationDate = DateTime.Now.AddMonths(-3)
            };

            var user2 = new User
            {
                IdentificationNumber = "USER002",
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane.smith@example.com",
                Password = "password123", 
                IsAdmin = false,
                RegistrationDate = DateTime.Now.AddMonths(-2)
            };

            _context.Users.Add(adminUser);
            _context.Users.Add(user1);
            _context.Users.Add(user2);
        }

        private void SeedContent()
        {
            // Add movies
            var movie1 = new Content
            {
                Title = "The Matrix",
                Description = "A computer hacker learns from mysterious rebels about the true nature of his reality and his role in the war against its controllers.",
                ReleaseYear = 1999,
                Genre = "Sci-Fi",
                Type = "Movie",
                Platform = "Netflix",
                Duration = 136
            };

            var movie2 = new Content
            {
                Title = "Inception",
                Description = "A thief who steals corporate secrets through the use of dream-sharing technology is given the inverse task of planting an idea into the mind of a C.E.O.",
                ReleaseYear = 2010,
                Genre = "Sci-Fi",
                Type = "Movie",
                Platform = "HBO Max",
                Duration = 148
            };

            var movie3 = new Content
            {
                Title = "The Shawshank Redemption",
                Description = "Two imprisoned men bond over a number of years, finding solace and eventual redemption through acts of common decency.",
                ReleaseYear = 1994,
                Genre = "Drama",
                Type = "Movie",
                Platform = "Amazon Prime",
                Duration = 142
            };

            // Add series with episodes
            var series1 = new Content
            {
                Title = "Stranger Things",
                Description = "When a young boy disappears, his mother, a police chief, and his friends must confront terrifying supernatural forces in order to get him back.",
                ReleaseYear = 2016,
                Genre = "Sci-Fi",
                Type = "Series",
                Platform = "Netflix",
                Duration = 0
            };

            series1.Episodes.Add(new Episode
            {
                EpisodeNumber = 1,
                Title = "Chapter One: The Vanishing of Will Byers",
                Duration = 47
            });

            series1.Episodes.Add(new Episode
            {
                EpisodeNumber = 2,
                Title = "Chapter Two: The Weirdo on Maple Street",
                Duration = 55
            });

            series1.Episodes.Add(new Episode
            {
                EpisodeNumber = 3,
                Title = "Chapter Three: Holly, Jolly",
                Duration = 51
            });

            var series2 = new Content
            {
                Title = "Breaking Bad",
                Description = "A high school chemistry teacher diagnosed with inoperable lung cancer turns to manufacturing and selling methamphetamine in order to secure his family's future.",
                ReleaseYear = 2008,
                Genre = "Drama",
                Type = "Series",
                Platform = "Netflix",
                Duration = 0
            };

            series2.Episodes.Add(new Episode
            {
                EpisodeNumber = 1,
                Title = "Pilot",
                Duration = 58
            });

            series2.Episodes.Add(new Episode
            {
                EpisodeNumber = 2,
                Title = "Cat's in the Bag...",
                Duration = 48
            });

            series2.Episodes.Add(new Episode
            {
                EpisodeNumber = 3,
                Title = "...And the Bag's in the River",
                Duration = 48
            });

            _context.Contents.Add(movie1);
            _context.Contents.Add(movie2);
            _context.Contents.Add(movie3);
            _context.Contents.Add(series1);
            _context.Contents.Add(series2);
        }

        private void SeedRatings()
        {
            var users = _context.Users.ToList();
            var contents = _context.Contents.ToList();

            // Add some ratings
            _context.Ratings.Add(new Rating
            {
                UserId = users[1].Id, // John Doe
                ContentId = contents[0].Id, // The Matrix
                Score = 5,
                Comment = "One of the best sci-fi movies ever made!",
                RatingDate = DateTime.Now.AddDays(-30)
            });

            _context.Ratings.Add(new Rating
            {
                UserId = users[2].Id, // Jane Smith
                ContentId = contents[0].Id, // The Matrix
                Score = 4,
                Comment = "Great movie, amazing special effects.",
                RatingDate = DateTime.Now.AddDays(-25)
            });

            _context.Ratings.Add(new Rating
            {
                UserId = users[1].Id, // John Doe
                ContentId = contents[3].Id, // Stranger Things
                Score = 5,
                Comment = "Addictive series with great characters.",
                RatingDate = DateTime.Now.AddDays(-20)
            });

            _context.Ratings.Add(new Rating
            {
                UserId = users[2].Id, // Jane Smith
                ContentId = contents[4].Id, // Breaking Bad
                Score = 5,
                Comment = "Possibly the best TV show ever made.",
                RatingDate = DateTime.Now.AddDays(-15)
            });
        }

        private void SeedViewingHistory()
        {
            var users = _context.Users.ToList();
            var contents = _context.Contents.ToList();

            // Add viewing history for movies
            _context.ViewingHistories.Add(new ViewingHistory
            {
                UserId = users[1].Id, // John Doe
                ContentId = contents[0].Id, // The Matrix
                WatchDate = DateTime.Now.AddDays(-40),
                WatchedEpisodes = ""
            });

            _context.ViewingHistories.Add(new ViewingHistory
            {
                UserId = users[2].Id, // Jane Smith
                ContentId = contents[0].Id, // The Matrix
                WatchDate = DateTime.Now.AddDays(-35),
                WatchedEpisodes = ""
            });

            // Add viewing history for series with episodes
            _context.ViewingHistories.Add(new ViewingHistory
            {
                UserId = users[1].Id, // John Doe
                ContentId = contents[3].Id, // Stranger Things
                WatchDate = DateTime.Now.AddDays(-25),
                WatchedEpisodes = "1,2,3" // Watched episodes 1, 2, and 3
            });

            _context.ViewingHistories.Add(new ViewingHistory
            {
                UserId = users[2].Id, // Jane Smith
                ContentId = contents[4].Id, // Breaking Bad
                WatchDate = DateTime.Now.AddDays(-20),
                WatchedEpisodes = "1,2" // Watched episodes 1 and 2
            });
        }
    }
}

