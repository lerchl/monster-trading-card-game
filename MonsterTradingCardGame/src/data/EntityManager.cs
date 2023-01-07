using Npgsql;

namespace MonsterTradingCardGame.Data {

    public class EntityManager {

        private static readonly Logger<EntityManager> _logger = new();

        private static EntityManager? _instance;

        // /////////////////////////////////////////////////////////////////////
        // Properties
        // /////////////////////////////////////////////////////////////////////

        public static EntityManager Instance {
            get {
                _instance ??= new EntityManager();
                return _instance;
            }
        }

        // TODO: make a property
        public readonly NpgsqlConnection connection;

        // /////////////////////////////////////////////////////////////////////
        // Constructor
        // /////////////////////////////////////////////////////////////////////

        private EntityManager() {
            string connectionString = "Host=localhost;Username=postgres;Password=Y68277k9PGypEcYuHjiu;Database=postgres";
            connection = new(connectionString);
            connection.Open();
            _logger.Info("Connected to database");
        }
    }
}
