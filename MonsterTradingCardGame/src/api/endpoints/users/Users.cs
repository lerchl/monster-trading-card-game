using MonsterTradingCardGame.Data;
using MonsterTradingCardGame.Data.User;
using MonsterTradingCardGame.Logic;
using MonsterTradingCardGame.Logic.Exceptions;
using MonsterTradingCardGame.Server;

namespace MonsterTradingCardGame.Api.Endpoints.Users {

    internal class Users {

        private const string USERNAME_PATH_PARAM = "username";
        private const string URL = $"^/users/(?'{USERNAME_PATH_PARAM}'{RegexUtils.USERNAME})$";

        private static readonly UserLogic _logic = new();

        // /////////////////////////////////////////////////////////////////////
        // Methods
        // /////////////////////////////////////////////////////////////////////

        [ApiEndpoint(HttpMethod = EHttpMethod.POST, Url = "/users", RequiresAuthentication = false)]
        public static Response Register([Body] User user) {
            try {
                _logic.Register(user);
                return new Response(HttpCode.CREATED_201);
            } catch (ConflictException e) {
                return new Response(HttpCode.CONFLICT_409, e.Message);
            }
        }

        [ApiEndpoint(HttpMethod = EHttpMethod.GET, Url = URL)]
        public static Response GetInfo([Bearer]    Token  bearer,
                                       [PathParam] string username) {
            try {
                return new(HttpCode.OK_200, _logic.GetInfo(bearer, username));
            } catch (ForbiddenException e) {
                return new(HttpCode.FORBIDDEN_403, e.Message);
            } catch (NoResultException e) {
                return new(HttpCode.NOT_FOUND_404, e.Message);
            }
        }

        [ApiEndpoint(HttpMethod = EHttpMethod.PUT, Url = URL)]
        public static Response SetInfo([Bearer]    Token      token,
                                       [PathParam] string     username,
                                       [Body]      UserInfoVO userInfoVO) {
            try {
                return new(HttpCode.OK_200, _logic.SetInfo(token, username, userInfoVO));
            } catch (ForbiddenException e) {
                return new(HttpCode.FORBIDDEN_403, e.Message);
            } catch (NoResultException e) {
                return new(HttpCode.NOT_FOUND_404, e.Message);
            }
        }
    }
}
