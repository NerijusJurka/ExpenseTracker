using ExpenseTracker.Data;
using ExpenseTracker.Model;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.UI
{
    public class DeleteIncome
    {
        private readonly string connectionString;
        private readonly ViewIncomes viewIncomes;

        public DeleteIncome(string connectionString)
        {
            this.connectionString = connectionString;
            this.viewIncomes = new ViewIncomes(connectionString);
        }

        public void DeleteIncomes(User user)
        {
            Console.Clear();
            Console.WriteLine("Current Incomes");
            Console.WriteLine("===============");

            List<Income> incomes = viewIncomes.RetrieveIncomes(user);

            if (incomes.Count == 0)
            {
                Console.WriteLine("No incomes found.");
            }
            else
            {
                foreach (Income income in incomes)
                {
                    Console.WriteLine($"ID: {income.Id}");
                    Console.WriteLine($"Description: {income.Description}");
                    Console.WriteLine($"Amount: {income.Amount:C}");
                    Console.WriteLine($"Frequency: {income.Frequency}");
                    Console.WriteLine($"Date: {income.Date}");
                    Console.WriteLine("-------------------");
                }
            }

            Console.WriteLine();

            Console.WriteLine("Enter the ID of the income to delete (or 'q' to quit): ");

            while (true)
            {
                string input = Console.ReadLine();

                if (input.ToLower() == "q")
                {
                    Console.WriteLine("Exiting delete income menu.");
                    break;
                }

                if (!int.TryParse(input, out int incomeId))
                {
                    Console.WriteLine("Invalid income ID. Please enter a valid integer value or 'q' to quit.");
                    continue;
                }

                if (IsIncomeExists(incomeId))
                {
                    DeleteIncomeFromDatabase(incomeId);
                    Console.WriteLine("Income deleted successfully.");
                }
                else
                {
                    Console.WriteLine("Income not found.");
                }

                Console.WriteLine();
                Console.WriteLine("Current Incomes");
                Console.WriteLine("===============");

                incomes = viewIncomes.RetrieveIncomes(user);

                if (incomes.Count == 0)
                {
                    Console.WriteLine("No incomes found.");
                }
                else
                {
                    foreach (Income income in incomes)
                    {
                        Console.WriteLine($"ID: {income.Id}");
                        Console.WriteLine($"Description: {income.Description}");
                        Console.WriteLine($"Amount: {income.Amount:C}");
                        Console.WriteLine($"Frequency: {income.Frequency}");
                        Console.WriteLine($"Date: {income.Date}");
                        Console.WriteLine("-------------------");
                    }
                }

                Console.WriteLine();
                Console.WriteLine("Enter the ID of the income to delete (or 'q' to quit): ");
            }

            Console.WriteLine("Press any key to go back to the main menu.");
            Console.ReadKey();
            var expenseTrackerDashboard = new ExpenseTrackerDashboard(connectionString);
            expenseTrackerDashboard.DisplayDashboard(user);
        }

        private bool IsIncomeExists(int incomeId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM Incomes WHERE Id = @IncomeId", connection))
                {
                    command.Parameters.AddWithValue("@IncomeId", incomeId);

                    return (int)command.ExecuteScalar() > 0;
                }
            }
        }

        private void DeleteIncomeFromDatabase(int incomeId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("DELETE FROM Incomes WHERE Id = @IncomeId", connection))
                {
                    command.Parameters.AddWithValue("@IncomeId", incomeId);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
