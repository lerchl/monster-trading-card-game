using MonsterTradingCardGame.Data.Cards;
using MonsterTradingCardGame.Data.Packages;
using MonsterTradingCardGame.Server;

namespace MonsterTradingCardGame.Api.Endpoints {

    internal class Packages {
        private const string URL = "/packages";

        private static readonly Logger<Packages> _logger = new();

        private static readonly PackageRepository _packageRepository = new();

        // /////////////////////////////////////////////////////////////////////
        // Methods
        // /////////////////////////////////////////////////////////////////////


        [ApiEndpoint(HttpMethod = EHttpMethod.POST, Url = URL)]
        public static Response CreatePackage([Body] Card[] cards) {
            if (cards.Any(card => card.IsPersisted())) {
                return new Response(HttpCode.BAD_REQUEST_400, "{message: \"card already has an id\"}");
            }

            Package? saved = _packageRepository.Save(new Package(cards.ToList()));
            _logger.Info($"Package {saved.id} has been created");
            return new Response(HttpCode.CREATED_201, saved);
        }

        [ApiEndpoint(HttpMethod = EHttpMethod.POST, Url = "/transactions" + URL)]
        public static void BuyPackages() {
            // TODO: Geld des Users prüfen, dann random Package kaufen
        }
    }
}