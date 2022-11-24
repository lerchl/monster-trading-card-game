using MonsterTradingCardGame.Api;
using Newtonsoft.Json;

namespace MonsterTradingCardGame.Server {

    public class HttpRequest {

        public Destination Destination { get; private set; }
        public Dictionary<string, string> Headers { get; private set; }
        public JsonReader? Data { get; private set; }

        // /////////////////////////////////////////////////////////////////////
        // Constructor
        // /////////////////////////////////////////////////////////////////////

        public HttpRequest(Destination destination, Dictionary<string, string> headers, JsonReader? data) {
            Destination = destination;
            Headers = headers;
            Data = data;
        }
    }
}
