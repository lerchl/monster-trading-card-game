namespace MonsterTradingCardGame.Api.Endpoints {

    internal class Cards {

        private const string URL = "/cards";

        [ApiEndpoint(HttpMethod = EHttpMethod.GET, Url = URL)]
        public static void FindAllByUser() {
            // TODO: Use CardRepository to find all cards for user of request
        }
    }
}