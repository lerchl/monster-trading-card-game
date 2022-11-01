using MonsterTradingCardGame.Api;
using Newtonsoft.Json.Linq;

namespace MonsterTradingCardGame.Server {

    public class HttpRequest {

        public readonly Destination destination;
        public readonly JToken? data;

        // /////////////////////////////////////////////////////////////////////
        // Constructor
        // /////////////////////////////////////////////////////////////////////

        public HttpRequest(Destination destination, JToken? data) {
            this.destination = destination;
            this.data = data;
        }
    }
}
