using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Data
{
    public abstract class DatabaseAccess
    {
        protected readonly IDbConnection connection;

        public string ConnectionString { get { return connection.ConnectionString; } }

        protected DatabaseAccess(IDbConnection connection)
        {
            this.connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        protected async Task<T> ExecuteScalarAsync<T>(string query, params SqlParameter[] parameters)
        {
            using (SqlCommand command = new SqlCommand(query, (SqlConnection)connection))
            {
                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }

                try
                {
                    await ((SqlConnection)connection).OpenAsync();
                    object result = await command.ExecuteScalarAsync();
                    return (T)Convert.ChangeType(result, typeof(T));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    return default(T);
                }
            }
        }
    }
}
