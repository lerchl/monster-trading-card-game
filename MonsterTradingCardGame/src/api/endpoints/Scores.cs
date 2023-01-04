using MonsterTradingCardGame.Server;

namespace MonsterTradingCardGame.Api.Endpoints {

    internal class Scores {

        private const string URL = "/scoreboard";

        private static readonly UserLogic _logic = new();

        // /////////////////////////////////////////////////////////////////////
        // Methods
        // /////////////////////////////////////////////////////////////////////

        [ApiEndpoint(HttpMethod = EHttpMethod.GET, Url = URL)]
        public static Response Get([Bearer] Token token) {
            return new();
        }
    }
}