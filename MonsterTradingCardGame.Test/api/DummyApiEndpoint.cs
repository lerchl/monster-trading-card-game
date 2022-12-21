using MonsterTradingCardGame.Api;
using MonsterTradingCardGame.Data;
using MonsterTradingCardGame.Data.Player;
using MonsterTradingCardGame.Server;

namespace MonsterTradingCardGame.Test {

    internal class DummyApiEndpoint {

        private const string USERNAME_PATH_PARAM = "username";

        [ApiEndpoint(HttpMethod = EHttpMethod.GET, Url = $"/dummy/(?'{USERNAME_PATH_PARAM}'{RegexUtils.Username})$")]
        public static Response TestEndpoint([Bearer]                                Token  token,
                                            [PathParam(Name = USERNAME_PATH_PARAM)] string username,
                                            [QueryParam(Name = "format")]           string format,
                                            [Body]                                  Player player) {
            string content = token.PlayerId + username + format + player.Username;
            return new Response(HttpCode.OK_200, content);
        }
    }
}
