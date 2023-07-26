using ExpenseTracker.DataAccess;
using ExpenseTracker.Filters;
using ExpenseTracker.Model;
using ExpenseTracker.Services;

namespace ExpenseTracker.UI
{
    public class ViewExpenses
    {
        private ExpenseFilterHandler _expenseFilterHandler;
        private readonly string connectionString;
        private readonly ExpenseDataHandler expenseDataAccess;

        public ViewExpenses(string connectionString)
        {
            this.connectionString = connectionString;
            this.expenseDataAccess = new ExpenseDataHandler(connectionString);
            _expenseFilterHandler = new ExpenseFilterHandler(new ExpenseFilter(), ApplyFilteringAndDisplay, ShowFilteringMenu);
        }

        public void DisplayExpenses(User user)
        {
            List<Expense> expenses = expenseDataAccess.RetrieveExpenses(user);

            if (!expenses.Any())
            {
                NotifyNoExpensesAndReturnToMainMenu(user);
                return;
            }

            DisplayExpenseList("Expense List", expenses);

            int choice = GetUserChoice(new[] { "Go to the Filtering Menu", "Go back to the Main Menu" });

            switch (choice)
            {
                case 1:
                    ShowFilteringMenu(user, expenses);
                    break;
                case 2:
                    ReturnToMainMenu(user);
                    break;
            }
        }

        private void ShowFilteringMenu(User user, List<Expense> expenses)
        {
            DisplayHeader("Filtering Menu");

            int choice = GetUserChoice(new[] { "Filter by ID", "Filter by Amount Range", "Filter by Date", "Filter by Category", "Filter by Payment Method", "Return to Main Menu" });

            switch (choice)
            {
                case 1:
                    _expenseFilterHandler.FilterExpensesById(user, expenses);
                    break;
                case 2:
                    _expenseFilterHandler.FilterExpensesByAmountRange(user, expenses);
                    break;
                case 3:
                    _expenseFilterHandler.FilterExpensesByDate(user, expenses);
                    break;
                case 4:
                    _expenseFilterHandler.FilterExpensesByCategory(user, expenses);
                    break;
                case 5:
                    _expenseFilterHandler.FilterExpensesByPaymentMethod(user, expenses);
                    break;
                case 6:
                    ReturnToMainMenu(user);
                    break;
            }
        }

        private void ApplyFilteringAndDisplay(List<Expense> expenses, FilterOptions filterOptions, User user)
        {
            List<Expense> filteredExpenses = new ExpenseFilter().FilterExpenses(expenses, filterOptions);

            if (!filteredExpenses.Any())
            {
                Console.WriteLine("No expenses found.");
            }
            else
            {
                DisplayExpenseList("Filtered Expense List", filteredExpenses);
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();

            ShowFilteringMenu(user, expenses);
        }
        private void DisplayExpenseList(string header, List<Expense> expenses)
        {
            Console.Clear();
            DisplayHeader(header);

            foreach (Expense expense in expenses)
            {
                DisplayExpense(expense);
            }
        }

        private void DisplayExpense(Expense expense)
        {
            Console.WriteLine($"ID: {expense.Id}");
            Console.WriteLine($"Description: {expense.Description}");
            Console.WriteLine($"Amount: {expense.Amount:C}");
            Console.WriteLine($"Date: {expense.Date}");
            Console.WriteLine($"Category: {expense.Category}");
            Console.WriteLine($"Payment Method: {expense.PaymentMethod}");
            Console.WriteLine("-------------------");
        }

        private void DisplayHeader(string header)
        {
            Console.WriteLine($"{header}");
            Console.WriteLine(new string('=', header.Length));
        }

        private void ReturnToMainMenu(User user)
        {
            var expenseTrackerDashboard = new ExpenseTrackerDashboard(connectionString);
            expenseTrackerDashboard.DisplayDashboard(user);
        }

        private void NotifyNoExpensesAndReturnToMainMenu(User user)
        {
            Console.WriteLine("No expenses found.");
            Console.WriteLine("Press any key to go back to the main menu.");
            Console.ReadKey();
            ReturnToMainMenu(user);
        }

        private int GetUserChoice(string[] options)
        {
            for (int i = 0; i < options.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {options[i]}");
            }

            int choice;
            bool isValidChoice;

            do
            {
                Console.Write("Enter your choice: ");
                isValidChoice = int.TryParse(Console.ReadLine(), out choice);

                if (!isValidChoice || choice < 1 || choice > options.Length)
                {
                    Console.WriteLine("Invalid choice. Please try again.");
                }
            } while (!isValidChoice || choice < 1 || choice > options.Length);

            return choice;
        }
        
    }
}