using System.Text.Json;
using MonsterTradingCardGame.Server;

namespace MonsterTradingCardGame.Api {
    
    public class Response {

        // /////////////////////////////////////////////////////////////////////
        // Properties
        // /////////////////////////////////////////////////////////////////////

        public HttpCode HttpCode { get; private set; }

        public string? Body { get; private set; }

        public bool IsJson { get; private set; } = true;

        // /////////////////////////////////////////////////////////////////////
        // Constructor
        // /////////////////////////////////////////////////////////////////////

        public Response(HttpCode httpCode) {
            HttpCode = httpCode;
            Body = null;
            IsJson = false;
        }

        /// <summary>
        ///     Creates a new <see cref="Response"/> with the given <see cref="HttpCode"/> and message.
        /// </summary>
        /// <param name="httpCode">The <see cref="HttpCode"/> of the <see cref="Response"/></param>
        /// <param name="message">The message of the <see cref="Response"/></param>
        /// <param name="asJson">If true, the message will be sent as a JSON-Object</param>
        public Response(HttpCode httpCode, string message, bool asJson = true) {
            HttpCode = httpCode;

            if (asJson) {
                Body = $"{{ message: \"{message}\" }}";
            } else {
                Body = message;
                IsJson = false;
            }
        }

        public Response(HttpCode httpCode, object entity) {
            HttpCode = httpCode;
            Body = JsonSerializer.Serialize(entity);
        }
    }
}