using MonsterTradingCardGame.Data;
using MonsterTradingCardGame.Data.Cards;
using MonsterTradingCardGame.Data.Trade;
using MonsterTradingCardGame.Server;

namespace MonsterTradingCardGame.Api {

    internal class Trades {

        private const string TRADE_ID_PATH_PARAM = "tradeId";
        private const string URL = "^/trades";

        private static readonly TradeRepository _tradeRepository = new();
        private static readonly CardRepository _cardRepository = new();

        // /////////////////////////////////////////////////////////////////////
        // Methods
        // /////////////////////////////////////////////////////////////////////

        [ApiEndpoint(HttpMethod = EHttpMethod.GET, Url = URL + "$")]
        public static Response GetAllTrades() {
            return new(HttpCode.OK_200, _tradeRepository.FindAll());
        }

        [ApiEndpoint(HttpMethod = EHttpMethod.POST, Url = URL + "$")]
        public static Response CreateTrade([Bearer] Token token,
                                           [Body]   Trade trade) {
            List<Card> cards = _cardRepository.FindAllByPlayer(token.PlayerId);
            if (!cards.Any(c => trade.CardId.Equals(c.id))) {
                return new(HttpCode.BAD_REQUEST_400, "{ message: \"You can only create trade offers for your own cards\" }");
            }

            trade.PlayerId = token.PlayerId;
            return new(HttpCode.CREATED_201, _tradeRepository.Save(trade));
        }

        [ApiEndpoint(HttpMethod = EHttpMethod.DELETE, Url = $"{URL}/(?'{TRADE_ID_PATH_PARAM}'{RegexUtils.Guid})$")]
        public static Response DeleteTrade([Bearer]                                Token token,
                                           [PathParam(Name = TRADE_ID_PATH_PARAM)] Guid  tradeId) {
            Trade? trade = _tradeRepository.FindById(tradeId);

            if (trade == null) {
                return new(HttpCode.NOT_FOUND_404, "{ message: \"Trade not found\" }");
            } else if (trade.PlayerId != token.PlayerId) {
                return new(HttpCode.FORBIDDEN_403, "{ message: \"You can only delete your own trade offers\" }");
            }

            _tradeRepository.Delete(tradeId);
            return new(HttpCode.NO_CONTENT_204, null);
        }
    }
}
