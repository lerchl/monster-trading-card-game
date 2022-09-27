using Api;
using Newtonsoft.Json.Linq;

namespace server {

    internal class HttpRequest {

        public readonly Destination destination;
        public readonly JObject data;

        // /////////////////////////////////////////////////////////////////////
        // Constructor
        // /////////////////////////////////////////////////////////////////////

        public HttpRequest(Destination destination, JObject data) {
            this.destination = destination;
            this.data = data;
        }
    }
}
