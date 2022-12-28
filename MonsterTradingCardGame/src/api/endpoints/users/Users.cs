using MonsterTradingCardGame.Data;
using MonsterTradingCardGame.Logic;
using MonsterTradingCardGame.Logic.Exceptions;
using MonsterTradingCardGame.Server;

namespace MonsterTradingCardGame.Api.Endpoints.Users {

    internal class Users {

        private const string USERNAME_PATH_PARAM = "username";
        private const string URL = $"^/users/(?'{USERNAME_PATH_PARAM}'{RegexUtils.Username})$";

        private static readonly UserLogic _logic = new();

        // /////////////////////////////////////////////////////////////////////
        // Methods
        // /////////////////////////////////////////////////////////////////////
    
        [ApiEndpoint(HttpMethod = EHttpMethod.GET, Url = URL)]
        public static Response FindByUsername([Bearer]                                Token bearer,
                                              [PathParam(Name = USERNAME_PATH_PARAM)] string username) {
            try {
                return new(HttpCode.OK_200, _logic.GetInfo(bearer, username));
            } catch (ForbiddenException e) {
                return new(HttpCode.FORBIDDEN_403, e.Message);
            } catch (NoResultException e) {
                return new(HttpCode.NOT_FOUND_404, e.Message);
            }
        }

        [ApiEndpoint(HttpMethod = EHttpMethod.PUT, Url = URL)]
        public static Response EditByUsername([Bearer]                                Token  token,
                                              [PathParam(Name = USERNAME_PATH_PARAM)] string username,
                                              [Body]                                  UserVO user) {
            try {
                return new(HttpCode.OK_200, _logic.SetInfo(token, username, user));
            } catch (ForbiddenException e) {
                return new(HttpCode.FORBIDDEN_403, e.Message);
            } catch (NoResultException e) {
                return new(HttpCode.NOT_FOUND_404, e.Message);
            }
        }
    }
}
