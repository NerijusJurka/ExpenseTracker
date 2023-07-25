using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Data;

namespace ExpenseTracker.Data
{
    public class ConnectionSettings
    {
        public string ConnectionString { get; set; }
    }

    public class DatabaseManager : IDatabaseAccess
    {
        private readonly IOptions<ConnectionSettings> settings;

        public DatabaseManager(IOptions<ConnectionSettings> settings)
        {
            this.settings = settings;
        }
        public void EnsureDatabaseTableExists()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string checkTableExistsCommandText = @"
                IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND type in (N'U'))
                BEGIN
                    CREATE TABLE [dbo].[Users](
                        [Id] [int] IDENTITY(1,1) NOT NULL,
                        [Username] [nvarchar](50) NOT NULL,
                        [PasswordHash] [nvarchar](128) NOT NULL,
                        [PasswordSalt] [nvarchar](128) NOT NULL,
                        [Email] [nvarchar](50) NOT NULL,
                        [IsActive] [bit] NOT NULL
                    )
                END";

                using (var command = new SqlCommand(checkTableExistsCommandText, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public string ConnectionString => settings.Value.ConnectionString;

        public async Task<T> ExecuteScalarAsync<T>(string commandText, params IDataParameter[] parameters)
        {
            return default(T);
        }

        public async Task<List<T>> ExecuteQueryAsync<T>(string commandText, params IDataParameter[] parameters) where T : class, new()
        {
            return new List<T>();
        }

        public async Task<int> ExecuteNonQueryAsync(string commandText, params IDataParameter[] parameters)
        {
            return await Task.FromResult(0);
        }
        public string GetConnectionString()
        {
            return settings.Value.ConnectionString;
        }
    }
}
