using MonsterTradingCardGame.Api;

namespace MonsterTradingCardGame.Test.Api {

    internal class NoResponseDummyApiEndpoint {

        [ApiEndpoint(HttpMethod = EHttpMethod.GET, Url = "/dummy", RequiresAuthentication = false)]
        public static void NoResponseTestEndpoint() {
            // noop
        }
    }
}
