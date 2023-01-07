using MonsterTradingCardGame.Data;
using MonsterTradingCardGame.Logic.BattleNS;
using MonsterTradingCardGame.Server;

namespace MonsterTradingCardGame.Api.Endpoints {

    internal class Battles {

        private const string URL = "/battles";

        // /////////////////////////////////////////////////////////////////////
        // Methods
        // /////////////////////////////////////////////////////////////////////

        [ApiEndpoint(HttpMethod = EHttpMethod.POST, Url = URL)]
        public static Response Battle([Bearer] Token token) {
            try {
                return new(HttpCode.OK_200, BattleLogic.Battle(token), false);
            } catch (NoResultException e) {
                return new(HttpCode.FORBIDDEN_403, e.Message);
            }
        }
    }
}
