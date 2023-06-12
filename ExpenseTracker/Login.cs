using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace ExpenseTracker
{
    public class Login
    {
        public void SignIn(string username, string password)
        {
            // Connection string
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\jurka\\source\\repos\\ExpenseTracker\\ExpenseTracker\\Database.mdf;Integrated Security=True";

            string query = "SELECT PasswordHash FROM Users WHERE Username = @Username";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);

                    try
                    {
                        connection.Open();
                        string passwordHashFromDb = (string)command.ExecuteScalar();

                        if (passwordHashFromDb != null)
                        {
                            // Hash the entered password
                            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                            byte[] hashBytes;
                            using (SHA256 sha256 = SHA256.Create())
                            {
                                hashBytes = sha256.ComputeHash(passwordBytes);
                            }
                            string enteredPasswordHash = BitConverter.ToString(hashBytes).Replace("-", string.Empty);

                            // Compare the hashed passwords
                            if (passwordHashFromDb.Equals(enteredPasswordHash))
                            {
                                Console.WriteLine("Login successful!");
                                var expenseTrackerDashBoard = new ExpenseTrackerDashboard(connectionString);
                                var user = GetUserByUsername(username);

                                if (user != null)
                                {
                                    expenseTrackerDashBoard.DisplayDashboard(user);
                                }
                                else
                                {
                                    Console.WriteLine("Failed to fetch user data from the database.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Username or password is incorrect.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Username or password is incorrect.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }
            }
        }
        private User GetUserByUsername(string username)
        {
            string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\jurka\\source\\repos\\ExpenseTracker\\ExpenseTracker\\Database.mdf;Integrated Security=True";

            string query = "SELECT Id, Username, PasswordHash, Email FROM Users WHERE Username = @Username";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);

                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            int userId = (int)reader["Id"];
                            string fetchedUsername = (string)reader["Username"];
                            string fetchedPassword = (string)reader["PasswordHash"];
                            string fetchedEmail = (string)reader["Email"];

                            // Create a new User object and populate it with the fetched data
                            User user = new User
                            {
                                Id = userId,
                                Username = fetchedUsername,
                                PasswordHash = fetchedPassword,
                                Email = fetchedEmail
                            };

                            return user;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }
            }

            return null; // User not found
        }
    }
}