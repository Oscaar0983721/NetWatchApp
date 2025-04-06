using Microsoft.EntityFrameworkCore;
using NetWatchApp.Classes.Models;
using NetWatchApp.Data.EntityFramework;
using NetWatchApp.Interfaces;
using NetWatchApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetWatchApp.Classes.Repositories
{
    public class ContentRepository : IContentRepository
    {
        private readonly JsonDataService _jsonDataService;
        private readonly NetWatchDbContext _context;

        public ContentRepository(NetWatchDbContext context)
        {
            _context = context;
            _jsonDataService = new JsonDataService();
        }

        public List<Content> GetAll()
        {
            return _jsonDataService.GetAllContent();
        }

        public Content GetById(int id)
        {
            return _jsonDataService.GetContentById(id);
        }

        public List<Content> Search(string searchTerm, string genre = null, string type = null)
        {
            return _jsonDataService.SearchContent(searchTerm, genre, type);
        }

        public void Add(Content content)
        {
            _jsonDataService.AddContent(content);
        }

        public void Update(Content content)
        {
            _jsonDataService.UpdateContent(content);
        }

        public void Delete(int id)
        {
            _jsonDataService.DeleteContent(id);
        }

        public List<string> GetAllGenres()
        {
            return _jsonDataService.GetAllGenres();
        }

        public List<string> GetAllPlatforms()
        {
            return _jsonDataService.GetAllPlatforms();
        }
    }
}

