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
    }
}
