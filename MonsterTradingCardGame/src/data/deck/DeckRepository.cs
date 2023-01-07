using Npgsql;

namespace MonsterTradingCardGame.Data.Deck {

    public class DeckRepository : Repository<Deck> {

        // /////////////////////////////////////////////////////////////////////
        // Methods
        // /////////////////////////////////////////////////////////////////////

        public Deck FindByPlayer(Guid playerId) {
            string query = "SELECT * FROM DECK WHERE PLAYER_ID = :playerId";
            var command = new NpgsqlCommand(query, EntityManager.Instance.connection) {
                Parameters = {
                    new(":playerId", playerId)
                }
            };
            return ConstructEntity(command.ExecuteReader());
        }

        /// <summary>
        ///     Checks if a card is in a player's deck.
        /// </summary>
        /// <param name="playerId">The player's id</param>
        /// <param name="cardId">The card's id</param>
        /// <returns>
        ///     <see langword="true"/> if the card is in the player's deck,
        ///     <see langword="false"/> otherwise
        /// </returns>
        public bool IsCardInDeck(Guid playerId, Guid cardId) {
            string query = "SELECT * FROM DECK WHERE PLAYER_ID = :playerId AND (CARD_1_ID = :cardId OR CARD_2_ID = :cardId OR CARD_3_ID = :cardId OR CARD_4_ID = :cardId)";
            return new NpgsqlCommand(query, EntityManager.Instance.connection) {
                Parameters = {
                    new(":playerId", playerId),
                    new(":cardId", cardId)
                }
            }.ExecuteReader().HasRows;
        }
    }
}
