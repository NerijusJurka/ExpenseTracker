using ExpenseTracker.DataAccess;
using ExpenseTracker.Model;

namespace ExpenseTracker.UI
{
    public class AddExpenses
    {
        private readonly string connectionString;
        private readonly ExpenseDataHandler _dataHandler;

        public AddExpenses(string connectionString)
        {
            this.connectionString = connectionString;
            this._dataHandler = new ExpenseDataHandler(connectionString);
        }

        public void AddExpense(User user, Expense expense)
        {
            try
            {
                _dataHandler.SaveExpense(expense);
                Console.WriteLine("Expense added successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving expense: {ex.Message}");
            }

            var viewExpenses = new ViewExpenses(connectionString);
            viewExpenses.DisplayExpenses(user);

            Console.WriteLine("Press any key to go back to the main menu.");
            Console.ReadKey();

            var expenseTrackerDashboard = new ExpenseTrackerDashboard(connectionString);
            expenseTrackerDashboard.DisplayDashboard(user);
        }
    }
}
