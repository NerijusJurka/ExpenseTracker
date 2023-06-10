using ExpenseTracker;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

public class Registration
{
    public void Signup()
    {
        string username, password, email;

        do
        {
            Console.Write("Enter username: ");
            username = Console.ReadLine();

            while (IsUsernameTaken(username))
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
        
            // Connection string
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\jurka\\source\\repos\\ExpenseTracker\\ExpenseTracker\\Database.mdf;Integrated Security=True";

            string tableName = "Users";
            string queryCheckTableExists = $"SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{tableName}'";
            string queryCreateTable = $"CREATE TABLE {tableName} (Id INT PRIMARY KEY IDENTITY, Username NVARCHAR(50) NOT NULL, PasswordHash NVARCHAR(64) NOT NULL, Email NVARCHAR(50) NOT NULL)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(queryCheckTableExists, connection))
                {
                    try
                    {
                        connection.Open();
                        int tableCount = (int)command.ExecuteScalar();

                        if (tableCount == 0)
                        {
                            // Table does not exist, create it
                            command.CommandText = queryCreateTable;
                            command.ExecuteNonQuery();
                            Console.WriteLine("Table created successfully.");
                        }

                        // Generate password hash
                        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                        byte[] hashBytes;
                        using (SHA256 sha256 = SHA256.Create())
                        {
                            hashBytes = sha256.ComputeHash(passwordBytes);
                        }
                        string passwordHash = BitConverter.ToString(hashBytes).Replace("-", string.Empty);

                        // Insert the user data
                        command.CommandText = "INSERT INTO Users (Username, PasswordHash, Email) VALUES (@Username, @PasswordHash, @Email); SELECT SCOPE_IDENTITY();";
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@PasswordHash", passwordHash);
                        command.Parameters.AddWithValue("@Email", email);
                        int userId = Convert.ToInt32(command.ExecuteScalar());
                        Console.WriteLine($"User with ID {userId} registered successfully.");
                        Console.WriteLine("Redirecting to the main menu...");
                        Thread.Sleep(5000); // Delay for 5 seconds (5000 milliseconds)
                        Console.Clear();
                        Program.DisplayMainMenu();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }
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
    private bool IsUsernameTaken(string username)
    {
        // Connection string
        string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\jurka\\source\\repos\\ExpenseTracker\\ExpenseTracker\\Database.mdf;Integrated Security=True";

        string query = "SELECT COUNT(*) FROM Users WHERE Username = @Username";

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Username", username);

                try
                {
                    connection.Open();
                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        return false;
    }
}