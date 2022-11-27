using Npgsql;

namespace MonsterTradingCardGame.Data.Cards {

    internal class CardRepository : Repository<Card> {

        public List<Card> FindAllByPlayer(Guid player) {
            string query = "SELECT * FROM CARDS WHERE PLAYER_ID = (player)";
            var command = new NpgsqlCommand(query, _entityManager.connection) {
                Parameters = {
                    new("player", player)
                }
            };
            return ConstructEntityList(command.ExecuteReader());
        }

        public List<Card?> PullCards(Guid package, Guid player) {
            return FindAllByPackage(package).Select(card => {
                card.PlayerId = player;
                card.PackageId = null;
                return Save(card);
            }).ToList();
        }

        public List<Card> FindAllByPackage(Guid package) {
            string query = "SELECT * FROM CARDS WHERE PACKAGE_ID = (package)";
            var command = new NpgsqlCommand(query, _entityManager.connection) {
                Parameters = {
                    new("package", package)
                }
            };
            return ConstructEntityList(command.ExecuteReader());
        }
    }
}