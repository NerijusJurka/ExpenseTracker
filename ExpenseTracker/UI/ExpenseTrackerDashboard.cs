using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpenseTracker.Model;

namespace ExpenseTracker.UI
{
    public class ExpenseTrackerDashboard
    {
        private readonly string connectionString;

        public ExpenseTrackerDashboard(string connectionString)
        {
            this.connectionString = connectionString;
        }
        private Expense GetUserExpense(User user)
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

            return new Expense(0, description, amount, date, user.Id, category, paymentMethod);
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
            Console.WriteLine("5. Add Income");
            Console.WriteLine("6. View Incomes");
            Console.WriteLine("7. Delete Incomes");
            Console.WriteLine("8. Logout");

            int choice;
            bool isValidChoice = false;

            do
            {
                Console.Write("Enter your choice: ");
                isValidChoice = int.TryParse(Console.ReadLine(), out choice);

                if (!isValidChoice || choice < 1 || choice > 8)
                {
                    Console.WriteLine("Invalid choice. Please try again.");
                }
            } while (!isValidChoice || choice < 1 || choice > 7);

            var viewExpenses = new ViewExpenses(connectionString);
            var addExpenses = new AddExpenses(connectionString);
            var editExpenses = new EditExpenses(connectionString);
            var deleteExpenses = new DeleteExpenses(connectionString);
            var addIncome = new IncomeManager(connectionString);
            var viewIncome = new ViewIncomes(connectionString);
            var deleteIncome = new DeleteIncome(connectionString);
            var logOut = new Logout();

            switch (choice)
            {
                case 1:
                    viewExpenses.DisplayExpenses(user);
                    break;
                case 2:
                    Expense expense = GetUserExpense(user);
                    addExpenses.AddExpense(user, expense);
                    break;
                case 3:
                    editExpenses.EditExpense(user);
                    break;
                case 4:
                    deleteExpenses.DeleteExpense(user);
                    break;
                case 5:
                    addIncome.AddIncome(user);
                    break;
                case 6:
                    viewIncome.DisplayIncomes(user);
                    break;
                case 7:
                    deleteIncome.DeleteIncomes(user);
                    break;
                case 8:
                    logOut.PerformLogout();
                    break;
            }
        }
    }
}
