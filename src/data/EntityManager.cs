using Npgsql;

namespace data {

    internal class EntityManager {

        private static readonly Logger<EntityManager> _logger = new();

        // /////////////////////////////////////////////////////////////////////
        // Properties
        // /////////////////////////////////////////////////////////////////////

        public static EntityManager Instance {
            get {
                if (Instance == null) {
                    Instance = new EntityManager();
                }
                return Instance;
            } private set {
                Instance = value;
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
