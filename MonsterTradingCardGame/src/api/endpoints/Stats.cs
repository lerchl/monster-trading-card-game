using MonsterTradingCardGame.Logic;
using MonsterTradingCardGame.Server;

namespace MonsterTradingCardGame.Api.Endpoints {

    internal class Stats {

        private const string URL = "/stats";

        private static readonly UserLogic _logic = new();

        // /////////////////////////////////////////////////////////////////////
        // Methods
        // /////////////////////////////////////////////////////////////////////

        [ApiEndpoint(HttpMethod = EHttpMethod.GET, Url = URL)]
        public static Response Get([Bearer] Token token) {
            return new(HttpCode.OK_200, _logic.GetStats(token));
        }
    }
}