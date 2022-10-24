using System.Net.Sockets;
using MonsterTradingCardGame.Data.Packages;

namespace MonsterTradingCardGame.Api.Endpoints {

    internal class Packages {

        private const string URL = "/packages";

        [ApiEndpoint(httpMethod = EHttpMethod.POST, url = URL)]
        public static void CreatePackage(Socket client, Package[] packages) {
            Console.WriteLine(packages);
        }

        [ApiEndpoint(httpMethod = EHttpMethod.POST, url = "/transactions" + URL)]
        public static void BuyPackages() {
            // TODO: Geld des Users prüfen, dann random Package kaufen
        }
    }
}