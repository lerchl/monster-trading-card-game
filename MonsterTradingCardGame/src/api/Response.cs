using MonsterTradingCardGame.Server;

namespace MonsterTradingCardGame.Api {
    
    public class Response {

        // /////////////////////////////////////////////////////////////////////
        // Properties
        // /////////////////////////////////////////////////////////////////////

        public HttpCode HttpCode { get; private set; }

        public string? Body { get; private set; }

        // /////////////////////////////////////////////////////////////////////
        // Constructor
        // /////////////////////////////////////////////////////////////////////

        public Response(HttpCode httpCode, string? body = null) {
            HttpCode = httpCode;
            Body = body;
        }
    }
}