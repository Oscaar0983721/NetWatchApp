using Microsoft.EntityFrameworkCore;
using NetWatchApp.Classes.Models;
using NetWatchApp.Data.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace NetWatchApp.Data.SeedData
{
    public static class DataSeeder
    {
        public static void SeedData(NetWatchDbContext context)
        {
            // Only seed if the database is empty
            if (context.Users.Any() || context.Contents.Any())
                return;

            // Seed users
            SeedUsers(context);

            // Seed content
            SeedContent(context);

            // Seed viewing history
            SeedViewingHistory(context);

            // Seed ratings
            SeedRatings(context);
        }

        private static void SeedUsers(NetWatchDbContext context)
        {
            var users = new List<User>
            {
                new User
                {
                    IdentificationNumber = "1234567890",
                    Name = "Admin User",
                    Email = "admin@netwatch.com",
                    PasswordHash = HashPassword("admin"),
                    IsAdmin = true,
                    RegistrationDate = DateTime.Now.AddMonths(-6)
                },
                new User
                {
                    IdentificationNumber = "0987654321",
                    Name = "Regular User",
                    Email = "user@netwatch.com",
                    PasswordHash = HashPassword("user"),
                    IsAdmin = false,
                    RegistrationDate = DateTime.Now.AddMonths(-3)
                }
            };

            // Add 50 more random users
            for (int i = 1; i <= 50; i++)
            {
                users.Add(new User
                {
                    IdentificationNumber = $"USER{i:D6}",
                    Name = $"Test User {i}",
                    Email = $"user{i}@netwatch.com",
                    PasswordHash = HashPassword("password"),
                    IsAdmin = false,
                    RegistrationDate = DateTime.Now.AddDays(-Random.Shared.Next(1, 365))
                });
            }

            context.Users.AddRange(users);
            context.SaveChanges();
        }

        private static void SeedContent(NetWatchDbContext context)
        {
            // Seed movies
            var movies = new List<Content>();
            string[] movieGenres = { "Action", "Comedy", "Drama", "Sci-Fi", "Horror", "Romance", "Thriller", "Fantasy", "Animation", "Documentary" };
            string[] platforms = { "Netflix", "Amazon Prime", "Disney+", "HBO Max", "Hulu", "Apple TV+", "Paramount+" };

            for (int i = 1; i <= 50; i++)
            {
                var movie = new Content
                {
                    Title = $"Movie {i}",
                    Type = ContentType.Movie,
                    Description = $"This is a description for Movie {i}. It's a great movie that you should definitely watch.",
                    Genre = movieGenres[Random.Shared.Next(movieGenres.Length)],
                    ReleaseYear = Random.Shared.Next(2000, 2023),
                    DurationMinutes = Random.Shared.Next(90, 180),
                    Platform = platforms[Random.Shared.Next(platforms.Length)],
                    ImagePath = $"/Images/movie{i}.jpg"
                };

                movies.Add(movie);
            }

            context.Contents.AddRange(movies);
            context.SaveChanges();

            // Seed series
            var series = new List<Content>();

            for (int i = 1; i <= 30; i++)
            {
                var seriesContent = new Content
                {
                    Title = $"Series {i}",
                    Type = ContentType.Series,
                    Description = $"This is a description for Series {i}. It's an exciting series with multiple seasons.",
                    Genre = movieGenres[Random.Shared.Next(movieGenres.Length)],
                    ReleaseYear = Random.Shared.Next(2000, 2023),
                    DurationMinutes = 0, // Total duration will be sum of episodes
                    Platform = platforms[Random.Shared.Next(platforms.Length)],
                    ImagePath = $"/Images/series{i}.jpg"
                };

                series.Add(seriesContent);
            }

            context.Contents.AddRange(series);
            context.SaveChanges();

            // Seed episodes for series
            foreach (var seriesContent in series)
            {
                int seasons = Random.Shared.Next(1, 6);

                for (int season = 1; season <= seasons; season++)
                {
                    int episodes = Random.Shared.Next(8, 16);

                    for (int episode = 1; episode <= episodes; episode++)
                    {
                        var episodeEntity = new Episode
                        {
                            Title = $"Episode {episode}",
                            SeasonNumber = season,
                            EpisodeNumber = episode,
                            DurationMinutes = Random.Shared.Next(30, 61),
                            Description = $"Season {season}, Episode {episode} of {seriesContent.Title}",
                            ContentId = seriesContent.Id
                        };

                        context.Episodes.Add(episodeEntity);
                    }
                }
            }

            context.SaveChanges();
        }

        private static void SeedViewingHistory(NetWatchDbContext context)
        {
            var users = context.Users.ToList();
            var contents = context.Contents.Include(c => c.Episodes).ToList();
            var viewingHistories = new List<ViewingHistory>();

            // Each user watches some content
            foreach (var user in users)
            {
                // Number of content items this user has watched
                int watchCount = Random.Shared.Next(5, 21);

                // Get random content items for this user
                var watchedContent = contents
                    .OrderBy(c => Guid.NewGuid()) // Random order
                    .Take(watchCount)
                    .ToList();

                foreach (var content in watchedContent)
                {
                    if (content.Type == ContentType.Movie)
                    {
                        // Create viewing history for movie
                        var viewingHistory = new ViewingHistory
                        {
                            UserId = user.Id,
                            ContentId = content.Id,
                            ViewDate = DateTime.Now.AddDays(-Random.Shared.Next(1, 180)),
                            WatchedMinutes = content.DurationMinutes,
                            Completed = Random.Shared.Next(100) < 80 // 80% chance of completing
                        };

                        viewingHistories.Add(viewingHistory);
                    }
                    else if (content.Type == ContentType.Series && content.Episodes.Any())
                    {
                        // For series, watch some episodes
                        int episodeCount = Random.Shared.Next(1, content.Episodes.Count + 1);
                        var watchedEpisodes = content.Episodes
                            .OrderBy(e => e.SeasonNumber)
                            .ThenBy(e => e.EpisodeNumber)
                            .Take(episodeCount)
                            .ToList();

                        foreach (var episode in watchedEpisodes)
                        {
                            var viewingHistory = new ViewingHistory
                            {
                                UserId = user.Id,
                                ContentId = content.Id,
                                EpisodeId = episode.Id,
                                ViewDate = DateTime.Now.AddDays(-Random.Shared.Next(1, 180)),
                                WatchedMinutes = episode.DurationMinutes,
                                Completed = Random.Shared.Next(100) < 85 // 85% chance of completing
                            };

                            viewingHistories.Add(viewingHistory);
                        }
                    }
                }
            }

            context.ViewingHistories.AddRange(viewingHistories);
            context.SaveChanges();
        }

        private static void SeedRatings(NetWatchDbContext context)
        {
            var users = context.Users.ToList();
            var contents = context.Contents.ToList();
            var ratings = new List<Rating>();

            // Each user rates some content
            foreach (var user in users)
            {
                // Number of content items this user has rated
                int rateCount = Random.Shared.Next(3, 11);

                // Get random content items for this user to rate
                var ratedContent = contents
                    .OrderBy(c => Guid.NewGuid()) // Random order
                    .Take(rateCount)
                    .ToList();

                foreach (var content in ratedContent)
                {
                    var rating = new Rating
                    {
                        UserId = user.Id,
                        ContentId = content.Id,
                        Score = Random.Shared.Next(1, 6), // Rating from 1 to 5
                        Comment = GetRandomComment(),
                        RatingDate = DateTime.Now.AddDays(-Random.Shared.Next(1, 90))
                    };

                    ratings.Add(rating);
                }
            }

            context.Ratings.AddRange(ratings);
            context.SaveChanges();
        }

        private static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }

        private static string GetRandomComment()
        {
            string[] comments =
            {
                "Really enjoyed this!",
                "Not my favorite, but still good.",
                "Absolutely loved it, would watch again.",
                "Decent, but could have been better.",
                "Amazing storyline and characters.",
                "The ending was disappointing.",
                "Great acting, weak plot.",
                "Highly recommend to everyone!",
                "Wouldn't watch again.",
                "A masterpiece!",
                "Mediocre at best.",
                "Exceeded my expectations.",
                "Waste of time.",
                "Perfect for a weekend watch.",
                "The visuals were stunning."
            };

            return comments[Random.Shared.Next(comments.Length)];
        }
    }
}