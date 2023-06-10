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
            Console.WriteLine("Expense Tracker");
            Console.WriteLine("1. Login");
            Console.WriteLine("2. Registration");
            Console.WriteLine("3. Exit");

            string userInput = Console.ReadLine();

            switch (userInput)
            {
                case "1":
                    Login login = new Login();
                    Console.Write("Enter username: ");
                    string username = Console.ReadLine();
                    Console.Write("Enter password: ");
                    string password = Console.ReadLine();
                    login.SignIn(username, password);
                    break;
                case "2":
                    Registration registration = new Registration();
                    registration.Signup();
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
            
    }
}