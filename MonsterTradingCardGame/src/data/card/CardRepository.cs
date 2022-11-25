using Npgsql;

namespace MonsterTradingCardGame.Data.Cards {

    internal class CardRepository : Repository<Card> {

        public List<Card> FindAllByUser() {
            // TODO: Pass user, either id or object and find their cards.
            return new List<Card>();
        }

        public List<Card?> PullCards(Guid package, Guid player) {
            return FindAllByPackage(package).Select(card => {
                card.PlayerId = player;
                card.PackageId = null;
                return Save(card);
            }).ToList();
        }

        public List<Card> FindAllByPackage(Guid package) {
            string query = $"SELECT * FROM CARDS WHERE PACKAGE_ID = (package)";
            var command = new NpgsqlCommand(query, _entityManager.connection) {
                Parameters = {
                    new NpgsqlParameter<Guid>("package", package)
                }
            };
            var result = command.ExecuteReader();

            List<Card> cards = new();
            Card? card;
            while ((card = ConstructEntity(result)) != null) {
                cards.Add(card);
            }

            return cards;
        }
    }
}