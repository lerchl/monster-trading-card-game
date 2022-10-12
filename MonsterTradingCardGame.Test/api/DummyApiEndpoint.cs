using System.Dynamic;
using System.Net.Http;
using System.Net.Sockets;
using MonsterTradingCardGame.Api;
using NUnit.Framework;

namespace MonsterTradingCardGame.Test {

    internal class DummyApiEndpoint {

        public static bool Invoked { get; private set; }

        [ApiEndpoint(httpMethod = EHttpMethod.GET, url = "/dummy")]
        public static void TestEndpoint(Socket client) {
            Invoked = true;
        }
    }
}
