using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetWatchApp.Classes.Models;
using NetWatchApp.Data.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetWatchApp.Classes.Repositories
{
    [TestClass]
    public class ContentRepositoryTests
    {
        private NetWatchDbContext _context;
        private ContentRepository _repository;

        [TestInitialize]
        public void Initialize()
        {
            // Create options for in-memory database
            var options = new DbContextOptionsBuilder<NetWatchDbContext>()
                .UseInMemoryDatabase(databaseName: "NetWatchTestDb")
                .Options;

            // Create context with in-memory database
            _context = new NetWatchDbContext(options);

            // Clear database
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            // Create repository
            _repository = new ContentRepository(_context);

            // Seed test data
            SeedTestData();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Dispose();
        }

        private void SeedTestData()
        {
            // Add test content
            var movie = new Content
            {
                Title = "Test Movie",
                Description = "A test movie description",
                ReleaseYear = 2023,
                Genre = "Action",
                Type = "Movie",
                Platform = "Netflix",
                Duration = 120,
                ImagePath = "https://example.com/test-movie.jpg"
            };
            _context.Contents.Add(movie);

            var series = new Content
            {
                Title = "Test Series",
                Description = "A test series description",
                ReleaseYear = 2022,
                Genre = "Drama",
                Type = "Series",
                Platform = "Amazon Prime",
                Duration = 0,
                ImagePath = "https://example.com/test-series.jpg"
            };

            // Add episodes to series
            series.Episodes.Add(new Episode
            {
                EpisodeNumber = 1,
                Title = "Pilot",
                Duration = 45
            });
            series.Episodes.Add(new Episode
            {
                EpisodeNumber = 2,
                Title = "Second Episode",
                Duration = 42
            });

            _context.Contents.Add(series);
            _context.SaveChanges();
        }

        [TestMethod]
        public void GetAll_ReturnsAllContent()
        {
            // Act
            var result = _repository.GetAll();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Any(c => c.Title == "Test Movie"));
            Assert.IsTrue(result.Any(c => c.Title == "Test Series"));
        }

        [TestMethod]
        public void GetById_ReturnsCorrectContent()
        {
            // Arrange
            var movie = _context.Contents.FirstOrDefault(c => c.Title == "Test Movie");

            // Act
            var result = _repository.GetById(movie.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Test Movie", result.Title);
            Assert.AreEqual("Action", result.Genre);
        }

        [TestMethod]
        public void Search_ByTitle_ReturnsMatchingContent()
        {
            // Act
            var result = _repository.Search("Movie");

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Test Movie", result[0].Title);
        }

        [TestMethod]
        public void Search_ByGenre_ReturnsMatchingContent()
        {
            // Act
            var result = _repository.Search("", "Drama");

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Test Series", result[0].Title);
        }

        [TestMethod]
        public void Search_ByType_ReturnsMatchingContent()
        {
            // Act
            var result = _repository.Search("", null, "Series");

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Test Series", result[0].Title);
        }

        [TestMethod]
        public void Add_AddsNewContent()
        {
            // Arrange
            var newContent = new Content
            {
                Title = "New Test Content",
                Description = "A new test content description",
                ReleaseYear = 2024,
                Genre = "Comedy",
                Type = "Movie",
                Platform = "Disney+",
                Duration = 95,
                ImagePath = "https://example.com/new-test.jpg"
            };

            // Act
            _repository.Add(newContent);

            // Assert
            var result = _context.Contents.FirstOrDefault(c => c.Title == "New Test Content");
            Assert.IsNotNull(result);
            Assert.AreEqual("Comedy", result.Genre);
            Assert.AreEqual(2024, result.ReleaseYear);
        }

        [TestMethod]
        public void Update_UpdatesExistingContent()
        {
            // Arrange
            var content = _context.Contents.FirstOrDefault(c => c.Title == "Test Movie");
            content.Title = "Updated Movie Title";
            content.Genre = "Sci-Fi";

            // Act
            _repository.Update(content);

            // Assert
            var result = _context.Contents.Find(content.Id);
            Assert.AreEqual("Updated Movie Title", result.Title);
            Assert.AreEqual("Sci-Fi", result.Genre);
        }

        [TestMethod]
        public void Delete_RemovesContent()
        {
            // Arrange
            var content = _context.Contents.FirstOrDefault(c => c.Title == "Test Movie");
            var contentId = content.Id;

            // Act
            _repository.Delete(contentId);

            // Assert
            var result = _context.Contents.Find(contentId);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetAllGenres_ReturnsUniqueGenres()
        {
            // Act
            var result = _repository.GetAllGenres();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Contains("Action"));
            Assert.IsTrue(result.Contains("Drama"));
        }

        [TestMethod]
        public void GetAllPlatforms_ReturnsUniquePlatforms()
        {
            // Act
            var result = _repository.GetAllPlatforms();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Contains("Netflix"));
            Assert.IsTrue(result.Contains("Amazon Prime"));
        }
    }
}

