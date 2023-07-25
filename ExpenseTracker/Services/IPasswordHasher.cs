using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Services
{
    public interface IPasswordHasher
    {
        string HashPassword(string password, string salt);
        bool ValidatePassword(string enteredPassword, string storedHash, string salt);
        string GenerateSalt();
    }
}
