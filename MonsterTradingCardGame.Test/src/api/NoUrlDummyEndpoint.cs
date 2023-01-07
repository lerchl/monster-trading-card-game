using MonsterTradingCardGame.Api;
using MonsterTradingCardGame.Server;

namespace MonsterTradingCardGame.Test {

    internal class NoUrlDummyApiEndpoint {

        [ApiEndpoint(HttpMethod = EHttpMethod.GET, RequiresAuthentication = false)]
        public static Response NoUrlTestEndpoint() {
            return new Response(HttpCode.OK_200, "Test");
        }
    }
}
