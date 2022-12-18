using MonsterTradingCardGame.Data;
using MonsterTradingCardGame.Data.Trade;
using MonsterTradingCardGame.Server;

namespace MonsterTradingCardGame.Api {

    internal class Trades {

        private const string TRADE_ID_PATH_PARAM = "tradeId";
        private const string URL = "^/trades";

        private static readonly Logger<Trades> _logger = new();
        private static readonly TradeRepository _tradeRepository = new();

        // /////////////////////////////////////////////////////////////////////
        // Methods
        // /////////////////////////////////////////////////////////////////////

        [ApiEndpoint(HttpMethod = EHttpMethod.GET, Url = URL + "$")]
        public static Response GetAllTrades([Bearer] Token token) {
            // TODO
            return SessionHandler.UNAUTHORIZED_RESPONSE;
        }

        [ApiEndpoint(HttpMethod = EHttpMethod.POST, Url = URL + "$")]
        public static Response CreateTrade([Bearer] Token token,
                                           [Body]   Trade trade) {
            return SessionHandler.UNAUTHORIZED_RESPONSE;
            // TODO
        }

        [ApiEndpoint(HttpMethod = EHttpMethod.DELETE, Url = $"{URL}/(?'{TRADE_ID_PATH_PARAM}'{RegexUtils.Guid})$")]
        public static Response DeleteTrade([Bearer]                                Token token,
                                           [PathParam(Name = TRADE_ID_PATH_PARAM)] Guid  tradeId) {
            return SessionHandler.UNAUTHORIZED_RESPONSE;
            // TODO
        }
    }
}