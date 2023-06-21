using ExpenseTracker.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Filters
{
    public class ExpenseFilter
    {
        public List<Expense> FilterExpenses(List<Expense> expenses, FilterOptions filterOptions)
        {
            List<Func<Expense, bool>> filters = new List<Func<Expense, bool>>();

            if (filterOptions.FilterById)
            {
                filters.Add(expense => expense.Id == filterOptions.Id);
            }

            if (filterOptions.FilterByAmount)
            {
                filters.Add(expense => expense.Amount == filterOptions.Amount);
            }

            if (filterOptions.FilterByAmountRange)
            {
                filters.Add(expense => expense.Amount >= filterOptions.MinAmount && expense.Amount <= filterOptions.MaxAmount);
            }

            if (filterOptions.FilterByDate)
            {
                filters.Add(expense => expense.Date == filterOptions.Date);
            }

            if (filterOptions.FilterByDateRange)
            {
                filters.Add(expense => expense.Date >= filterOptions.MinDate && expense.Date <= filterOptions.MaxDate);
            }

            if (filterOptions.FilterByCategory)
            {
                filters.Add(expense => expense.Category == filterOptions.Category);
            }

            if (filterOptions.FilterByPaymentMethod)
            {
                filters.Add(expense => expense.PaymentMethod == filterOptions.PaymentMethod);
            }

            if (filterOptions.FilterByCategory)
            {
                filters.Add(expense => expense.Category.Equals(filterOptions.Category, StringComparison.OrdinalIgnoreCase));
            }

            if (filterOptions.FilterByPaymentMethod)
            {
                filters.Add(expense => expense.PaymentMethod.Equals(filterOptions.PaymentMethod, StringComparison.OrdinalIgnoreCase));
            }

            if (filterOptions.FilterByAmount)
            {
                filters.Add(expense => expense.Amount == filterOptions.Amount);
            }
            List<Expense> filteredExpenses = expenses.Where(expense => filters.All(filter => filter(expense))).ToList();

            return filteredExpenses;
        }

        private bool IsExpenseMatchingFilters(Expense expense, FilterOptions filterOptions)
        {
            if (filterOptions.FilterById && expense.Id != filterOptions.Id)
                return false;

            if (filterOptions.FilterByAmount && expense.Amount != filterOptions.Amount)
                return false;

            if (filterOptions.FilterByDate && expense.Date != filterOptions.Date)
                return false;

            if (filterOptions.FilterByCategory && expense.Category != filterOptions.Category)
                return false;

            if (filterOptions.FilterByPaymentMethod && expense.PaymentMethod != filterOptions.PaymentMethod)
                return false;

            return true;
        }
    }
}
