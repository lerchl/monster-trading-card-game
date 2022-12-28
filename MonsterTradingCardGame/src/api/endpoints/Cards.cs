using MonsterTradingCardGame.Data.Cards;
using MonsterTradingCardGame.Server;

namespace MonsterTradingCardGame.Api.Endpoints {

    internal class Cards {

        private const string URL = "/cards";

        private static readonly CardRepository _cardRepository = new();

        // /////////////////////////////////////////////////////////////////////
        // Methods
        // /////////////////////////////////////////////////////////////////////

        [ApiEndpoint(HttpMethod = EHttpMethod.GET, Url = URL)]
        public static Response FindAllCardsByPlayer([Bearer] Token token) {
            List<Card> cards = _cardRepository.FindAllByPlayer(token.UserId);
            return new(HttpCode.OK_200, cards);
        }
    }
}