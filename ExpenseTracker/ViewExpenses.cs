using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker
{
    public class ViewExpenses
    {
        private readonly string connectionString;

        public ViewExpenses(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public void DisplayExpenses(User user)
        {

            List<Expense> expenses = RetrieveExpenses(user);

            Console.WriteLine(user.Id);
            if (expenses.Count == 0)
            {
                Console.WriteLine("No expenses found.");
                return;
            }

            Console.WriteLine("Expense List");
            Console.WriteLine("============");

            foreach (Expense expense in expenses)
            {
                Console.WriteLine($"ID: {expense.Id}");
                Console.WriteLine($"Description: {expense.Description}");
                Console.WriteLine($"Amount: {expense.Amount:C}");
                Console.WriteLine($"Date: {expense.Date}");
                Console.WriteLine("-------------------");
            }
        }
        private List<Expense> RetrieveExpenses(User user)
        {
            string query = "SELECT Id, Description, Amount, Date FROM Expenses WHERE UserId = @UserId";

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

                            Expense expense = new Expense
                            {
                                Id = expenseId,
                                Description = description,
                                Amount = amount,
                                Date = date,
                                UserId = user.Id
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
