using MonsterTradingCardGame.Api.Endpoints.Users;
using Npgsql;

namespace MonsterTradingCardGame.Data.User {

    public class UserRepository : Repository<User> {

        // /////////////////////////////////////////////////////////////////////
        // Methods
        // /////////////////////////////////////////////////////////////////////

        public virtual User FindByUsername(string username) {
            string query = @"SELECT * FROM player WHERE username = :username;";
            var result = new NpgsqlCommand(query, EntityManager.Instance.connection) {
                Parameters = { new NpgsqlParameter(":username", username) }
            }.ExecuteReader();

            return ConstructEntity(result);
        }

        public virtual List<UserStatsVO> FindStats() {
            string query = @"
                SELECT name, elo, (
                    SELECT COUNT(*)
                    FROM battle
                    WHERE winner_id = p.id
                ) as wins, (
                    SELECT COUNT(*)
                    FROM battle
                    WHERE (player_1_id = p.id OR player_2_id = p.id)
                            AND winner_id != p.id AND winner_id IS NOT NULL
                ) as losses, (
                    SELECT COUNT(*)
                    FROM battle
                    WHERE (player_1_id = p.id OR player_2_id = p.id)
                            AND winner_id IS NULL
                ) as draws
                FROM player p
                ORDER BY elo DESC;
            ";
            var result = new NpgsqlCommand(query, EntityManager.Instance.connection).ExecuteReader();

            return ConstructList<UserStatsVO>(result);
        }

        public virtual UserStatsVO FindStatsById(Guid id) {
            string query = @"
                SELECT name, elo, (
                    SELECT COUNT(*)
                    FROM battle
                    WHERE winner_id = :id
                ) as wins, (
                    SELECT COUNT(*)
                    FROM battle
                    WHERE (player_1_id = :id OR player_2_id = :id)
                            AND winner_id != :id AND winner_id IS NOT NULL
                ) as losses, (
                    SELECT COUNT(*)
                    FROM battle
                    WHERE (player_1_id = :id OR player_2_id = :id)
                            AND winner_id IS NULL
                ) as draws
                FROM player
                WHERE id = :id;
            ";
            var result = new NpgsqlCommand(query, EntityManager.Instance.connection) {
                Parameters = { new NpgsqlParameter(":id", id) }
            }.ExecuteReader();

            return Construct<UserStatsVO>(result);
        }
    }
}
