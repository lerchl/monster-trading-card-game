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
                    WHERE (status = 2 AND user_1_id = p.id)
                       OR (status = 3 AND user_2_id = p.id)
                ) as wins, (
                    SELECT COUNT(*)
                    FROM battle
                    WHERE (status = 3 AND user_1_id = p.id)
                       OR (status = 2 AND user_2_id = p.id)
                ) as losses
                FROM player p;
            ";
            var result = new NpgsqlCommand(query, _entityManager.connection).ExecuteReader();

            return ConstructList<UserStatsVO>(result);
        }

        public UserStatsVO FindStatsById(Guid id) {
            string query = @"
                SELECT name, elo, (
                    SELECT COUNT(*)
                    FROM battle
                    WHERE (status = 2 AND user_1_id = :id)
                       OR (status = 3 AND user_2_id = :id)
                ) as wins, (
                    SELECT COUNT(*)
                    FROM battle
                    WHERE (status = 3 AND user_1_id = :id)
                       OR (status = 2 AND user_2_id = :id)
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
