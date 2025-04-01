using NetWatchApp.Classes.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetWatchApp.Interfaces
{
    public interface IContentRepository
    {
        Task<Content> GetByIdAsync(int id);
        Task<IEnumerable<Content>> GetAllAsync();
        Task<IEnumerable<Content>> GetByTypeAsync(ContentType type);
        Task<IEnumerable<Content>> GetByGenreAsync(string genre);
        Task<IEnumerable<Content>> SearchAsync(string searchTerm);
        Task<bool> AddAsync(Content content);
        Task<bool> UpdateAsync(Content content);
        Task<bool> DeleteAsync(int id);
    }
}