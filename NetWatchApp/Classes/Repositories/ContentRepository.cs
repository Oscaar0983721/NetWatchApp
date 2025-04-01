using Microsoft.EntityFrameworkCore;
using NetWatchApp.Classes.Models;
using NetWatchApp.Data.EntityFramework;
using NetWatchApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetWatchApp.Classes.Repositories
{
    public class ContentRepository : IContentRepository
    {
        private readonly NetWatchDbContext _context;

        public ContentRepository(NetWatchDbContext context)
        {
            _context = context;
        }

        public async Task<Content> GetByIdAsync(int id)
        {
            return await _context.Contents
                .Include(c => c.Episodes)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Content>> GetAllAsync()
        {
            return await _context.Contents
                .Include(c => c.Episodes)
                .ToListAsync();
        }

        public async Task<IEnumerable<Content>> GetByTypeAsync(ContentType type)
        {
            return await _context.Contents
                .Include(c => c.Episodes)
                .Where(c => c.Type == type)
                .ToListAsync();
        }

        public async Task<IEnumerable<Content>> GetByGenreAsync(string genre)
        {
            return await _context.Contents
                .Include(c => c.Episodes)
                .Where(c => c.Genre == genre)
                .ToListAsync();
        }

        public async Task<IEnumerable<Content>> SearchAsync(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
                return await GetAllAsync();

            return await _context.Contents
                .Include(c => c.Episodes)
                .Where(c => c.Title.Contains(searchTerm) ||
                           c.Description.Contains(searchTerm) ||
                           c.Genre.Contains(searchTerm))
                .ToListAsync();
        }

        public async Task<bool> AddAsync(Content content)
        {
            try
            {
                await _context.Contents.AddAsync(content);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(Content content)
        {
            try
            {
                _context.Contents.Update(content);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var content = await _context.Contents.FindAsync(id);
                if (content == null)
                    return false;

                _context.Contents.Remove(content);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}