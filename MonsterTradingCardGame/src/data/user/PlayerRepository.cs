using Npgsql;

namespace MonsterTradingCardGame.Data.Player {

    internal class PlayerRepository : Repository<Player> {

        public Player? FindByUsername(string username) {
            string query = @"SELECT * FROM users WHERE username = $1;";
            var result = new NpgsqlCommand(query, _entityManager.connection) {
                Parameters = {
                    new NpgsqlParameter("$1", username)
                }
            }.ExecuteScalar();
            // TODO: Use result to create User object
            return new("test", "test");
        }
    }
}