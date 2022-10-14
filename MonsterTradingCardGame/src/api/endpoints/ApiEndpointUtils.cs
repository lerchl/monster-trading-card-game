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
            SendResponse(client, httpCode, "");
        }

        public static void SendResponse(Socket client, HttpCode httpCode, string body) {
            client.Send(Encoding.ASCII.GetBytes(
                $"HTTP/1.1 {httpCode.Code} {httpCode.Message}\n\r" +
                $"Date: {DateTime.Now}\n\r" +
                 "Connection: close\n\r" +
                 "Server: .NET/6.0\n\r" +
                 "Content-Type: application/json\n\r" +
                 "Content-Length: " + body.Length + "\n\r\n\r" +
                body
            ));
        }
    }
}
