using MonsterTradingCardGame.Data.Cards;
using MonsterTradingCardGame.Server;

namespace MonsterTradingCardGame.Api.Endpoints {

    internal class Packages {
        private const string URL = "/packages";

        private static readonly Logger<Packages> _logger = new();

        private static readonly CardRepository _cardRepository = new();

        // /////////////////////////////////////////////////////////////////////
        // Methods
        // /////////////////////////////////////////////////////////////////////


        [ApiEndpoint(HttpMethod = EHttpMethod.POST, Url = URL)]
        public static Response CreatePackage([Body] Card[] cards) {
            if (cards.Any(card => card.IsPersisted())) {
                return new Response(HttpCode.BAD_REQUEST_400, "{message: \"card already has an id\"}");
            }

            Guid packageId = Guid.NewGuid();
            foreach (Card card in cards) {
                card.PackageId = packageId;
                Card? savedCard = _cardRepository.Save(card);
                if (savedCard == null) {
                    return new Response(HttpCode.INTERNAL_SERVER_ERROR_500, "{message: \"could not create card\"}");
                }
            }

            _logger.Info("Package and cards successfully created");
            return new Response(HttpCode.CREATED_201, cards);
        }

        [ApiEndpoint(HttpMethod = EHttpMethod.POST, Url = "/transactions" + URL)]
        public static void BuyPackages() {
            // TODO: Geld des Users prüfen, dann random Package kaufen
        }
    }
}