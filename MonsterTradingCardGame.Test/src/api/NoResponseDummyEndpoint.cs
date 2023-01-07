using MonsterTradingCardGame.Api;

namespace MonsterTradingCardGame.Test {

    internal class NoResponseDummyApiEndpoint {

        [ApiEndpoint(HttpMethod = EHttpMethod.GET, Url = "/dummy", RequiresAuthentication = false)]
        public static void NoResponseTestEndpoint() {
            // noop
        }
    }
}
