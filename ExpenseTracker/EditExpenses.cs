using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker
{
    public class EditExpenses
    {
        private readonly string connectionString;

        public EditExpenses(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public void EditExpense(User user)
        {
            Console.Clear();
            var viewExpenses = new ViewExpenses(connectionString);
            List<Expense> expenses = viewExpenses.RetrieveExpenses(user);

            if (expenses.Count == 0)
            {
                Console.WriteLine("No expenses found");
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

            Console.Write("Enter the ID of the expense you want to edit: ");
            int expenseId;
            while (!int.TryParse(Console.ReadLine(), out expenseId))
            {
                Console.WriteLine("Invalid expense ID. Please enter a valid integer value.");
                Console.Write("Enter the ID of the expense you want to edit: ");
            }

            Expense selectedExpense = expenses.FirstOrDefault(expense => expense.Id == expenseId);
            if (selectedExpense == null)
            {
                Console.WriteLine("Expense not found.");
                return;
            }

            PerformEdit(selectedExpense, user);
        }
        private void PerformEdit(Expense expense, User user)
        {
            Console.WriteLine("Enter the new description (or leave blank to keep the existing one): ");
            string newDescription = Console.ReadLine();
            if (!string.IsNullOrEmpty(newDescription))
            {
                expense.Description = newDescription;
            }

            Console.WriteLine("Enter the new amount (or leave blank to keep the existing one): ");
            string newAmountInput = Console.ReadLine();
            if (!string.IsNullOrEmpty(newAmountInput) && decimal.TryParse(newAmountInput, out decimal newAmount))
            {
                expense.Amount = newAmount;
            }

            Console.WriteLine("Enter the new date (YYYY-MM-DD) (or leave blank to keep the existing one): ");
            string newDateInput = Console.ReadLine();
            if (!string.IsNullOrEmpty(newDateInput) && DateTime.TryParse(newDateInput, out DateTime newDate))
            {
                expense.Date = newDate;
            }

            string updateQuery = "UPDATE Expenses SET Description = @Description, Amount = @Amount, Date = @Date WHERE Id = @ExpenseId AND UserId = @UserId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@Description", expense.Description);
                    command.Parameters.AddWithValue("@Amount", expense.Amount);
                    command.Parameters.AddWithValue("@Date", expense.Date);
                    command.Parameters.AddWithValue("@ExpenseId", expense.Id);
                    command.Parameters.AddWithValue("@UserId", user.Id);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            Console.WriteLine("Expense edited successfully.");
                        }
                        else
                        {
                            Console.WriteLine("Failed to edit the expense. Please try again.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }
            }

            Console.WriteLine("Expense edited successfully.");
            Console.WriteLine("Redirecting to the main menu...");
            Thread.Sleep(5000);

            var expenseTrackerDashboard = new ExpenseTrackerDashboard(connectionString);
            expenseTrackerDashboard.DisplayDashboard(user);
        }
    }
}
