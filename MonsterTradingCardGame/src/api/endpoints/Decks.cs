using MonsterTradingCardGame.Data.Deck;
using MonsterTradingCardGame.Server;

namespace MonsterTradingCardGame.Api.Endpoints {

    internal class Decks {

        private const string URL = "/decks";

        private static readonly Logger<Decks> _logger = new();

        private static readonly DeckRepository _deckRepository = new();

        // /////////////////////////////////////////////////////////////////////
        // Methods
        // /////////////////////////////////////////////////////////////////////

        public static Response GetDeck([Header(Name = "Authorization")] string bearer) {
            Token? token = SessionHandler.Instance.GetSession(bearer.Split(" ")[1]);
            if (token == null) {
                _logger.Info("Player tried to get deck without being logged in");
                return new(HttpCode.UNAUTHORIZED_401, "{message: \"not logged in\"}");
            }

            Deck? deck = _deckRepository.FindByPlayer(token.PlayerId);

            if (deck == null) {
                return new(HttpCode.NOT_FOUND_404, "{message: \"deck not found\"}");
            }

            return new(HttpCode.OK_200, deck);
        }

        public static Response SetDeck([Header(Name = "Authorization")] string bearer, [Body] Deck deck) {
            Token? token = SessionHandler.Instance.GetSession(bearer.Split(" ")[1]);
            if (token == null) {
                _logger.Info("Player tried to get deck without being logged in");
                return new(HttpCode.UNAUTHORIZED_401, "{message: \"not logged in\"}");
            }

            Deck? savedDeck = _deckRepository.Save(deck);

            if (savedDeck == null) {
                return new(HttpCode.INTERNAL_SERVER_ERROR_500, "{message: \"deck could not be saved\"}");
            }

            return new(HttpCode.CREATED_201, savedDeck);
        }
    }
}