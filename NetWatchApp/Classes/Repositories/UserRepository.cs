using NetWatchApp.Classes.Models;
using NetWatchApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetWatchApp.Classes.Repositories
{
    public class UserRepository
    {
        private readonly JsonDataService _jsonDataService;

        public UserRepository()
        {
            _jsonDataService = new JsonDataService();
        }

        public List<User> GetAll()
        {
            return _jsonDataService.GetAllUsers();
        }

        public User GetById(int id)
        {
            return _jsonDataService.GetUserById(id);
        }

        public User GetByEmail(string email)
        {
            return _jsonDataService.GetUserByEmail(email);
        }

        public User GetByIdentificationNumber(string identificationNumber)
        {
            return _jsonDataService.GetUserByIdentificationNumber(identificationNumber);
        }

        public User Authenticate(string email, string password)
        {
            return _jsonDataService.AuthenticateUser(email, password);
        }

        public void Add(User user)
        {
            _jsonDataService.AddUser(user);
        }

        public void Update(User user)
        {
            _jsonDataService.UpdateUser(user);
        }

        public void Delete(int id)
        {
            _jsonDataService.DeleteUser(id);
        }
    }
}

