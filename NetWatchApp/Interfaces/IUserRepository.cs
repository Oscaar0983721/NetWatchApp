using NetWatchApp.Classes.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NetWatchApp.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(int id);
        Task<User> GetByIdentificationNumberAsync(string identificationNumber);
        Task<IEnumerable<User>> GetAllAsync();
        Task<bool> AddAsync(User user);
        Task<bool> UpdateAsync(User user);
        Task<bool> DeleteAsync(int id);
        Task<bool> ValidateCredentialsAsync(string identificationNumber, string password);
    }
}