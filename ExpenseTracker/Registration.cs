using ExpenseTracker;
using Microsoft.Data.SqlClient;
using System.Threading;
public class Registration
{
    public void Signup()
    {
        Console.Write("Enter username: ");
        string username = Console.ReadLine();
        Console.Write("Enter password: ");
        string password = Console.ReadLine();
        Console.Write("Enter email: ");
        string email = Console.ReadLine();

        // Connection string
        string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\jurka\\source\\repos\\ExpenseTracker\\ExpenseTracker\\Database.mdf;Integrated Security=True";

        string tableName = "Users";
        string queryCheckTableExists = $"SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{tableName}'";
        string queryCreateTable = $"CREATE TABLE {tableName} (Id INT PRIMARY KEY IDENTITY, Username NVARCHAR(50) NOT NULL, Password NVARCHAR(50) NOT NULL, Email NVARCHAR(50) NOT NULL)";

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

                    // Insert the user data
                    command.CommandText = "INSERT INTO Users (Username, Password, Email) VALUES (@Username, @Password, @Email); SELECT SCOPE_IDENTITY();";
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);
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
}