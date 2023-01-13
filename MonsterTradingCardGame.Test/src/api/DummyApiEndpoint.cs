using MonsterTradingCardGame.Api;
using MonsterTradingCardGame.Data;
using MonsterTradingCardGame.Data.User;
using MonsterTradingCardGame.Server;

namespace MonsterTradingCardGame.Test.Api {

    internal class DummyApiEndpoint {

        private const string USERNAME_PATH_PARAM = "username";

        [ApiEndpoint(HttpMethod = EHttpMethod.GET, Url = $"/dummy/(?'{USERNAME_PATH_PARAM}'{RegexUtils.USERNAME})")]
        public static Response TestEndpoint([Bearer]     Token  token,
                                            [PathParam]  string username,
                                            [QueryParam] string format,
                                            [Body]       User   user) {
            string content = token.UserId + username + format + user.Username;
            return new Response(HttpCode.OK_200, content, false);
        }
    }
}
