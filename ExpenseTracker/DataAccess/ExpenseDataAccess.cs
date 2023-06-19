using ExpenseTracker.Model;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            string query = "SELECT Id, Description, Amount, Date, Category, PaymentMethod FROM Expenses WHERE UserId = @UserId";

            List<Expense> expenses = new List<Expense>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", user.Id);

                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        while (reader.Read())
                        {
                            int expenseId = (int)reader["Id"];
                            string description = (string)reader["Description"];
                            decimal amount = (decimal)reader["Amount"];
                            DateTime date = (DateTime)reader["Date"];
                            string category = (string)reader["Category"];
                            string paymentMethod = (string)reader["PaymentMethod"];

                            Expense expense = new Expense
                            {
                                Id = expenseId,
                                Description = description,
                                Amount = amount,
                                Date = date,
                                UserId = user.Id,
                                Category = category,
                                PaymentMethod = paymentMethod
                            };

                            expenses.Add(expense);
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
