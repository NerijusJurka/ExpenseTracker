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

            Console.WriteLine("Enter expense category: ");
            string category = Console.ReadLine();

            Console.WriteLine("Enter payment method: ");
            string paymentMethod = Console.ReadLine();

            Expense expense = new Expense
            {
                Description = description,
                Amount = amount,
                Date = date,
                UserId = user.Id,
                Category = category,
                PaymentMethod = paymentMethod
            };

            try
            {
                SaveExpense(expense);
                Console.WriteLine("Expense added successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving expense: {ex.Message}");
            }

            Console.WriteLine("Expense added successfully.");

            // Display the updated list of expenses after saving
            var viewExpenses = new ViewExpenses(connectionString);
            viewExpenses.DisplayExpenses(user);

            Console.WriteLine("Press any key to go back to the main menu.");
            Console.ReadKey();
            var expenseTrackerDashboard = new ExpenseTrackerDashboard(connectionString);
            expenseTrackerDashboard.DisplayDashboard(user);
        }
        private void SaveExpense(Expense expense)
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
