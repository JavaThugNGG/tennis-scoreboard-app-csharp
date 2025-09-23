using Microsoft.Data.Sqlite;

namespace TennisScoreboard
{
    public class DatabaseConnectionManager : IDisposable
    {
        private readonly string _connectionString;
        private readonly SqliteConnection _connection;
        private readonly ILogger<DatabaseConnectionManager> _logger;

        public SqliteConnection Connection => _connection;

        public DatabaseConnectionManager(string connectionString, ILogger<DatabaseConnectionManager> logger)
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
