using ExpenseTracker.Data;
using ExpenseTracker.Model;
using System.Threading.Tasks;

namespace ExpenseTracker.Services
{
    public interface IUserService
    {
        string ConnectionString { get; }
        Task<User> GetUserByUsernameAsync(string username);
        string GenerateVerificationCode();
        Task CreateUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(User user);
        IDatabaseAccess DatabaseAccess { get; }
    }
}
