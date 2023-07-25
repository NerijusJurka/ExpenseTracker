using ExpenseTracker.Data;
using ExpenseTracker.Model;
using Microsoft.Data.SqlClient;

namespace ExpenseTracker.Services
{
    public class UserService : IUserService
    {
        public string ConnectionString { get; }
        public IDatabaseAccess DatabaseAccess { get; }
        private readonly IDatabaseAccess databaseAccess;

        public UserService(IDatabaseAccess databaseAccess)
        {
            DatabaseAccess = databaseAccess;
            this.databaseAccess = databaseAccess;
            this.ConnectionString = databaseAccess.ConnectionString;
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            string query = "SELECT Id, Username, PasswordHash, PasswordSalt, Email FROM Users WHERE Username = @Username";
            SqlParameter parameter = new SqlParameter("@Username", username);

            using (SqlConnection connection = new SqlConnection(databaseAccess.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add(parameter);

                    try
                    {
                        await connection.OpenAsync();
                        SqlDataReader reader = await command.ExecuteReaderAsync();

                        if (await reader.ReadAsync())
                        {
                            int userId = (int)reader["Id"];
                            string fetchedUsername = (string)reader["Username"];
                            string fetchedPasswordHash = (string)reader["PasswordHash"];
                            string fetchedPasswordSalt = (string)reader["PasswordSalt"];
                            string fetchedEmail = (string)reader["Email"];
                            bool fetchedIsEmailVerified = (bool)reader["IsEmailVerified"];
                            return new User(userId, fetchedUsername, fetchedPasswordHash, fetchedPasswordSalt, fetchedEmail, fetchedIsEmailVerified);
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
        public async Task UpdateUserAsync(User user)
        {
            string query = "UPDATE Users SET Username = @Username, PasswordHash = @PasswordHash, PasswordSalt = @PasswordSalt, Email = @Email WHERE Id = @Id";

            using (SqlConnection connection = new SqlConnection(databaseAccess.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", user.Username);
                    command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
                    command.Parameters.AddWithValue("@PasswordSalt", user.PasswordSalt);
                    command.Parameters.AddWithValue("@Email", user.Email);
                    command.Parameters.AddWithValue("@Id", user.Id);

                    try
                    {
                        await connection.OpenAsync();
                        await command.ExecuteNonQueryAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }
            }
        }
        public async Task CreateUserAsync(User user)
        {
            string query = "INSERT INTO Users (Username, PasswordHash, PasswordSalt, Email, IsEmailVerified, IsActive) VALUES (@Username, @PasswordHash, @PasswordSalt, @Email, @IsEmailVerified, @IsActive)";

            using (SqlConnection connection = new SqlConnection(databaseAccess.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", user.Username);
                    command.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
                    command.Parameters.AddWithValue("@PasswordSalt", user.PasswordSalt);
                    command.Parameters.AddWithValue("@Email", user.Email);
                    command.Parameters.AddWithValue("@IsEmailVerified", user.IsEmailVerified);
                    command.Parameters.AddWithValue("@IsActive", user.IsActive);

                    try
                    {
                        await connection.OpenAsync();
                        await command.ExecuteNonQueryAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }
            }
        }
        public string GenerateVerificationCode()
        {
            Random random = new Random();
            return random.Next(1000, 9999).ToString();
        }


        public async Task DeleteUserAsync(User user)
        {
            string query = "DELETE FROM Users WHERE Id = @Id";

            using (SqlConnection connection = new SqlConnection(databaseAccess.ConnectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", user.Id);

                    try
                    {
                        await connection.OpenAsync();
                        await command.ExecuteNonQueryAsync();
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
