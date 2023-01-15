using MonsterTradingCardGame.Api;
using MonsterTradingCardGame.Data.User;
using MonsterTradingCardGame.Logic;
using MonsterTradingCardGame.Logic.Exceptions;
using MonsterTradingCardGame.Server;

namespace Api.Endpoints {

    internal class Authentication {

        private const string URL = "/sessions";

        private static readonly UserLogic _logic = new();

        // /////////////////////////////////////////////////////////////////////
        // Methods
        // /////////////////////////////////////////////////////////////////////

        [ApiEndpoint(HttpMethod = EHttpMethod.POST, Url = URL, RequiresAuthentication = false)]
        public static Response Login([Body] User user) {
            try {
                return new(HttpCode.OK_200, _logic.Login(user));
            } catch (UnauthorizedException e) {
                return new(HttpCode.UNAUTHORIZED_401, e.Message);
            }
        }
    }
}
