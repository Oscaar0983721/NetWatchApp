using NetWatchApp.Classes.Models;
using System.Collections.Generic;

namespace NetWatchApp.Interfaces
{
    public interface IUserRepository
    {
        List<User> GetAll();
        User GetById(int id);
        User GetByEmail(string email);
        User GetByIdentificationNumber(string identificationNumber);
        User Authenticate(string email, string password);
        void Add(User user);
        void Update(User user);
        void Delete(int id);
    }
}

