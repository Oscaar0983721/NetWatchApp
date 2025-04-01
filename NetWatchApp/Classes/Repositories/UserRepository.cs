using Microsoft.EntityFrameworkCore;
using NetWatchApp.Classes.Models;
using NetWatchApp.Data.EntityFramework;
using NetWatchApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NetWatchApp.Classes.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly NetWatchDbContext _context;

        public UserRepository(NetWatchDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> GetByIdentificationNumberAsync(string identificationNumber)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.IdentificationNumber == identificationNumber);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<bool> AddAsync(User user)
        {
            try
            {
                // Hash the password before storing
                user.PasswordHash = HashPassword(user.PasswordHash);

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(User user)
        {
            try
            {
                // Check if the password has been changed (not already hashed)
                var existingUser = await _context.Users.FindAsync(user.Id);
                if (existingUser != null && existingUser.PasswordHash != user.PasswordHash)
                {
                    // Hash the new password
                    user.PasswordHash = HashPassword(user.PasswordHash);
                }

                _context.Users.Update(user);
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
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                    return false;

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> ValidateCredentialsAsync(string identificationNumber, string password)
        {
            var user = await GetByIdentificationNumberAsync(identificationNumber);
            if (user == null)
                return false;

            return VerifyPassword(password, user.PasswordHash);
        }

        private string HashPassword(string password)
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

        private bool VerifyPassword(string password, string hashedPassword)
        {
            string hashedInput = HashPassword(password);
            return hashedInput == hashedPassword;
        }
    }
}