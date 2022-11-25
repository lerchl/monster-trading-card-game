using MonsterTradingCardGame.Data.Cards;
using MonsterTradingCardGame.Data.Packages;
using MonsterTradingCardGame.Data.Player;
using MonsterTradingCardGame.Server;

namespace MonsterTradingCardGame.Api.Endpoints {

    internal class Packages {
        private const string URL = "/packages";

        private static readonly Logger<Packages> _logger = new();

        private static readonly PackageRepository _packageRepository = new();
        private static readonly CardRepository _cardRepository = new();
        private static readonly PlayerRepository _playerRepository = new();

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
        public static Response BuyPackages([Header(Name = "Authorization")] string bearer) {
            // TODO: logs
            Token? token = SessionHandler.Instance.GetSession(bearer.Split(" ")[1]);
            if (token == null) {
                _logger.Info("User tried to buy packages without being logged in");
                return new Response(HttpCode.UNAUTHORIZED_401, "{message: \"not logged in\"}");
            }

            Player? player = _playerRepository.FindByUsername(token.Username);

            if (player.Money < 5) {
                return new Response(HttpCode.BAD_REQUEST_400, "{message: \"not enough money\"}");
            }

            List<Guid> packages = _packageRepository.FindAll();

            if (packages.Count == 0) {
                return new Response(HttpCode.BAD_REQUEST_400, "{message: \"no packages available\"}");
            }

            Guid package = packages[new Random().Next(packages.Count)];
            // TODO: id cannot be null, tell c# to please recognize that
            List<Card> pulledCards = _cardRepository.PullCards(package, (Guid) player.id);

            player.Money -= 5;
            _playerRepository.Save(player);
            _logger.Info("User successfully bought a package");
            return new Response(HttpCode.OK_200, pulledCards);
        }
    }
}