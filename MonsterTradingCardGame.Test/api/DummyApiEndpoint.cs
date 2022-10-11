using MonsterTradingCardGame.Api;
using NUnit.Framework;

namespace MonsterTradingCardGame.Test {

    internal class DummyApiEndpoint {

        [ApiEndpoint(httpMethod = EHttpMethod.GET, url = "/dummy")]
        public static void TestEndpoint() {
            Assert.Pass();
        }
    }
}
