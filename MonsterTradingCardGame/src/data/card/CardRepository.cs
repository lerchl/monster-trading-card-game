using Npgsql;

namespace MonsterTradingCardGame.Data.Cards {

    public class CardRepository : Repository<Card> {

        public List<Card> FindAllByPlayer(Guid player) {
            string query = "SELECT * FROM CARD WHERE PLAYER_ID = :playerId";
            var command = new NpgsqlCommand(query, EntityManager.Instance.connection) {
                Parameters = {
                    new(":playerId", player)
                }
            };
            return ConstructEntityList(command.ExecuteReader());
        }

        public Card[] PullCards(Guid package, Guid player) {
            return FindAllByPackage(package).Select(card => {
                card.UserId = player;
                card.PackageId = null;
                return Save(card);
            }).ToArray();
        }

        public List<Card> FindAllByPackage(Guid package) {
            string query = @"SELECT * FROM CARD WHERE PACKAGE_ID = :package";
            var command = new NpgsqlCommand(query, EntityManager.Instance.connection) {
                Parameters = {
                    new(":package", package)
                }
            };
            return ConstructEntityList(command.ExecuteReader());
        }
    }
}