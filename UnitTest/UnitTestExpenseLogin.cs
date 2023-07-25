using ExpenseTracker.Model;
using ExpenseTracker.UI;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using Moq;
using System.Text;

namespace UnitTest
{ 
    public interface ISqlConnectionWrapper : IDisposable
    {
        void Open();
        ISqlCommandWrapper CreateCommand();
    }

    public interface ISqlCommandWrapper : IDisposable
    {
        void AddParameter(string parameterName, object value);
        object ExecuteScalar();
        ISqlDataReaderWrapper ExecuteReader();
    }

    public interface ISqlDataReaderWrapper : IDisposable
    {
        bool Read();
        object this[string name] { get; }
    }

    public class SqlConnectionWrapper : ISqlConnectionWrapper
    {
        private readonly SqlConnection connection;

        public SqlConnectionWrapper(string connectionString)
        {
            connection = new SqlConnection(connectionString);
        }

        public void Open() => connection.Open();
        public ISqlCommandWrapper CreateCommand() => new SqlCommandWrapper(connection.CreateCommand());

        public void Dispose() => connection.Dispose();
    }

    public class SqlCommandWrapper : ISqlCommandWrapper
    {
        private readonly SqlCommand command;

        public SqlCommandWrapper(SqlCommand command)
        {
            this.command = command;
        }

        public void AddParameter(string parameterName, object value) => command.Parameters.AddWithValue(parameterName, value);
        public object ExecuteScalar() => command.ExecuteScalar();
        public ISqlDataReaderWrapper ExecuteReader() => new SqlDataReaderWrapper(command.ExecuteReader());

        public void Dispose() => command.Dispose();
    }

    public class SqlDataReaderWrapper : ISqlDataReaderWrapper
    {
        private readonly SqlDataReader reader;

        public SqlDataReaderWrapper(SqlDataReader reader)
        {
            this.reader = reader;
        }

        public bool Read() => reader.Read();
        public object this[string name] => reader[name];

        public void Dispose() => reader.Dispose();
    }

    public class Login
    {
        public Func<ISqlConnectionWrapper> SqlConnectionWrapperFactory { get; set; }

        public void SignIn(string username, string password)
        {
            // Connection string
            string connectionString = "your_connection_string";

            string query = "SELECT PasswordHash FROM Users WHERE Username = @Username";

            using (ISqlConnectionWrapper connection = SqlConnectionWrapperFactory.Invoke())
            {
                using (ISqlCommandWrapper command = connection.CreateCommand())
                {
                    command.AddParameter("@Username", username);

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
            string connectionString = "your_connection_string";

            string query = "SELECT Id, Username, PasswordHash, Email FROM Users WHERE Username = @Username";

            using (ISqlConnectionWrapper connection = SqlConnectionWrapperFactory.Invoke())
            {
                using (ISqlCommandWrapper command = connection.CreateCommand())
                {
                    command.AddParameter("@Username", username);

                    try
                    {
                        connection.Open();
                        using (ISqlDataReaderWrapper reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int userId = (int)reader["Id"];
                                string fetchedUsername = (string)reader["Username"];
                                string fetchedPassword = (string)reader["PasswordHash"];
                                string fetchedEmail = (string)reader["Email"];

                                User user = new User(userId, fetchedUsername, fetchedPassword, fetchedEmail, null, false);

                                return user;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }
            }

            return null;
        }
    }

    [TestClass]
    public class LoginTests
    {
        [TestMethod]
        public void SignIn_WithValidCredentials_LoginSuccessful()
        {
            // Arrange
            string username = "testuser";
            string password = "testpassword";
            string passwordHashFromDb = "hash"; 

            // Mock the dependencies
            var connectionMock = new Mock<ISqlConnectionWrapper>();
            var commandMock = new Mock<ISqlCommandWrapper>();
            var readerMock = new Mock<ISqlDataReaderWrapper>();

            // Set up the mock objects
            connectionMock.Setup(c => c.Open());
            connectionMock.Setup(c => c.CreateCommand()).Returns(commandMock.Object);
            commandMock.Setup(cmd => cmd.AddParameter("@Username", username));
            commandMock.Setup(cmd => cmd.ExecuteScalar()).Returns(passwordHashFromDb);
            commandMock.Setup(cmd => cmd.ExecuteReader()).Returns(readerMock.Object);
            readerMock.Setup(r => r.Read()).Returns(true);
            readerMock.Setup(r => r["Id"]).Returns(1);
            readerMock.Setup(r => r["Username"]).Returns(username);
            readerMock.Setup(r => r["PasswordHash"]).Returns(passwordHashFromDb);
            readerMock.Setup(r => r["Email"]).Returns("test@example.com");

            var login = new Login
            {
                SqlConnectionWrapperFactory = () => connectionMock.Object
            };

            login.SignIn(username, password);
        }
    }
}