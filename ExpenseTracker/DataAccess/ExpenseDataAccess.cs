using ExpenseTracker.Model;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ExpenseTracker.DataAccess
{
    public class ExpenseDataAccess
    {
        private readonly string connectionString;

        public ExpenseDataAccess(string connectionString)
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
    }
}
