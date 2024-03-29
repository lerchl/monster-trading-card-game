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

        public NpgsqlConnection Connection { get; }

        // /////////////////////////////////////////////////////////////////////
        // Constructor
        // /////////////////////////////////////////////////////////////////////

        private EntityManager() {
            string connectionString = "Host=localhost;Username=postgres;Password=gFA4lKnavg4iRsjfCR0m;Database=postgres";
            Connection = new(connectionString);
            Connection.Open();
            _logger.Info("Connected to database");
        }
    }
}
