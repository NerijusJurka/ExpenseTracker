using ExpenseTracker.Data;
using ExpenseTracker.Model;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace ExpenseTracker.UI
{
    public class Login : DatabaseAccess
    {
        public Login(string connectionString) : base(connectionString)
        {
        }

        public void SignIn(string username, string password)
        {
            string query = "SELECT PasswordHash FROM Users WHERE Username = @Username";
            SqlParameter parameter = new SqlParameter("@Username", username);

            string passwordHashFromDb = ExecuteScalar<string>(query, parameter);

            if (!string.IsNullOrEmpty(passwordHashFromDb))
            {
                string enteredPasswordHash = GetPasswordHash(password);

                if (passwordHashFromDb.Equals(enteredPasswordHash))
                {
                    Console.WriteLine("Login successful!");
                    var expenseTrackerDashboard = new ExpenseTrackerDashboard(connectionString);
                    var user = GetUserByUsername(username);

                    if (user != null)
                    {
                        expenseTrackerDashboard.DisplayDashboard(user);
                    }
                    else
                    {
                        Console.WriteLine("Failed to fetch user data from the database.");
                    }
                }
                else
                {
                    Console.WriteLine("Username or password is incorrect.");
                    return;
                }
            }
            else
            {
                Console.WriteLine("Username or password is incorrect.");
                return;
            }
        }

        private string GetPasswordHash(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha256.ComputeHash(passwordBytes);
                return BitConverter.ToString(hashBytes).Replace("-", string.Empty);
            }
        }

        private User GetUserByUsername(string username)
        {
            string query = "SELECT Id, Username, PasswordHash, Email FROM Users WHERE Username = @Username";
            SqlParameter parameter = new SqlParameter("@Username", username);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(parameter);

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

                            return new User
                            {
                                Id = userId,
                                Username = fetchedUsername,
                                PasswordHash = fetchedPassword,
                                Email = fetchedEmail
                            };
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