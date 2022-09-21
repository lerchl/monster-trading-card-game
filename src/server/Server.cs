using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using Api;

namespace server {

    internal class Server {

        private static readonly Logger<Server> _logger = new();

        public Socket serverSocket;

        // /////////////////////////////////////////////////////////////////////
        // Constructor
        // /////////////////////////////////////////////////////////////////////
    
        public Server(int port) {
            serverSocket = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream,
                    ProtocolType.Tcp);
            
            serverSocket.Bind(new IPEndPoint(IPAddress.Loopback, port));
            serverSocket.Listen();
            _logger.Info("Listening on port " + port);
            
            for (;;) {
                serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), serverSocket);
            }
        }

        // /////////////////////////////////////////////////////////////////////
        // Methods
        // /////////////////////////////////////////////////////////////////////
    
        private void AcceptCallback(IAsyncResult ar) {
            Socket client = serverSocket.EndAccept(ar);
            EndPoint? endPoint = client.RemoteEndPoint;
            byte[] buffer = new byte[2048];
            int length = client.Receive(buffer);
            client.Disconnect(true);

            string text = Encoding.ASCII.GetString(buffer, 0, length);
            HttpRequest request = ParseRequest(text);
            _logger.Info($"Received {request.HttpMethod} request for {request.ApiEndpoint} from {endPoint}");
        
            
        }

        private static HttpRequest ParseRequest(string text) {
            string requestPattern = @"^(?'httpMethod'\w+) (?'endpoint'/\w*)";
            Regex requestRegex = new(requestPattern);
            Match requestMatch = requestRegex.Match(text);

            string dataPattern = @"(\{.*\})";
            Regex dataRegex = new(dataPattern);
            Match dataMatch = dataRegex.Match(text);

            string httpMethod = requestMatch.Groups["httpMethod"].Value;
            string apiEndpoint = requestMatch.Groups["endpoint"].Value;
            string data = dataMatch.Value;
            _ = Enum.TryParse(httpMethod, out EHttpMethod eHttpMethod);

            return new HttpRequest(eHttpMethod, apiEndpoint, data);
        }
    }
}
