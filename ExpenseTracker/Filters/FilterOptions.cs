using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Filters
{
    public class FilterOptions
    {
        public bool FilterById { get; set; }
        public int Id { get; set; }

        public bool FilterByAmount { get; set; }
        public decimal Amount { get; set; }
        public bool FilterByAmountRange { get; set; }
        public decimal MinAmount { get; set; }
        public decimal MaxAmount { get; set; }

        public bool FilterByDate { get; set; }
        public DateTime Date { get; set; }
        public bool FilterByDateRange { get; set; }
        public DateTime MinDate { get; set; }
        public DateTime MaxDate { get; set; }

        public bool FilterByCategory { get; set; }
        public string Category { get; set; }

        public bool FilterByPaymentMethod { get; set; }
        public string PaymentMethod { get; set; } 
    }
}
