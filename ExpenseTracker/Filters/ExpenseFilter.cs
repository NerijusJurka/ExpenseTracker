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
            List<Expense> filteredExpenses = new List<Expense>();

            foreach (Expense expense in expenses)
            {
                if (IsExpenseMatchingFilters(expense, filterOptions))
                {
                    filteredExpenses.Add(expense);
                }
            }

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
