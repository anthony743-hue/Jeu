usgin Npgsql;
using models;

namespace dao{
    public class DAO{
        public void insertEvents(Game game){
            List<GameEvent> ls = game.events;
        }
    }

    public class ConnectionFactory
    {
        private string host = null!;
        private int port;
        private string database = null!;
        private string user = null!;
        private string password = null!;
        private string url = null!;
        private DbProviderFactory _factory;
        public ConnectionFactory(string host, int port, string database, string user, string password)
        {
            url = $"Host={host};Port={port};Database={database};Username={user};Password={password}";
            _factory = NpgsqlFactory.Instance;
        }
        public DbConnection getConnection()
        {
            if (_factory == null)
                throw new InvalidOperationException("La Factory n'a pas été initialisée. Appelez Initialize().");

            DbConnection connection = _factory.CreateConnection();
            connection.ConnectionString = url;

            return connection;
        }
    }
}