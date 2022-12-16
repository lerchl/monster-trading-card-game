using MonsterTradingCardGame.Data;
using MonsterTradingCardGame.Data.Trade;

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
        public static Response GetAllTrades([Header(Name = "Authorization")] string bearer) {
            // TODO
        }

        [ApiEndpoint(HttpMethod = EHttpMethod.POST, Url = URL + "$")]
        public static Response CreateTrade([Header(Name = "Authorization")] string bearer,
                                           [Body]                           Trade trade) {
            // TODO
        }

        [ApiEndpoint(HttpMethod = EHttpMethod.DELETE, Url = $"{URL}/(?'{TRADE_ID_PATH_PARAM}'{RegexUtils.Guid})$")]
        public static Response DeleteTrade([Header(Name = "Authorization")]        string bearer,
                                           [PathParam(Name = TRADE_ID_PATH_PARAM)] Guid tradeId) {
            // TODO
        }
    }
}