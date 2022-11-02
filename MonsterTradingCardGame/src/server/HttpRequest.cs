using MonsterTradingCardGame.Api;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MonsterTradingCardGame.Server {

    public class HttpRequest {

        public readonly Destination destination;
        public readonly JsonReader? data;

        // /////////////////////////////////////////////////////////////////////
        // Constructor
        // /////////////////////////////////////////////////////////////////////

        public HttpRequest(Destination destination, JsonReader? data) {
            this.destination = destination;
            this.data = data;
        }
    }
}
