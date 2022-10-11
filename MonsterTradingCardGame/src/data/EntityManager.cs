using Npgsql;

namespace MonsterTradingCardGame.Data {

    internal class EntityManager {

        private static readonly Logger<EntityManager> _logger = new();

        private static EntityManager? _instance;

        // /////////////////////////////////////////////////////////////////////
        // Properties
        // /////////////////////////////////////////////////////////////////////

        public static EntityManager Instance {
            get {
                if (_instance == null) {
                    _instance = new EntityManager();
                }
                return _instance;
            }
        }

        public readonly NpgsqlConnection connection;

        // /////////////////////////////////////////////////////////////////////
        // Constructor
        // /////////////////////////////////////////////////////////////////////

        public EntityManager() {
            string connectionString = "Host=localhost;Username=postgres;Password=gFA4lKnavg4iRsjfCR0m;Database=postgres";
            connection = new(connectionString);
            connection.Open();
            _logger.Info("Connected to database");
        }
    }
}
