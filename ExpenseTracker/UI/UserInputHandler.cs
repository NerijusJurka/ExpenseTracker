﻿using ExpenseTracker.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.UI
{
    public class UserInputHandler
    {
        public Expense GetExpenseFromUser(User user)
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
    }
}
