using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker
{
    public class ViewExpenses
    {
        public void DisplayExpenses(User user)
        {
            List<Expense> expenses = RetrieveExpenses(user);

            if (expenses.Count == 0) 
            {
                Console.WriteLine("No expenses found.");
                return;
            }
            Console.WriteLine("Expense List");
            Console.WriteLine("============");
            foreach (Expense expense in expenses)
            {
                Console.WriteLine($"ID: {expense.Id}");
                Console.WriteLine($"Description: {expense.Description}");
                Console.WriteLine($"Amount: {expense.Amount:C}");
                Console.WriteLine($"Date: {expense.Date}");
                Console.WriteLine("-------------------");
            }
        }
        private List<Expense> RetrieveExpenses(User user)
        {
            return new List<Expense>();
        }
    }
}
