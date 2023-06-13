using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker
{
    public class ExpenseTrackerDashboard
    {
        private readonly string connectionString;

        public ExpenseTrackerDashboard(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public void DisplayDashboard(User user)
        {
            Console.Clear();
            Console.WriteLine($"Welcome, {user.Username}, {user.Id}!");
            Console.WriteLine("Expense Tracker Dashboard");
            Console.WriteLine("=========================");
            Console.WriteLine("1. View Expenses");
            Console.WriteLine("2. Add Expense");
            Console.WriteLine("3. Edit Expense");
            Console.WriteLine("4. Delete Expense");
            Console.WriteLine("5. Logout");
            int choice;
            bool isValidChoice = false;

            do
            {
                Console.Write("Enter your choice: ");
                isValidChoice = int.TryParse(Console.ReadLine(), out choice);

                if (!isValidChoice || choice < 1 || choice > 5)
                {
                    Console.WriteLine("Invalid choice. Please try again.");
                }
            } while (!isValidChoice || choice < 1 || choice > 5);

            var viewExpenses = new ViewExpenses(connectionString);
            var addExpenses = new AddExpenses(connectionString);
            var editExpenses = new EditExpenses(connectionString);

            switch (choice)
            {
                case 1:
                    viewExpenses.DisplayExpenses(user);
                    break;
                case 2:
                    addExpenses.AddExpense(user);
                    break;
                case 3:
                    editExpenses.EditExpense(user);
                    break;
                case 4:
                    // Call method to delete expense
                    break;
                case 5:
                    // Call method to logout
                    break;
            }
        }
    }
}
