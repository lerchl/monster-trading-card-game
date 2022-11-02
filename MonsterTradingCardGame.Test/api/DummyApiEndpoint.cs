using MonsterTradingCardGame.Api;
using MonsterTradingCardGame.Server;

namespace MonsterTradingCardGame.Test {

    internal class DummyApiEndpoint {

        [ApiEndpoint(HttpMethod = EHttpMethod.GET, Url = "/dummy")]
        public static Response TestEndpoint() {
            return new Response(HttpCode.OK_200, "Test");
        }
    }
}
