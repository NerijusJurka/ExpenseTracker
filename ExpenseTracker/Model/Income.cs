using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Model
{
    public class Income
    {
        public int Id { get; }
        public string Description { get; }
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
        public FrequencyType Frequency { get; }
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

        public Income(int id, string description, decimal amount, FrequencyType frequency, DateTime date, int userId)
        {
            Id = id;
            Description = description ?? throw new ArgumentNullException(nameof(description));
            Amount = amount;
            Frequency = frequency;
            Date = date;
            UserId = userId;
        }
        public enum FrequencyType
        {
            Weekly,
            Monthly,
            Annually
        }
    }
}
