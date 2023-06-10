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
                                // Another class call
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
    }
}