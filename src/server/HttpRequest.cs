using Api;

namespace server {

    internal class HttpRequest {

        // turn into destination

        public EHttpMethod HttpMethod { get; }
        public string ApiEndpoint { get; }
        public string Data { get; }

        // /////////////////////////////////////////////////////////////////////
        // Constructor
        // /////////////////////////////////////////////////////////////////////

        public HttpRequest(EHttpMethod httpMethod, string apiEndpoint, string data) {
            HttpMethod = httpMethod;
            ApiEndpoint = apiEndpoint;
            Data = data;
        }
    }
}
