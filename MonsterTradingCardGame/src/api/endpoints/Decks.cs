using MonsterTradingCardGame.Data;
using MonsterTradingCardGame.Data.Cards;
using MonsterTradingCardGame.Logic;
using MonsterTradingCardGame.Logic.Exceptions;
using MonsterTradingCardGame.Server;

namespace MonsterTradingCardGame.Api.Endpoints {

    internal class Decks {

        private const string URL = "/decks";

        private static readonly DeckLogic _logic = new();

        // /////////////////////////////////////////////////////////////////////
        // Methods
        // /////////////////////////////////////////////////////////////////////

        [ApiEndpoint(HttpMethod = EHttpMethod.GET, Url = URL)]
        public static Response GetDeck([Bearer]     Token  token,
                                       [QueryParam] string format) {
            Card[] cards;

            try {
                cards = _logic.Get(token);
            } catch (NoContentException e) {
                return new(HttpCode.NO_CONTENT_204, e.Message);
            }

            if ("plain".Equals(format)) {
                return new(HttpCode.OK_200, string.Join("\n", cards.Select(c => c.ToString()).ToArray()), false);
            }

            return new(HttpCode.OK_200, cards);
        }

        [ApiEndpoint(HttpMethod = EHttpMethod.PUT, Url = URL)]
        public static Response SetDeck([Bearer] Token  token,
                                       [Body]   Guid[] cardIds) {
            try {
                return new(HttpCode.OK_200, _logic.Set(token, cardIds));
            } catch (BadRequestException e) {
                return new(HttpCode.BAD_REQUEST_400, e.Message);
            } catch (ForbiddenException e) {
                return new(HttpCode.FORBIDDEN_403, e.Message);
            } catch (NoResultException e) {
                return new(HttpCode.FORBIDDEN_403, e.Message);
            }
        }
    }
}
