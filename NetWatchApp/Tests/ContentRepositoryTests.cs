using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetWatchApp.Classes.Models;
using NetWatchApp.Classes.Repositories;
using NetWatchApp.Data.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetWatchApp.Tests
{
    [TestClass]
    public class ContentRepositoryTests
    {
        private NetWatchDbContext _context;
        private ContentRepository _repository;

        [TestInitialize]
        public void Initialize()
        {
            // Set up in-memory database for testing
            var options = new DbContextOptionsBuilder<NetWatchDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new NetWatchDbContext(options);
            _repository = new ContentRepository(_context);

            // Seed test data
            SeedTestData();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        private void SeedTestData()
        {
            // Add test movies
            var movie1 = new Content
            {
                Title = "Test Movie 1",
                Description = "Description for test movie 1",
                ReleaseYear = 2020,
                Genre = "Action",
                Type = "Movie",
                Platform = "Netflix",
                Duration = 120,
                ImagePath = "Images/test1.jpg"
            };

            var movie2 = new Content
            {
                Title = "Test Movie 2",
                Description = "Description for test movie 2",
                ReleaseYear = 2021,
                Genre = "Comedy",
                Type = "Movie",
                Platform = "Amazon Prime",
                Duration = 90,
                ImagePath = "Images/test2.jpg"
            };

            // Add test series with episodes
            var series1 = new Content
            {
                Title = "Test Series 1",
                Description = "Description for test series 1",
                ReleaseYear = 2019,
                Genre = "Drama",
                Type = "Series",
                Platform = "Netflix",
                Duration = 0,
                ImagePath = "Images/test3.jpg"
            };

            series1.Episodes.Add(new Episode
            {
                EpisodeNumber = 1,
                Title = "Episode 1",
                Duration = 45
            });

            series1.Episodes.Add(new Episode
            {
                EpisodeNumber = 2,
                Title = "Episode 2",
                Duration = 50
            });

            _context.Contents.Add(movie1);
            _context.Contents.Add(movie2);
            _context.Contents.Add(series1);
            _context.SaveChanges();
        }

        [TestMethod]
        public void GetAll_ShouldReturnAllContents()
        {
            // Act
            var result = _repository.GetAll();

            // Assert
            Assert.AreEqual(3, result.Count);
            Assert.IsTrue(result.Any(c => c.Title == "Test Movie 1"));
            Assert.IsTrue(result.Any(c => c.Title == "Test Movie 2"));
            Assert.IsTrue(result.Any(c => c.Title == "Test Series 1"));
        }

        [TestMethod]
        public void GetById_ShouldReturnCorrectContent()
        {
            // Arrange
            var expectedContent = _context.Contents.First(c => c.Title == "Test Movie 1");

            // Act
            var result = _repository.GetById(expectedContent.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedContent.Id, result.Id);
            Assert.AreEqual("Test Movie 1", result.Title);
            Assert.AreEqual("Action", result.Genre);
        }

        [TestMethod]
        public void GetById_WithInvalidId_ShouldReturnNull()
        {
            // Act
            var result = _repository.GetById(-1);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Search_ByTitle_ShouldReturnMatchingContents()
        {
            // Act
            var result = _repository.Search("Movie");

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.All(c => c.Title.Contains("Movie")));
        }

        [TestMethod]
        public void Search_ByGenre_ShouldReturnMatchingContents()
        {
            // Act
            var result = _repository.Search("", "Action");

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Action", result.First().Genre);
        }

        [TestMethod]
        public void Search_ByType_ShouldReturnMatchingContents()
        {
            // Act
            var result = _repository.Search("", null, "Series");

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Series", result.First().Type);
        }

        [TestMethod]
        public void Add_ShouldAddNewContent()
        {
            // Arrange
            var newContent = new Content
            {
                Title = "New Test Movie",
                Description = "Description for new test movie",
                ReleaseYear = 2022,
                Genre = "Horror",
                Type = "Movie",
                Platform = "Disney+",
                Duration = 110,
                ImagePath = "Images/new.jpg"
            };

            // Act
            _repository.Add(newContent);
            var result = _repository.GetAll();
            var addedContent = _repository.GetById(newContent.Id);

            // Assert
            Assert.AreEqual(4, result.Count);
            Assert.IsNotNull(addedContent);
            Assert.AreEqual("New Test Movie", addedContent.Title);
            Assert.AreEqual("Horror", addedContent.Genre);
        }

        [TestMethod]
        public void Update_ShouldUpdateExistingContent()
        {
            // Arrange
            var contentToUpdate = _context.Contents.First(c => c.Title == "Test Movie 1");
            contentToUpdate.Title = "Updated Movie Title";
            contentToUpdate.Genre = "Sci-Fi";

            // Act
            _repository.Update(contentToUpdate);
            var updatedContent = _repository.GetById(contentToUpdate.Id);

            // Assert
            Assert.AreEqual("Updated Movie Title", updatedContent.Title);
            Assert.AreEqual("Sci-Fi", updatedContent.Genre);
        }

        [TestMethod]
        public void Delete_ShouldRemoveContent()
        {
            // Arrange
            var contentToDelete = _context.Contents.First(c => c.Title == "Test Movie 2");
            var contentId = contentToDelete.Id;

            // Act
            _repository.Delete(contentId);
            var result = _repository.GetAll();
            var deletedContent = _repository.GetById(contentId);

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.IsNull(deletedContent);
        }

        [TestMethod]
        public void Delete_ShouldRemoveContentWithEpisodes()
        {
            // Arrange
            var contentToDelete = _context.Contents.First(c => c.Title == "Test Series 1");
            var contentId = contentToDelete.Id;

            // Act
            _repository.Delete(contentId);
            var result = _repository.GetAll();
            var deletedContent = _repository.GetById(contentId);
            var episodes = _context.Episodes.Where(e => e.ContentId == contentId).ToList();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.IsNull(deletedContent);
            Assert.AreEqual(0, episodes.Count);
        }

        [TestMethod]
        public void GetAllGenres_ShouldReturnUniqueGenres()
        {
            // Act
            var result = _repository.GetAllGenres();

            // Assert
            Assert.AreEqual(3, result.Count);
            Assert.IsTrue(result.Contains("Action"));
            Assert.IsTrue(result.Contains("Comedy"));
            Assert.IsTrue(result.Contains("Drama"));
        }

        [TestMethod]
        public void GetAllPlatforms_ShouldReturnUniquePlatforms()
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

