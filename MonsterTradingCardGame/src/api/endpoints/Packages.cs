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
                card.PlayerId = null;
                Card? savedCard = _cardRepository.Save(card);
                if (savedCard == null) {
                    return new Response(HttpCode.INTERNAL_SERVER_ERROR_500, "{message: \"could not create card\"}");
                }
            }

            _logger.Info("Package and cards successfully created");
            return new Response(HttpCode.CREATED_201, cards);
        }

        [ApiEndpoint(HttpMethod = EHttpMethod.POST, Url = "/transactions" + URL)]
        public static Response BuyPackages([Bearer] Token token) {
            Player? player = _playerRepository.FindById(token.PlayerId);
            _logger.Info($"Player {player?.Username} is buying a package...");

            if (player?.Money < 5) {
                _logger.Info($"Player {player?.Username} tried to buy a package but did not have enough money");
                return new Response(HttpCode.BAD_REQUEST_400, "{message: \"not enough money\"}");
            }

            List<Guid> packages = _packageRepository.FindAll();

            if (packages.Count == 0) {
                _logger.Info($"Player {player?.Username} tried to buy a package but there are no packages left");
                return new Response(HttpCode.BAD_REQUEST_400, "{message: \"no packages available\"}");
            }

            Guid package = packages[new Random().Next(packages.Count)];
            // TODO: id cannot be null, tell c# to please recognize that
            List<Card?> pulledCards = _cardRepository.PullCards(package, (Guid) player?.id);

            player.Money -= 5;
            _playerRepository.Save(player);
            _logger.Info($"Player {player?.Username} successfully bought a package");
            return new Response(HttpCode.OK_200, pulledCards);
        }
    }
}