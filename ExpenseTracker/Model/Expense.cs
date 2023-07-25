using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Model
{
    public class Expense
    {
        public int Id { get; }
        public string Description { get; set; }
        private decimal amount;
        public decimal Amount
        {
            get { return amount; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Amount must be positive");
                amount = value;
            }
        }
        private DateTime date;
        public DateTime Date
        {
            get { return date; }
            set
            {
                if (value > DateTime.Now)
                    throw new ArgumentException("Date cannot be in the future");
                date = value;
            }
        }
        public int UserId { get; }
        public string Category { get; set; }
        public string PaymentMethod { get; set; }

        public Expense(int id, string description, decimal amount, DateTime date, int userId, string category, string paymentMethod)
        {
            Id = id;
            Description = description ?? throw new ArgumentNullException(nameof(description));
            Amount = amount;
            Date = date;
            UserId = userId;
            Category = category ?? throw new ArgumentNullException(nameof(category));
            PaymentMethod = paymentMethod ?? throw new ArgumentNullException(nameof(paymentMethod));
        }
    }
}
