using System.Net.Sockets;
using MonsterTradingCardGame.Api;

namespace MonsterTradingCardGame.Test {

    internal class DummyApiEndpoint {

        public static bool Invoked { get; private set; }

        [ApiEndpoint(HttpMethod = EHttpMethod.GET, Url = "/dummy")]
        public static void TestEndpoint() {
            Invoked = true;
        }
    }
}
