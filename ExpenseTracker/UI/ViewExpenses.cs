using ExpenseTracker.DataAccess;
using ExpenseTracker.Filters;
using ExpenseTracker.Model;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.UI
{
    public class ViewExpenses
    {
        private readonly string connectionString;
        private readonly ExpenseDataAccess expenseDataAccess;

        public ViewExpenses(string connectionString)
        {
            this.connectionString = connectionString;
            this.expenseDataAccess = new ExpenseDataAccess(connectionString);
        }

        public void DisplayExpenses(User user)
        {
            List<Expense> expenses = expenseDataAccess.RetrieveExpenses(user);

            if (expenses.Count == 0)
            {
                Console.WriteLine("No expenses found.");
                Console.WriteLine("Press any key to go back to the main menu.");
                Console.ReadKey();

                var expenseTrackerDashboard = new ExpenseTrackerDashboard(connectionString);
                expenseTrackerDashboard.DisplayDashboard(user);
                return;
            }

            Console.Clear();
            Console.WriteLine("Expense List");
            Console.WriteLine("============");

            foreach (Expense expense in expenses)
            {
                Console.WriteLine($"ID: {expense.Id}");
                Console.WriteLine($"Description: {expense.Description}");
                Console.WriteLine($"Amount: {expense.Amount:C}");
                Console.WriteLine($"Date: {expense.Date}");
                Console.WriteLine($"Category: {expense.Category}");
                Console.WriteLine($"Payment Method: {expense.PaymentMethod}");
                Console.WriteLine("-------------------");
            }

            Console.WriteLine();
            Console.WriteLine("Select an option:");
            Console.WriteLine("1. Go to the Filtering Menu");
            Console.WriteLine("2. Go back to the Main Menu");

            int choice;
            bool isValidChoice;

            do
            {
                Console.Write("Enter your choice: ");
                isValidChoice = int.TryParse(Console.ReadLine(), out choice);

                if (!isValidChoice || choice < 1 || choice > 2)
                {
                    Console.WriteLine("Invalid choice. Please try again.");
                }
            } while (!isValidChoice || choice < 1 || choice > 2);

            switch (choice)
            {
                case 1:
                    ShowFilteringMenu(user, expenses);
                    break;
                case 2:
                    var expenseTrackerDashboard = new ExpenseTrackerDashboard(connectionString);
                    expenseTrackerDashboard.DisplayDashboard(user);
                    break;
            }
        }

        private void ShowFilteringMenu(User user, List<Expense> expenses)
        {
            Console.Clear();
            Console.WriteLine("Filtering Menu");
            Console.WriteLine("==============");
            Console.WriteLine("1. Filter by ID");
            Console.WriteLine("2. Filter by Amount Range");
            Console.WriteLine("3. Filter by Date");
            Console.WriteLine("4. Filter by Category");
            Console.WriteLine("5. Filter by Payment Method");
            Console.WriteLine("6. Return to Main Menu");
            Console.WriteLine();

            int choice;
            bool isValidChoice;

            do
            {
                Console.Write("Enter your choice: ");
                isValidChoice = int.TryParse(Console.ReadLine(), out choice);

                if (!isValidChoice || choice < 1 || choice > 6)
                {
                    Console.WriteLine("Invalid choice. Please try again.");
                }
            } while (!isValidChoice || choice < 1 || choice > 6);

            switch (choice)
            {
                case 1:
                    Console.Write("Enter expense ID: ");
                    int expenseId;
                    if (int.TryParse(Console.ReadLine(), out expenseId))
                    {
                        FilterOptions filterOptions = new FilterOptions
                        {
                            FilterById = true,
                            Id = expenseId
                        };

                        ApplyFilteringAndDisplay(expenses, filterOptions, user);
                    }
                    else
                    {
                        Console.WriteLine("Invalid expense ID entered.");
                        ShowFilteringMenu(user, expenses);
                    }
                    break;
                case 2:
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

                            ApplyFilteringAndDisplay(expenses, filterOptions, user);
                        }
                        else
                        {
                            Console.WriteLine("Invalid maximum amount entered.");
                            ShowFilteringMenu(user, expenses);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid minimum amount entered.");
                        ShowFilteringMenu(user, expenses);
                    }
                    break;
                case 3:
                    Console.Write("Enter expense date (yyyy-MM-dd): ");
                    DateTime expenseDate;
                    if (DateTime.TryParse(Console.ReadLine(), out expenseDate))
                    {
                        FilterOptions filterOptions = new FilterOptions
                        {
                            FilterByDate = true,
                            Date = expenseDate
                        };

                        ApplyFilteringAndDisplay(expenses, filterOptions, user);
                    }
                    else
                    {
                        Console.WriteLine("Invalid expense date entered.");
                        ShowFilteringMenu(user, expenses);
                    }
                    break;
                case 4:
                    Console.Write("Enter expense category: ");
                    string expenseCategory = Console.ReadLine();
                    FilterOptions filterOptionsCategory = new FilterOptions
                    {
                        FilterByCategory = true,
                        Category = expenseCategory
                    };

                    ApplyFilteringAndDisplay(expenses, filterOptionsCategory, user);
                    break;
                case 5:
                    Console.Write("Enter payment method: ");
                    string paymentMethod = Console.ReadLine();
                    FilterOptions filterOptionsPaymentMethod = new FilterOptions
                    {
                        FilterByPaymentMethod = true,
                        PaymentMethod = paymentMethod
                    };

                    ApplyFilteringAndDisplay(expenses, filterOptionsPaymentMethod, user);
                    break;
                case 6:
                    var expenseTrackerDashboard = new ExpenseTrackerDashboard(connectionString);
                    expenseTrackerDashboard.DisplayDashboard(user);
                    break;
                default:
                    Console.WriteLine("Invalid choice entered.");
                    ShowFilteringMenu(user, expenses);
                    break;
            }
        }

        private void ApplyFilteringAndDisplay(List<Expense> expenses, FilterOptions filterOptions, User user)
        {
            ExpenseFilter expenseFilter = new ExpenseFilter();
            List<Expense> filteredExpenses;

            if (filterOptions.FilterByAmountRange)
            {
                filteredExpenses = expenseFilter.FilterExpenses(expenses, filterOptions);
            }
            else
            {
                filteredExpenses = expenseFilter.FilterExpenses(expenses, filterOptions);
            }

            if (filteredExpenses.Count == 0)
            {
                Console.WriteLine("No expenses found.");
            }
            else
            {
                Console.WriteLine("Filtered Expense List");
                Console.WriteLine("====================");

                foreach (Expense expense in filteredExpenses)
                {
                    Console.WriteLine($"ID: {expense.Id}");
                    Console.WriteLine($"Description: {expense.Description}");
                    Console.WriteLine($"Amount: {expense.Amount:C}");
                    Console.WriteLine($"Date: {expense.Date}");
                    Console.WriteLine($"Category: {expense.Category}");
                    Console.WriteLine($"Payment Method: {expense.PaymentMethod}");
                    Console.WriteLine("-------------------");
                }
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();

            ShowFilteringMenu(user, expenses);
        }
    }
}