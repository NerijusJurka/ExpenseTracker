using System.Data;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ExpenseTracker.Data
{
    public interface IDatabaseAccess
    {
        string ConnectionString { get; }
        Task<T> ExecuteScalarAsync<T>(string query, params IDataParameter[] parameters);
        Task<int> ExecuteNonQueryAsync(string commandText, params IDataParameter[] parameters);
        Task<List<T>> ExecuteQueryAsync<T>(string commandText, params IDataParameter[] parameters) where T : class, new();
    }
}
