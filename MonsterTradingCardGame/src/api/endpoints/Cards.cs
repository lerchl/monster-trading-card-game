namespace MonsterTradingCardGame.Api.Endpoints {

    internal class Cards {

        private const string URL = "/cards";

        [ApiEndpoint(httpMethod = EHttpMethod.GET, url = URL)]
        public static void FindAllByUser() {
            // TODO: Use CardRepository to find all cards for user of request
        }
    }
}