using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using Api.Endpoints;
using MonsterTradingCardGame.Api;
using MonsterTradingCardGame.Api.Endpoints;
using Newtonsoft.Json.Linq;

namespace MonsterTradingCardGame.Server {

    internal class ServerSocket {

        private static readonly Logger<ServerSocket> _logger = new();

        private readonly ApiEndpointRegister _endpointRegister = new(typeof(Authentication), typeof(Packages));
        private readonly Socket _serverSocket;
        private bool wait = false;

        // /////////////////////////////////////////////////////////////////////
        // Constructor
        // /////////////////////////////////////////////////////////////////////
    
        public ServerSocket(int port) {
            _serverSocket = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream,
                    ProtocolType.Tcp);
            
            _serverSocket.Bind(new IPEndPoint(IPAddress.Loopback, port));
            _serverSocket.Listen();
            _logger.Info("Listening on port " + port);
            
            for (;;) {
                if (!wait) {
                    _logger.Info("Accepting new connection");
                    _serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), _serverSocket);
                    wait = true;
                }
                Thread.Sleep(100);
            }
        }

        // /////////////////////////////////////////////////////////////////////
        // Methods
        // /////////////////////////////////////////////////////////////////////
    
        private void AcceptCallback(IAsyncResult ar) {
            wait = false;

            Socket client = _serverSocket.EndAccept(ar);
            EndPoint? endPoint = client.RemoteEndPoint;
            byte[] buffer = new byte[2048];
            int length = client.Receive(buffer);

            string text = Encoding.ASCII.GetString(buffer, 0, length);
            HttpRequest request = ParseRequest(text);
            _logger.Info($"Received {request.destination.method} request for {request.destination.endpoint} from {endPoint}");

            _endpointRegister.Execute(request, client);

            client.Disconnect(true);
        }

        private static HttpRequest ParseRequest(string text) {
            string requestPattern = @"^(?'httpMethod'\w+) (?'endpoint'/\w*)";
            Regex requestRegex = new(requestPattern);
            Match requestMatch = requestRegex.Match(text);

            string dataPattern = @"(\[?\{.*\}\]?)";
            Regex dataRegex = new(dataPattern);
            Match dataMatch = dataRegex.Match(text);

            string arrayPattern = @"(\[\{.*\}\])";
            Regex arrayRegex = new(arrayPattern);
            bool isArray = arrayRegex.IsMatch(text);

            string httpMethod = requestMatch.Groups["httpMethod"].Value;
            _ = Enum.TryParse(httpMethod, out EHttpMethod eHttpMethod);
            string apiEndpoint = requestMatch.Groups["endpoint"].Value;

            Console.WriteLine(dataMatch.Value);

            
            return new HttpRequest(new(eHttpMethod, apiEndpoint), JContainer.Parse(dataMatch.Value));
        }
    }
}
