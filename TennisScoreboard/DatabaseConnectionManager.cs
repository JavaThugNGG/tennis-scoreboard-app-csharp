using System.Diagnostics;
using Microsoft.Data.Sqlite;

namespace TennisScoreboard
{
    public class DatabaseConnectionManager : IDisposable
    {
        private readonly string _connectionString;
        private readonly SqliteConnection _connection;

        public DatabaseConnectionManager(string connectionString)
        {
            _connectionString = connectionString;
            _connection = new SqliteConnection(_connectionString);
        }

        public void OpenPersistent()
        {
            _connection.Open();
        }

        public void Dispose()
        {
            try
            {
                if (_connection != null)
                {
                    _connection.Dispose();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка при закрытии соединения: {ex.Message}");//в лог
            }
        }
    }
}
