using ExpenseTracker.Filters;
using ExpenseTracker.Model;
using ExpenseTracker.UI;

namespace ExpenseTracker.Services
{
    public class ExpenseFilterHandler
    {
        private readonly ExpenseFilter _expenseFilter;
        private readonly Action<List<Expense>, FilterOptions, User> _applyFilteringAndDisplay;
        private readonly Action<User, List<Expense>> _showFilteringMenu;

        public ExpenseFilterHandler(ExpenseFilter expenseFilter, Action<List<Expense>, FilterOptions, User> applyFilteringAndDisplay, Action<User, List<Expense>> showFilteringMenu)
        {
            _expenseFilter = expenseFilter;
            _applyFilteringAndDisplay = applyFilteringAndDisplay;
            _showFilteringMenu = showFilteringMenu;
        }

        public void FilterExpensesById(User user, List<Expense> expenses)
        {
            Console.Write("Enter expense ID: ");
            if (int.TryParse(Console.ReadLine(), out int expenseId))
            {
                FilterOptions filterOptions = new FilterOptions
                {
                    FilterById = true,
                    Id = expenseId
                };

                _applyFilteringAndDisplay(expenses, filterOptions, user);
            }
            else
            {
                Console.WriteLine("Invalid expense ID entered.");
                _showFilteringMenu(user, expenses);
            }
        }
        public void FilterExpensesByAmountRange(User user, List<Expense> expenses)
        {
            Console.Write("Enter minimum amount: ");
            decimal minAmount;
            if (decimal.TryParse(Console.ReadLine(), out minAmount))
            {
                Console.Write("Enter maximum amount: ");
                decimal maxAmount;
                if (decimal.TryParse(Console.ReadLine(), out maxAmount))
                {
                    FilterOptions filterOptions = new FilterOptions
                    {
                        FilterByAmountRange = true,
                        MinAmount = minAmount,
                        MaxAmount = maxAmount
                    };

                    _applyFilteringAndDisplay(expenses, filterOptions, user);
                }
                else
                {
                    Console.WriteLine("Invalid maximum amount entered.");
                    _showFilteringMenu(user, expenses);
                }
            }
            else
            {
                Console.WriteLine("Invalid minimum amount entered.");
                _showFilteringMenu(user, expenses);
            }
        }
        public void FilterExpensesByDate(User user, List<Expense> expenses)
        {
            Console.Write("Enter date (YYYY-MM-DD): ");
            DateTime date;
            if (DateTime.TryParse(Console.ReadLine(), out date))
            {
                FilterOptions filterOptions = new FilterOptions
                {
                    FilterByDate = true,
                    Date = date
                };

                _applyFilteringAndDisplay(expenses, filterOptions, user);
            }
            else
            {
                Console.WriteLine("Invalid date entered.");
                _showFilteringMenu(user, expenses);
            }
        }
        public void FilterExpensesByCategory(User user, List<Expense> expenses)
        {
            Console.Write("Enter expense category: ");
            string category = Console.ReadLine();

            FilterOptions filterOptions = new FilterOptions
            {
                FilterByCategory = true,
                Category = category
            };

            _applyFilteringAndDisplay(expenses, filterOptions, user);
        }
        public void FilterExpensesByPaymentMethod(User user, List<Expense> expenses)
        {
            Console.Write("Enter payment method: ");
            string paymentMethod = Console.ReadLine();

            FilterOptions filterOptions = new FilterOptions
            {
                FilterByPaymentMethod = true,
                PaymentMethod = paymentMethod
            };

            _applyFilteringAndDisplay(expenses, filterOptions, user);
        }
    }
}
