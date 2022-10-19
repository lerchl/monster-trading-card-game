using System.Net.Sockets;
using System.Text;
using MonsterTradingCardGame.Api;
using MonsterTradingCardGame.Server;

namespace Api.Endpoints {

    internal class ApiEndpointUtils {

        // /////////////////////////////////////////////////////////////////////
        // Methods
        // /////////////////////////////////////////////////////////////////////

        public static void SendResponse(Socket client, HttpCode httpCode) {
            SendResponse(client, httpCode, null);
        }

        public static void SendResponse(Socket client, HttpCode httpCode, string? body) {
            int length = body == null ? 0 : body.Length;

            StringBuilder sb = new($"HTTP/1.1 {httpCode.Code} {httpCode.Message}\r\n");
            sb.Append($"Date: {DateTime.Now}\r\n");
            sb.Append("Connection: close\r\n");
            sb.Append("Server: MonsterTradingCardGame .NET/6.0\r\n");
            sb.Append($"Content-Length: {length}\r\n");

            if (body != null) {
                sb.Append("Content-Type: application/json\r\n\r\n");
                sb.Append(body);
            } else {
                sb.Append("\r\n");
            }

            client.Send(Encoding.ASCII.GetBytes(sb.ToString()));
        }
    }
}
