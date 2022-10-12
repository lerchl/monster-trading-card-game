using System.Net.Sockets;
using System.Text;

namespace Api.Endpoints {

    internal class ApiEndpointUtils {

        // /////////////////////////////////////////////////////////////////////
        // Methods
        // /////////////////////////////////////////////////////////////////////

        public static void SendResponse(Socket client, int code, string message) {
            client.Send(Encoding.ASCII.GetBytes($"HTTP/1.1 {code} {message}\r\n\r\nAuthortization: Bearer 1234567890"));
        }
    }
}
