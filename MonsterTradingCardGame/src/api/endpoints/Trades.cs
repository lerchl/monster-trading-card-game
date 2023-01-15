using MonsterTradingCardGame.Data;
using MonsterTradingCardGame.Data.Trade;
using MonsterTradingCardGame.Logic;
using MonsterTradingCardGame.Logic.Exceptions;
using MonsterTradingCardGame.Server;

namespace MonsterTradingCardGame.Api {

    internal class Trades {

        private const string TRADE_ID_PATH_PARAM = "tradeId";
        private const string URL = "^/trades";

        private static readonly TradeLogic _logic = new();

        // /////////////////////////////////////////////////////////////////////
        // Methods
        // /////////////////////////////////////////////////////////////////////

        [ApiEndpoint(HttpMethod = EHttpMethod.GET, Url = "/trades")]
        public static Response GetAllTrades() {
            return new(HttpCode.OK_200, _logic.FindAll());
        }

        [ApiEndpoint(HttpMethod = EHttpMethod.POST, Url = URL + "$")]
        public static Response CreateTrade([Bearer] Token token,
                                           [Body]   Trade trade) {
            try {
                return new(HttpCode.CREATED_201, _logic.Create(token, trade));
            } catch (NoResultException e) {
                return new(HttpCode.NOT_FOUND_404, e.Message);
            } catch (ForbiddenException e) {
                return new(HttpCode.FORBIDDEN_403, e.Message);
            }
        }

        [ApiEndpoint(HttpMethod = EHttpMethod.POST, Url = $"{URL}/(?'{TRADE_ID_PATH_PARAM}'{RegexUtils.GUID})$")]
        public static Response Trade([Bearer]    Token       token,
                                     [PathParam] string      tradeId,
                                     [Body]      GuidWrapper cardId) {
            try {
                _logic.Trade(token, Guid.Parse(tradeId), Guid.Parse(cardId.Id));
                return new(HttpCode.OK_200);
            } catch (NoResultException e) {
                return new(HttpCode.NOT_FOUND_404, e.Message);
            } catch (ForbiddenException e) {
                return new(HttpCode.FORBIDDEN_403, e.Message);
            }
        }

        [ApiEndpoint(HttpMethod = EHttpMethod.DELETE, Url = $"{URL}/(?'{TRADE_ID_PATH_PARAM}'{RegexUtils.GUID})$")]
        public static Response DeleteTrade([Bearer]    Token   token,
                                           [PathParam] string  tradeId) {
            try {
                _logic.Delete(token, Guid.Parse(tradeId));
                return new(HttpCode.NO_CONTENT_204);
            } catch (NoResultException e) {
                return new(HttpCode.NOT_FOUND_404, e.Message);
            } catch (ForbiddenException e) {
                return new(HttpCode.FORBIDDEN_403, e.Message);
            }
        }
    }
}
