using Npgsql;

namespace MonsterTradingCardGame.Data.Deck {

    internal class DeckRepository : Repository<Deck> {

        // /////////////////////////////////////////////////////////////////////
        // Methods
        // /////////////////////////////////////////////////////////////////////

        public Deck? FindByPlayer(Guid playerId) {
            string query = "SELECT * FROM DECK WHERE PLAYER_ID = :playerId";
            var command = new NpgsqlCommand(query, _entityManager.connection) {
                Parameters = {
                    new(":playerId", playerId)
                }
            };
            return ConstructEntity(command.ExecuteReader());
        }

    }
}
