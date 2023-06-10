using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker
{
    public class ExpenseTrackerDashboard
    {
        public void DisplayDashboard(User user)
        {
            Console.WriteLine($"Welcome, {user.Username}!");
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
            switch (choice)
            {
                case 1:
                    var viewExpenses = new ViewExpenses();
                    viewExpenses.DisplayExpenses(user);
                    break;
                case 2:
                    var addExpenses = new AddExpenses();
                    addExpenses.AddExpense(user);
                    break;
                case 3:
                    // Call method to edit expense
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
