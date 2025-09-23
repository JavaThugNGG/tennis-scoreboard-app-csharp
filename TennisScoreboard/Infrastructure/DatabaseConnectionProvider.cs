using Microsoft.Data.Sqlite;

namespace TennisScoreboard.Infrastructure
{
    public class DatabaseConnectionProvider : IDisposable
    {
        private readonly string _connectionString;
        private readonly SqliteConnection _connection;
        private readonly ILogger<DatabaseConnectionProvider> _logger;

        public SqliteConnection Connection => _connection;

        public DatabaseConnectionProvider(string connectionString, ILogger<DatabaseConnectionProvider> logger)
        {
            _connectionString = connectionString;
            _connection = new SqliteConnection(_connectionString);
            _logger = logger;
        }

        public SqliteConnection OpenPersistent()
        {
            try
            {
                _connection.Open();
                _logger.LogInformation("Database connection opened successfully");
                return _connection;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to open database connection");
                throw;
            }
        }
        
        public void Dispose()
        {
            try
            {
                _connection?.Dispose();
                _logger.LogInformation("Database connection closed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to close database connection");
            }
        }
    }
}
