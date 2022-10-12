using Npgsql;

namespace MonsterTradingCardGame.Data.Player {

    internal class PlayerRepository : Repository<Player> {

        public Player? FindByUsername(string username) {
            string query = @"SELECT * FROM player WHERE username = :username;";
            var result = new NpgsqlCommand(query, _entityManager.connection) {
                Parameters = { new NpgsqlParameter(":username", username) }
            }.ExecuteReader();

            return ConstructEntity(result);
        }
    }
}
