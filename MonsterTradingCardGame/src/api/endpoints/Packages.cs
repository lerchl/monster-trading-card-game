namespace MonsterTradingCardGame.Api.Endpoints {

    internal class Packages {

        private const string URL = "/packages";

        [ApiEndpoint(httpMethod = EHttpMethod.POST, url = URL)]
        public static void CreatePackage() {
            // TODO: Liste bzw Array an (5) Karten übergeben
        }

        [ApiEndpoint(httpMethod = EHttpMethod.POST, url = "/transactions" + URL)]
        public static void BuyPackages() {
            // TODO: Geld des Users prüfen, dann random Package kaufen
        }
    }
}