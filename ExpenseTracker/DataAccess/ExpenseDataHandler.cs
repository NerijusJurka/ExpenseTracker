using ExpenseTracker.Model;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ExpenseTracker.DataAccess
{
    public class ExpenseDataHandler
    {
        private readonly string connectionString;

        public ExpenseDataHandler(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Expense> RetrieveExpenses(User user)
        {
            var expenses = new List<Expense>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Expenses WHERE UserId = @UserId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", user.Id);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var expense = new Expense(
                                    (int)reader["Id"],
                                    (string)reader["Description"],
                                    (decimal)reader["Amount"],
                                    (DateTime)reader["Date"],
                                    (int)reader["UserId"],
                                    (string)reader["Category"],
                                    (string)reader["PaymentMethod"]
                                );
                                expenses.Add(expense);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }
            }

            return expenses;
        }
        public void SaveExpense(Expense expense)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                if (!IsTableExists(connection, "Expenses"))
                {
                    CreateExpensesTable(connection);
                    Console.WriteLine("Expenses table created successfully.");
                }

                using (SqlCommand command = new SqlCommand("INSERT INTO Expenses (Description, Amount, Date, UserId, Category, PaymentMethod) VALUES (@Description, @Amount, @Date, @UserId, @Category, @PaymentMethod)", connection))
                {
                    command.Parameters.AddWithValue("@Description", expense.Description);
                    command.Parameters.AddWithValue("@Amount", expense.Amount);
                    command.Parameters.AddWithValue("@Date", expense.Date);
                    command.Parameters.AddWithValue("@UserId", expense.UserId);
                    command.Parameters.AddWithValue("@Category", expense.Category);
                    command.Parameters.AddWithValue("@PaymentMethod", expense.PaymentMethod);

                    command.ExecuteNonQuery();
                    Console.WriteLine("Expense saved successfully.");
                }
            }
        }

        private bool IsTableExists(SqlConnection connection, string tableName)
        {
            using (SqlCommand command = new SqlCommand($"SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{tableName}'", connection))
            {
                return (int)command.ExecuteScalar() > 0;
            }
        }

        private void CreateExpensesTable(SqlConnection connection)
        {
            using (SqlCommand command = new SqlCommand("CREATE TABLE Expenses (Id INT PRIMARY KEY IDENTITY, Description NVARCHAR(100) NOT NULL, Amount DECIMAL(18,2) NOT NULL, Date DATE NOT NULL, UserId INT NOT NULL, Category NVARCHAR(50) NOT NULL, PaymentMethod NVARCHAR(50) NOT NULL)", connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }
}
