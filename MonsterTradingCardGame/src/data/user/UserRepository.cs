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

        public List<UserStatsVO> FindStats() {
            string query = @"
                SELECT name, elo, (
                    SELECT COUNT(*)
                    FROM battle
                    WHERE winner_id = p.id
                ) as wins, (
                    SELECT COUNT(*)
                    FROM battle
                    WHERE winner_id != p.id AND winner_id IS NOT NULL
                ) as losses
                FROM player p
                ORDER BY elo DESC;
            ";
            var result = new NpgsqlCommand(query, _entityManager.connection).ExecuteReader();

            return ConstructList<UserStatsVO>(result);
        }

        public UserStatsVO FindStatsById(Guid id) {
            string query = @"
                SELECT name, elo, (
                    SELECT COUNT(*)
                    FROM battle
                    WHERE winner_id = :id
                ) as wins, (
                    SELECT COUNT(*)
                    FROM battle
                    WHERE winner_id != :id AND winner_id IS NOT NULL
                ) as losses
                FROM player
                WHERE id = :id;
            ";
            var result = new NpgsqlCommand(query, _entityManager.connection) {
                Parameters = { new NpgsqlParameter(":id", id) }
            }.ExecuteReader();

            return Construct<UserStatsVO>(result);
        }
    }
}
