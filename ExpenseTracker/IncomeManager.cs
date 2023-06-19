using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker
{
    public class IncomeManager
    {
        private readonly string connectionString;

        public IncomeManager(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public void AddIncome(User user)
        {
            Console.WriteLine("Enter income description: ");
            string description = Console.ReadLine();

            Console.WriteLine("Enter income amount: ");
            decimal amount;
            while (!decimal.TryParse(Console.ReadLine(), out amount))
            {
                Console.WriteLine("Invalid amount. Please enter a valid decimal value.");
                Console.Write("Enter income amount: ");
            }

            Console.WriteLine("Enter income frequency (Weekly/Monthly/Annually): ");
            string frequencyInput = Console.ReadLine();
            FrequencyType frequency;
            while (!Enum.TryParse(frequencyInput, out frequency))
            {
                Console.WriteLine("Invalid frequency. Please enter a valid frequency (Weekly/Monthly/Annually).");
                Console.Write("Enter income frequency: ");
                frequencyInput = Console.ReadLine();
            }

            Console.WriteLine("Enter income date (YYYY-MM-DD): ");
            DateTime date;
            while (!DateTime.TryParse(Console.ReadLine(), out date))
            {
                Console.WriteLine("Invalid date format. Please enter a valid date in the format YYYY-MM-DD.");
                Console.Write("Enter income date (YYYY-MM-DD): ");
            }

            Income income = new Income
            {
                Description = description,
                Amount = amount,
                Frequency = frequency,
                Date = date,
                UserId = user.Id
            };

            SaveIncome(income);

            Console.WriteLine("Income added successfully.");

            var viewIncomes = new ViewIncomes(connectionString);
            viewIncomes.DisplayIncomes(user);

            Console.WriteLine("Press any key to go back to the main menu.");
            Console.ReadKey();
            var expenseTrackerDashboard = new ExpenseTrackerDashboard(connectionString);
            expenseTrackerDashboard.DisplayDashboard(user);
        }

        private void SaveIncome(Income income)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                if (!IsTableExists(connection, "Incomes"))
                {
                    CreateIncomesTable(connection);
                    Console.WriteLine("Incomes table created successfully.");
                }

                using (SqlCommand command = new SqlCommand("INSERT INTO Incomes (Description, Amount, Frequency, Date, UserId) VALUES (@Description, @Amount, @Frequency, @Date, @UserId)", connection))
                {
                    command.Parameters.AddWithValue("@Description", income.Description);
                    command.Parameters.AddWithValue("@Amount", income.Amount);
                    command.Parameters.AddWithValue("@Frequency", income.Frequency.ToString());
                    command.Parameters.AddWithValue("@Date", income.Date);
                    command.Parameters.AddWithValue("@UserId", income.UserId);

                    command.ExecuteNonQuery();
                    Console.WriteLine("Income saved successfully.");
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

        private void CreateIncomesTable(SqlConnection connection)
        {
            using (SqlCommand command = new SqlCommand("CREATE TABLE Incomes (Id INT PRIMARY KEY IDENTITY, Description NVARCHAR(100) NOT NULL, Amount DECIMAL(18,2) NOT NULL, Frequency NVARCHAR(20) NOT NULL, Date DATE NOT NULL, UserId INT NOT NULL)", connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }
}
