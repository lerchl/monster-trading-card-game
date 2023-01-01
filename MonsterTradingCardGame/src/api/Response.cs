using System.Text.Json;
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

        public Response(HttpCode httpCode) {
            HttpCode = httpCode;
            Body = null;
        }

        public Response(HttpCode httpCode, string message) {
            HttpCode = httpCode;
            Body = $"{{ message: \"{message}\" }}";
        }

        public Response(HttpCode httpCode, object entity) {
            HttpCode = httpCode;
            Body = JsonSerializer.Serialize(entity);
        }
    }
}