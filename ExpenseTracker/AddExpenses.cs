using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker
{
    public class AddExpenses
    {
        private readonly string connectionString;

        public AddExpenses(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public void AddExpense(User user)
        {
            Console.WriteLine("Enter expense description: ");
            string description = Console.ReadLine();

            Console.WriteLine("Enter expense amount: ");
            decimal amount;
            while (!decimal.TryParse(Console.ReadLine(), out amount))
            {
                Console.WriteLine("Invalid amount. Please enter a valid decimal value.");
                Console.Write("Enter expense amount: ");
            }

            Console.WriteLine("Enter expense date (YYYY-MM-DD): ");
            DateTime date;
            while (!DateTime.TryParse(Console.ReadLine(), out date))
            {
                Console.WriteLine("Invalid date format. Please enter a valid date in the format YYYY-MM-DD.");
                Console.Write("Enter expense date (YYYY-MM-DD): ");
            }

            Expense expense = new Expense
            {
                Description = description,
                Amount = amount,
                Date = date,
                UserId = user.Id
            };

            SaveExpense(expense);

            Console.WriteLine("Expense added successfully.");

            // Display the updated list of expenses after saving
            var viewExpenses = new ViewExpenses(connectionString);
            viewExpenses.DisplayExpenses(user);
        }
        private void SaveExpense(Expense expense)
        {
            string tableName = "Expenses";
            string queryCheckTableExists = $"SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{tableName}'";
            string queryCreateTable = $"CREATE TABLE {tableName} (Id INT PRIMARY KEY IDENTITY, Description NVARCHAR(100) NOT NULL, Amount DECIMAL(18,2) NOT NULL, Date DATE NOT NULL, UserId INT NOT NULL)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(queryCheckTableExists, connection))
                {
                    try
                    {
                        connection.Open();
                        int tableCount = (int)command.ExecuteScalar();
                        if (tableCount == 0)
                        {
                            command.CommandText = queryCreateTable;
                            command.ExecuteNonQuery();
                            
                            Console.WriteLine("Table created successfully.");
                        }

                        command.CommandText = "INSERT INTO Expenses (Description, Amount, Date, UserId) VALUES (@Description, @Amount, @Date, @UserId)";
                        command.Parameters.AddWithValue("@Description", expense.Description);
                        command.Parameters.AddWithValue("@Amount", expense.Amount);
                        command.Parameters.AddWithValue("@Date", expense.Date);
                        command.Parameters.AddWithValue("@UserId", expense.UserId);
                        command.ExecuteNonQuery();
                        Console.WriteLine("Expense saved successfully {0}." + expense.UserId);
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
