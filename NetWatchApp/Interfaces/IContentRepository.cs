using NetWatchApp.Classes.Models;
using System.Collections.Generic;

namespace NetWatchApp.Interfaces
{
    public interface IContentRepository
    {
        List<Content> GetAll();
        Content GetById(int id);
        List<Content> Search(string searchTerm, string genre = null, string type = null);
        void Add(Content content);
        void Update(Content content);
        void Delete(int id);
        List<string> GetAllGenres();
        List<string> GetAllPlatforms();
    }
}

