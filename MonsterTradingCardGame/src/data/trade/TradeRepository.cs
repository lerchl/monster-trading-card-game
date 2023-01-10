using Npgsql;

namespace MonsterTradingCardGame.Data.Trade {

    /// <summary>
    ///     Repository for trades.
    /// </summary>
    public class TradeRepository : Repository<Trade> {

        public virtual void DeleteByCardId(Guid cardId) {
            string query = @"DELETE FROM trade WHERE card_id = :card_id;";
            new NpgsqlCommand(query, EntityManager.Instance.connection) {
                Parameters = { new NpgsqlParameter(":card_id", cardId) }
            }.ExecuteNonQuery();
        }
    }
}
