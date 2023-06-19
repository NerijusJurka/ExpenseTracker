using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker
{
    public class Income
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public FrequencyType Frequency { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; }

        public User User { get; set; }
    }
    public enum FrequencyType
    {
        Weekly,
        Monthly,
        Annually
    }
}
