using MonsterTradingCardGame.Api.Endpoints.Users;
using Npgsql;

namespace MonsterTradingCardGame.Data.User {

    internal class UserRepository : Repository<User> {

        // /////////////////////////////////////////////////////////////////////
        // Methods
        // /////////////////////////////////////////////////////////////////////

        public User FindByUsername(string username) {
            string query = @"SELECT * FROM player WHERE username = :username;";
            var result = new NpgsqlCommand(query, _entityManager.connection) {
                Parameters = { new NpgsqlParameter(":username", username) }
            }.ExecuteReader();

            return ConstructEntity(result);
        }

        public UserStatsVO FindStatsById(Guid id) {
            string query = @"SELECT name, elo, () as wins, () as losses FROM player WHERE id = :id;";
            var result = new NpgsqlCommand(query, _entityManager.connection) {
                Parameters = { new NpgsqlParameter(":id", id) }
            }.ExecuteReader();

            
        }
    }
}
