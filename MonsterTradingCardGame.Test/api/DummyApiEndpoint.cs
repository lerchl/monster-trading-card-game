using System.Dynamic;
using MonsterTradingCardGame.Api;
using NUnit.Framework;

namespace MonsterTradingCardGame.Test {

    internal class DummyApiEndpoint {

        public static bool Invoked { get; private set; }

        [ApiEndpoint(httpMethod = EHttpMethod.GET, url = "/dummy")]
        public static void TestEndpoint() {
            Invoked = true;
        }
    }
}
