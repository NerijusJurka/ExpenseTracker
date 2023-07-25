using ExpenseTracker.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Model
{
    public class User
    {
        public int Id { get; }
        public string Username { get; private set; }
        public string PasswordHash { get; private set; }
        public string PasswordSalt { get; private set; }
        public string Email { get; private set; }
        public bool IsActive { get; }
        public bool IsEmailVerified { get; private set; }
        public ICollection<Expense> Expenses { get; set; }
        public ICollection<Income> Incomes { get; set; }

        public User(int id, string username, string passwordHash, string passwordSalt, string email, bool isEmailVerified = true)
        {
            Id = id;
            Username = username ?? throw new ArgumentNullException(nameof(username));
            PasswordHash = passwordHash ?? throw new ArgumentNullException(nameof(passwordHash));
            PasswordSalt = passwordSalt ?? throw new ArgumentNullException(nameof(passwordSalt));
            Email = email ?? throw new ArgumentNullException(nameof(email));
            IsEmailVerified = isEmailVerified;
            IsActive = true;
        }

        public async Task VerifyEmail(User user, IUserService userService)
        {
            var emailService = new EmailService();

            var code = userService.GenerateVerificationCode();

            await emailService.SendEmail(user.Email, "Please verify your email", $"Your verification code is: {code}");

            Console.WriteLine("Verification email sent!");

            Console.WriteLine("Please enter the verification code you received in your email:");
            var enteredCode = Console.ReadLine();

            if (enteredCode == code)
            {
                Console.WriteLine("Email verified!");
                user.IsEmailVerified = true;

                // Save the updated user status to your database
                await userService.UpdateUserAsync(user);
            }
            else
            {
                Console.WriteLine("Verification code is incorrect.");
            }
        }

        public void ChangeUsername(string newUsername)
        {
            Username = newUsername ?? throw new ArgumentNullException(nameof(newUsername));
        }

    }
}
