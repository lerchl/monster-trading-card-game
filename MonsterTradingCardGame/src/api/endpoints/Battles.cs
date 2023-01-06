using MonsterTradingCardGame.Data;
using MonsterTradingCardGame.Logic.BattleNS;
using MonsterTradingCardGame.Server;

namespace MonsterTradingCardGame.Api.Endpoints {

    internal class Battles {

        private const string URL = "/battles";

        private static readonly BattleLogic _logic = new();

        // /////////////////////////////////////////////////////////////////////
        // Methods
        // /////////////////////////////////////////////////////////////////////

        [ApiEndpoint(HttpMethod = EHttpMethod.POST, Url = URL)]
        public static Response Battle([Bearer] Token token) {
            try {
                return new(HttpCode.OK_200, _logic.Battle(token));
            } catch (NoResultException e) {
                return new(HttpCode.FORBIDDEN_403, e.Message);
            }
        }
    }
}