using ExpenseTracker.Model;
using ExpenseTracker.Services;

namespace ExpenseTracker.UI
{
    public class Login
    {
        private string username;
        private string password;
        private readonly IUserService userService;
        private readonly IPasswordHasher passwordHasher;

        public Login(string username, string password, IUserService userService, IPasswordHasher passwordHasher)
        {
            this.username = username;
            this.password = password;
            this.userService = userService;
            this.passwordHasher = passwordHasher;
        }

        public async Task SignInAsync(string username, string password)
        {
            User user = await userService.GetUserByUsernameAsync(username);
            if (user == null)
            {
                Console.WriteLine("Username or password is incorrect.");
                return;
            }

            bool isPasswordValid = passwordHasher.ValidatePassword(password, user.PasswordHash, user.PasswordSalt);
            if (!isPasswordValid)
            {
                Console.WriteLine("Username or password is incorrect.");
                return;
            }

            Console.WriteLine("Login successful!");
            var connectionString = userService.DatabaseAccess.ConnectionString;
            var expenseTrackerDashboard = new ExpenseTrackerDashboard(connectionString);
            expenseTrackerDashboard.DisplayDashboard(user);
        }
    }
}