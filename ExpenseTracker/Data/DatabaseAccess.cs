using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Data
{
    public abstract class DatabaseAccess
    {
        protected readonly string connectionString;

        protected DatabaseAccess(string connectionString)
        {
            this.connectionString = connectionString;
        }

        protected T ExecuteScalar<T>(string query, SqlParameter parameter)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (parameter != null)
                    {
                        command.Parameters.Add(parameter);
                    }

                    try
                    {
                        connection.Open();
                        return (T)command.ExecuteScalar();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                }
            }

            return default(T);
        }
    }
}
