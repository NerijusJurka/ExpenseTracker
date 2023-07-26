using ExpenseTracker.DataAccess;
using ExpenseTracker.Model;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.UI
{
    public class DeleteExpenses
    {
        private readonly string connectionString;
        private readonly ExpenseDataHandler expenseDataAccess;

        public DeleteExpenses(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public void DeleteExpense(User user)
        {
            Console.Clear();
            var viewExpenses = new ViewExpenses(connectionString);
            List<Expense> expenses = expenseDataAccess.RetrieveExpenses(user);

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

            Console.Write("Enter the ID of the expense you want to delete: ");
            int expenseId;
            while (!int.TryParse(Console.ReadLine(), out expenseId))
            {
                Console.WriteLine("Invalid expense ID. Please enter a valid integer value.");
                Console.Write("Enter the ID of the expense you want to delete: ");
            }

            Expense selectedExpense = expenses.FirstOrDefault(expense => expense.Id == expenseId);
            if (selectedExpense == null)
            {
                Console.WriteLine("Expense not found.");
                return;
            }
            Delete(selectedExpense, user);
        }
        public void Delete(Expense selectedExpense, User user)
        {
            string deleteQuery = "DELETE FROM Expenses WHERE Id = @ExpenseId AND UserId = @UserId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                {
                    command.Parameters.AddWithValue("@ExpenseId", selectedExpense.Id);
                    command.Parameters.AddWithValue("@UserId", user.Id);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            Console.WriteLine("Expense deleted successfully.");
                        }
                        else
                        {
                            Console.WriteLine("Failed to delete the expense. Please try again.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }
            }

            var expenseTrackerDashboard = new ExpenseTrackerDashboard(connectionString);
            expenseTrackerDashboard.DisplayDashboard(user);
        }

    }
}
