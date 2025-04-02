using Microsoft.EntityFrameworkCore;
using NetWatchApp.Classes.Models;
using NetWatchApp.Data.EntityFramework;
using NetWatchApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetWatchApp.Classes.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly NetWatchDbContext _context;

        public UserRepository(NetWatchDbContext context)
        {
            _context = context;
        }

        public List<User> GetAll()
        {
            return _context.Users.ToList();
        }

        public User GetById(int id)
        {
            return _context.Users.Find(id);
        }

        public User GetByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email);
        }

        public User GetByIdentificationNumber(string identificationNumber)
        {
            return _context.Users.FirstOrDefault(u => u.IdentificationNumber == identificationNumber);
        }

        public User Authenticate(string email, string password)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
        }

        public void Add(User user)
        {
            // Check if email already exists
            if (_context.Users.Any(u => u.Email == user.Email))
            {
                throw new Exception("Email already exists.");
            }

            // Check if identification number already exists
            if (_context.Users.Any(u => u.IdentificationNumber == user.IdentificationNumber))
            {
                throw new Exception("Identification number already exists.");
            }

            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void Update(User user)
        {
            var existingUser = _context.Users.Find(user.Id);
            if (existingUser == null)
            {
                throw new Exception($"User with ID {user.Id} not found.");
            }

            // Check if email already exists for another user
            if (_context.Users.Any(u => u.Email == user.Email && u.Id != user.Id))
            {
                throw new Exception("Email already exists.");
            }

            // Check if identification number already exists for another user
            if (_context.Users.Any(u => u.IdentificationNumber == user.IdentificationNumber && u.Id != user.Id))
            {
                throw new Exception("Identification number already exists.");
            }

            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Email = user.Email;
            existingUser.IdentificationNumber = user.IdentificationNumber;

            // Only update password if it's not empty
            if (!string.IsNullOrEmpty(user.Password))
            {
                existingUser.Password = user.Password;
            }

            existingUser.IsAdmin = user.IsAdmin;

            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }
    }
}

