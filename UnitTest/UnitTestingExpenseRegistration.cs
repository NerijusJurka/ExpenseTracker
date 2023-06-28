using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using Moq;
using System.Text;


namespace UnitTest
{
    public class UnitTestSqlConnectionWrapper
    {
        private readonly SqlConnection connection;

        public UnitTestSqlConnectionWrapper(string connectionString)
        {
            connection = new SqlConnection(connectionString);
        }

        public void Open() => connection.Open();
        public void Dispose() => connection.Dispose();
    }

    public class Registration
    {
        public Func<ISqlConnectionWrapper> SqlConnectionWrapperFactory { get; set; }

        public void Signup(string username, string password, string email)
        {
            // Connection string
            string connectionString = "your_connection_string";

            using (ISqlConnectionWrapper connection = SqlConnectionWrapperFactory.Invoke())
            {
                using (ISqlCommandWrapper command = connection.CreateCommand())
                {
                    command.AddParameter("@Username", username);
                    string query = "SELECT COUNT(*) FROM Users WHERE Username = @Username";

                    try
                    {
                        connection.Open();
                        int count = (int)command.ExecuteScalar();

                        if (count > 0)
                        {
                            Console.WriteLine("Username is already taken.");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                        return;
                    }

                    byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                    byte[] hashBytes;
                    using (SHA256 sha256 = SHA256.Create())
                    {
                        hashBytes = sha256.ComputeHash(passwordBytes);
                    }
                    string passwordHash = BitConverter.ToString(hashBytes).Replace("-", string.Empty);

                    command.AddParameter("@Username", username);
                    command.AddParameter("@PasswordHash", passwordHash);
                    command.AddParameter("@Email", email);
                    query = "INSERT INTO Users (Username, PasswordHash, Email) VALUES (@Username, @PasswordHash, @Email)";

                    try
                    {
                        command.ExecuteScalar();
                        Console.WriteLine("Registration successful!");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }
            }
        }
    }

    [TestClass]
    public class RegistrationTests
    {
        [TestMethod]
        public void Signup_WithValidData_RegistrationSuccessful()
        {
            // Arrange
            string username = "testuser";
            string password = "testpassword";
            string email = "test@example.com";

            // Mock the dependencies
            var connectionMock = new Mock<ISqlConnectionWrapper>();
            var commandMock = new Mock<ISqlCommandWrapper>();

            // Set up the mock objects
            connectionMock.Setup(c => c.Open());
            connectionMock.Setup(c => c.CreateCommand()).Returns(commandMock.Object);
            commandMock.Setup(cmd => cmd.AddParameter("@Username", username)).Verifiable();
            commandMock.Setup(cmd => cmd.AddParameter("@PasswordHash", It.IsAny<string>())).Verifiable();
            commandMock.Setup(cmd => cmd.AddParameter("@Email", email)).Verifiable();
            commandMock.Setup(cmd => cmd.ExecuteScalar()).Returns(0); // Assuming username is not taken

            var registration = new Registration
            {
                SqlConnectionWrapperFactory = () => connectionMock.Object
            };

            // Act
            registration.Signup(username, password, email);

            // Assert
            connectionMock.Verify(c => c.Open(), Times.Once);
            connectionMock.Verify(c => c.CreateCommand(), Times.Once);
            commandMock.Verify(cmd => cmd.AddParameter("@Username", username), Times.Exactly(2));
            commandMock.Verify(cmd => cmd.AddParameter("@PasswordHash", It.IsAny<string>()), Times.Once);
            commandMock.Verify(cmd => cmd.AddParameter("@Email", email), Times.Once);
            commandMock.Verify(cmd => cmd.ExecuteScalar(), Times.Exactly(2));
        }
    }
}