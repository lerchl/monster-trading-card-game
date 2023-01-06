using MonsterTradingCardGame.Logic;
using MonsterTradingCardGame.Server;

namespace MonsterTradingCardGame.Api.Endpoints {

    internal class Scoreboard {

        private const string URL = "/scoreboard";

        private static readonly UserLogic _logic = new();

        // /////////////////////////////////////////////////////////////////////
        // Methods
        // /////////////////////////////////////////////////////////////////////

        [ApiEndpoint(HttpMethod = EHttpMethod.GET, Url = URL)]
        public static Response Get() {
            return new(HttpCode.OK_200, _logic.GetStats());
        }
    }
}
