using System;
using ExpenseTracker.Data;
using ExpenseTracker.Services;
using ExpenseTracker.UI;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;

namespace ExpenseTracker
{
    class Program
    {
        public static IConfigurationRoot Configuration { get; set; }
        public static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json");

            Configuration = builder.Build();

            var connectionString = Configuration.GetSection("ConnectionSettings:ConnectionString").Value;
            var connectionSettings = new ConnectionSettings { ConnectionString = connectionString };
            var options = Options.Create(connectionSettings);

            DatabaseManager databaseManager = new DatabaseManager(options);
            databaseManager.EnsureDatabaseTableExists();

            await DisplayMainMenu();
        }

        public static async Task DisplayMainMenu()
        {
            Console.Clear();
            Console.WriteLine("Expense Tracker");
            Console.WriteLine("================");
            Console.WriteLine("1. Login");
            Console.WriteLine("2. Registration");
            Console.WriteLine("3. Exit");

            Console.Write("Enter your choice: ");
            string userInput = Console.ReadLine();

            switch (userInput)
            {
                case "1":
                    await PerformLogin();
                    break;
                case "2":
                    await PerformRegistration();
                    break;
                case "3":
                    Console.WriteLine("Exiting...");
                    return;
                default:
                    Console.WriteLine("Invalid option. Please select a valid option.");
                    break;
            }

            Console.ReadLine();
        }

        private static async Task PerformLogin()
        {
            Console.Clear();
            Console.WriteLine("Login");
            Console.WriteLine("=====");

            Console.Write("Enter username: ");
            string username = Console.ReadLine();

            Console.Write("Enter password: ");
            string password = Console.ReadLine();

            string connectionString = Configuration.GetSection("ConnectionSettings:ConnectionString").Value;

            var connectionSettings = new ConnectionSettings { ConnectionString = connectionString };
            var options = Options.Create(connectionSettings);

            IDatabaseAccess databaseAccess = new DatabaseManager(options);
            var passwordHasher = new PasswordHasher();
            IUserService userService = new UserService(databaseAccess);

            Login login = new Login(username, password, userService, passwordHasher);
            login.SignInAsync(username, password);
        }

        private static async Task PerformRegistration()
        {
            Console.Clear();
            Console.WriteLine("Registration");
            Console.WriteLine("============");

            var connectionString = Configuration.GetSection("ConnectionSettings:ConnectionString").Value;

            var connectionSettings = new ConnectionSettings { ConnectionString = connectionString };
            var options = Options.Create(connectionSettings);

            IDatabaseAccess databaseAccess = new DatabaseManager(options);

            var passwordHasher = new PasswordHasher();
            IUserService userService = new UserService(databaseAccess);

            Registration registration = new Registration(connectionString, userService, passwordHasher);
            await registration.SignUpAsync();
        }
    }
}