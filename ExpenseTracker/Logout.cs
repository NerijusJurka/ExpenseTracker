using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker
{
    public class Logout
    {
        public void PerformLogout()
        {
            Console.WriteLine("Logging out...");
            Thread.Sleep(5000);
            Program.DisplayMainMenu();
        }
    }
}
