using System;

namespace ExpenseTracker
{
    class Program
    {
        static void Main(string[] args)
        {
            DisplayMainMenu();
        }

        public static void DisplayMainMenu()
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
                    PerformLogin();
                    break;
                case "2":
                    PerformRegistration();
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

        private static void PerformLogin()
        {
            Console.Clear();
            Console.WriteLine("Login");
            Console.WriteLine("=====");

            Console.Write("Enter username: ");
            string username = Console.ReadLine();

            Console.Write("Enter password: ");
            string password = Console.ReadLine();

            Login login = new Login();
            login.SignIn(username, password);
        }

        private static void PerformRegistration()
        {
            Console.Clear();
            Console.WriteLine("Registration");
            Console.WriteLine("============");

            Registration registration = new Registration();
            registration.Signup();
        }
    }
}