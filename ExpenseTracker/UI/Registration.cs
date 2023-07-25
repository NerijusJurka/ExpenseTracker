using ExpenseTracker;
using ExpenseTracker.Data;
using ExpenseTracker.Model;
using ExpenseTracker.Services;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

public class Registration
{
    private readonly IUserService userService;
    private readonly IPasswordHasher passwordHasher;
    private string connectionString;

    public Registration(string connectionString, IUserService userService, IPasswordHasher passwordHasher)
    {
        this.connectionString = connectionString;
        this.userService = userService;
        this.passwordHasher = passwordHasher;
    }

    public async Task SignUpAsync()
    {   
        try
        {
            string username, password, email;
            do
            {
                Console.Write("Enter username: ");
                username = Console.ReadLine();

                while (await IsUsernameTakenAsync(username))
                {
                    Console.WriteLine("Username is already taken. Please choose another username.");
                    Console.Write("Enter username: ");
                    username = Console.ReadLine();
                }

                Console.Write("Enter password: ");
                password = HidePasswordInput();

                Console.Write("Enter email: ");
                email = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(email))
                {
                    Console.WriteLine("Username, password, and email cannot be null or empty. Please try again.");
                }
                else if (!IsValidEmail(email))
                {
                    Console.WriteLine("Invalid email format. Please enter a valid email address.");
                }

            } while (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(email));

            // Generate password hash
            string passwordSalt = passwordHasher.GenerateSalt();

            string passwordHash = passwordHasher.HashPassword(password, passwordSalt);

            User user = new User(0, username, passwordHash, passwordSalt, email, false);

            await userService.CreateUserAsync(user);

            Console.WriteLine("User registered successfully.");
            Console.WriteLine("Redirecting to the main menu...");
            Thread.Sleep(5000); // Delay for 5 seconds (5000 milliseconds)
            Console.Clear();
            Program.DisplayMainMenu();
        }
        catch(Exception ex)
        {
            Console.WriteLine("An error occurred during registration: " + ex.ToString());
            Console.ReadLine(); // Wait for the user to press Enter before closing the console
        }
    }
    private bool IsValidEmail(string email)
    {
        // Email validation using regular expression
        string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
        return Regex.IsMatch(email, pattern);
    }

    private string HidePasswordInput()
    {
        StringBuilder password = new StringBuilder();
        ConsoleKeyInfo key;

        do
        {
            key = Console.ReadKey(true);

            // Append asterisk (*) character to password
            if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
            {
                password.Append(key.KeyChar);
                Console.Write("*");
            }
            else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                password.Remove(password.Length - 1, 1);
                Console.Write("\b \b");
            }
        }
        while (key.Key != ConsoleKey.Enter);

        Console.WriteLine();

        return password.ToString();
    }

    private async Task<bool> IsUsernameTakenAsync(string username)
    {
        var user = await userService.GetUserByUsernameAsync(username);
        return user != null;
    }
}