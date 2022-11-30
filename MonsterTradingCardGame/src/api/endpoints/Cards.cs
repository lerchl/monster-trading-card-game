using MonsterTradingCardGame.Data.Cards;
using MonsterTradingCardGame.Server;

namespace MonsterTradingCardGame.Api.Endpoints {

    internal class Cards {

        private const string URL = "/cards";

        private static readonly Logger<Cards> _logger = new();

        private static readonly CardRepository _cardRepository = new();

        [ApiEndpoint(HttpMethod = EHttpMethod.GET, Url = URL)]
        public static Response FindAllCardsByPlayer([Header(Name = "Authorization")] string bearer) {
            Token? token = SessionHandler.Instance.GetSession(bearer.Split(" ")[1]);
            if (token == null) {
                _logger.Info("Player tried to get all cards without being logged in");
                return new(HttpCode.UNAUTHORIZED_401, "{message: \"not logged in\"}");
            }

            List<Card> cards = _cardRepository.FindAllByPlayer(token.PlayerId);
            return new(HttpCode.OK_200, cards);
        }
    }
}