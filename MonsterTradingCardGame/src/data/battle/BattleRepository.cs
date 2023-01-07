using Npgsql;

namespace MonsterTradingCardGame.Data.BattleNS {

    /// <summary>
    ///     <see cref="Repository{E}"/> for <see cref="Battle" />s.
    /// </summary>
    internal class BattleRepository : Repository<Battle> {

        public Battle FindOpenBattle() {
            string query = @"SELECT * FROM battle WHERE status = 0 ORDER BY creation LIMIT 1;";
            var result = new NpgsqlCommand(query, EntityManager.Instance.connection).ExecuteReader();
            return ConstructEntity(result);
        }
    }
}
