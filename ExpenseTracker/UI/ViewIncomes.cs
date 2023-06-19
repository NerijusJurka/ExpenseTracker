using ExpenseTracker.Model;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.UI
{
    public class ViewIncomes
    {
        private readonly string connectionString;

        public ViewIncomes(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void DisplayIncomes(User user)
        {
            Console.Clear();
            List<Income> incomes = RetrieveIncomes(user);

            if (incomes.Count == 0)
            {
                Console.WriteLine("No incomes found.");
                return;
            }

            Console.WriteLine("Income List");
            Console.WriteLine("===========");

            foreach (Income income in incomes)
            {
                Console.WriteLine($"ID: {income.Id}");
                Console.WriteLine($"Description: {income.Description}");
                Console.WriteLine($"Amount: {income.Amount:C}");
                Console.WriteLine($"Frequency: {income.Frequency}");
                Console.WriteLine($"Date: {income.Date}");
                Console.WriteLine("-------------------");
            }

            Console.WriteLine("Press any key to go back to the main menu.");
            Console.ReadKey();
            var expenseTrackerDashboard = new ExpenseTrackerDashboard(connectionString);
            expenseTrackerDashboard.DisplayDashboard(user);
        }

        public List<Income> RetrieveIncomes(User user)
        {
            string query = "SELECT Id, Description, Amount, Frequency, Date FROM Incomes WHERE UserId = @UserId";

            List<Income> incomes = new List<Income>();

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
                            int incomeId = (int)reader["Id"];
                            string description = (string)reader["Description"];
                            decimal amount = (decimal)reader["Amount"];
                            string frequencyString = (string)reader["Frequency"];
                            FrequencyType frequency = Enum.Parse<FrequencyType>(frequencyString);
                            DateTime date = (DateTime)reader["Date"];

                            Income income = new Income
                            {
                                Id = incomeId,
                                Description = description,
                                Amount = amount,
                                Frequency = frequency,
                                Date = date,
                                UserId = user.Id
                            };

                            incomes.Add(income);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }
            }

            return incomes;
        }
    }
}
