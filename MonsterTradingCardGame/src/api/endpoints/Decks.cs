using System.Net.Http;
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

        [ApiEndpoint(HttpMethod = EHttpMethod.GET, Url = URL)]
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

        [ApiEndpoint(HttpMethod = EHttpMethod.PUT, Url = URL)]
        public static Response SetDeck([Header(Name = "Authorization")] string bearer, [Body] string[] cardIds) {
            Token? token = SessionHandler.Instance.GetSession(bearer.Split(" ")[1]);
            if (token == null) {
                _logger.Info("Player tried to get deck without being logged in");
                return new(HttpCode.UNAUTHORIZED_401, "{message: \"not logged in\"}");
            }

            // TODO: Array ist hier null, JSON konnte nicht geparsed werden
            if (cardIds.Length != 4) {
                return new(HttpCode.BAD_REQUEST_400, "{message: \"deck must contain exactly 4 cards\"}");
            }

            Deck? savedDeck = _deckRepository.Save(new(token.PlayerId, token.PlayerId,
                    Guid.Parse(cardIds[0]), Guid.Parse(cardIds[1]), Guid.Parse(cardIds[2]), Guid.Parse(cardIds[3])));

            if (savedDeck == null) {
                return new(HttpCode.INTERNAL_SERVER_ERROR_500, "{message: \"deck could not be saved\"}");
            }

            return new(HttpCode.CREATED_201, savedDeck);
        }
    }
}