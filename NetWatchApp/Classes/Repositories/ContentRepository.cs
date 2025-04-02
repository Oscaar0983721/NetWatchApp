using Microsoft.EntityFrameworkCore;
using NetWatchApp.Classes.Models;
using NetWatchApp.Data.EntityFramework;
using NetWatchApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetWatchApp.Classes.Repositories
{
    public class ContentRepository : IContentRepository
    {
        private readonly NetWatchDbContext _context;

        public ContentRepository(NetWatchDbContext context)
        {
            _context = context;
        }

        public List<Content> GetAll()
        {
            return _context.Contents
                .Include(c => c.Episodes)
                .ToList();
        }

        public Content GetById(int id)
        {
            return _context.Contents
                .Include(c => c.Episodes)
                .FirstOrDefault(c => c.Id == id);
        }

        public List<Content> Search(string searchTerm, string genre = null, string type = null)
        {
            var query = _context.Contents
                .Include(c => c.Episodes)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(c => c.Title.Contains(searchTerm) || c.Description.Contains(searchTerm));
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

        public void Add(Content content)
        {
            _context.Contents.Add(content);
            _context.SaveChanges();
        }

        public void Update(Content content)
        {
            // Get existing content with episodes
            var existingContent = _context.Contents
                .Include(c => c.Episodes)
                .FirstOrDefault(c => c.Id == content.Id);

            if (existingContent == null)
            {
                throw new Exception($"Content with ID {content.Id} not found.");
            }

            // Update content properties
            existingContent.Title = content.Title;
            existingContent.Description = content.Description;
            existingContent.ReleaseYear = content.ReleaseYear;
            existingContent.Genre = content.Genre;
            existingContent.Type = content.Type;
            existingContent.Platform = content.Platform;
            existingContent.Duration = content.Duration;

            // Handle episodes
            if (content.Type == "Movie")
            {
                // Remove all episodes if content is now a movie
                _context.Episodes.RemoveRange(existingContent.Episodes);
                existingContent.Episodes.Clear();
            }
            else
            {
                // Update episodes for series
                // Remove episodes that are not in the updated list
                var episodesToRemove = existingContent.Episodes
                    .Where(e => !content.Episodes.Any(ne => ne.Id == e.Id))
                    .ToList();

                foreach (var episode in episodesToRemove)
                {
                    existingContent.Episodes.Remove(episode);
                    _context.Episodes.Remove(episode);
                }

                // Update existing episodes and add new ones
                foreach (var episode in content.Episodes)
                {
                    var existingEpisode = existingContent.Episodes
                        .FirstOrDefault(e => e.Id == episode.Id);

                    if (existingEpisode != null)
                    {
                        // Update existing episode
                        existingEpisode.EpisodeNumber = episode.EpisodeNumber;
                        existingEpisode.Title = episode.Title;
                        existingEpisode.Duration = episode.Duration;
                    }
                    else
                    {
                        // Add new episode
                        existingContent.Episodes.Add(new Episode
                        {
                            EpisodeNumber = episode.EpisodeNumber,
                            Title = episode.Title,
                            Duration = episode.Duration
                        });
                    }
                }
            }

            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var content = _context.Contents
                .Include(c => c.Episodes)
                .FirstOrDefault(c => c.Id == id);

            if (content != null)
            {
                // Remove related episodes first
                _context.Episodes.RemoveRange(content.Episodes);

                // Remove the content
                _context.Contents.Remove(content);
                _context.SaveChanges();
            }
        }

        public List<string> GetAllGenres()
        {
            return _context.Contents
                .Select(c => c.Genre)
                .Distinct()
                .OrderBy(g => g)
                .ToList();
        }

        public List<string> GetAllPlatforms()
        {
            return _context.Contents
                .Select(c => c.Platform)
                .Distinct()
                .OrderBy(p => p)
                .ToList();
        }
    }
}

