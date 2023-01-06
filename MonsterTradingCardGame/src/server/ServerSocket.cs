using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using Api.Endpoints;
using MonsterTradingCardGame.Api;
using MonsterTradingCardGame.Api.Endpoints;
using MonsterTradingCardGame.Api.Endpoints.Users;
using MonsterTradingCardGame.Data;
using Newtonsoft.Json;

namespace MonsterTradingCardGame.Server {

    internal class ServerSocket {

        private static readonly Logger<ServerSocket> _logger = new();

        private readonly ApiEndpointRegister _endpointRegister = new(typeof(Authentication),
                                                                     typeof(Battles),
                                                                     typeof(Cards),
                                                                     typeof(Decks),
                                                                     typeof(Packages),
                                                                     typeof(Scoreboard),
                                                                     typeof(Stats),
                                                                     typeof(Trades),
                                                                     typeof(Users));
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
            _logger.Info($"Received {request.Destination.method} request for {request.Destination.endpoint} from {endPoint}");

            Response response;
            try {
                response = _endpointRegister.Execute(request);
            } catch (NoSuchDestinationException) {
                response = new Response(HttpCode.NOT_FOUND_404);
            } catch (ProgrammerFailException e) {
                response = new Response(HttpCode.INTERNAL_SERVER_ERROR_500);
                _logger.Error(e.Message);
                Console.WriteLine(e.StackTrace);
            }

            SendResponse(client, response);
            client.Disconnect(true);
        }

        private static HttpRequest ParseRequest(string text) {
            Match requestMatch = new Regex(RegexUtils.REQUEST_GROUPS).Match(text);

            Regex headerRegex = new(RegexUtils.HEADERS_GROUPS, RegexOptions.Multiline);
            MatchCollection headerMatches = headerRegex.Matches(text);
            Dictionary<string, string> headers = new();
            foreach (Match headerMatch in headerMatches.Cast<Match>()) {
                string key = headerMatch.Groups[RegexUtils.HEADERS_KEY_GROUP_NAME].Value;
                string value = Regex.Replace(headerMatch.Groups[RegexUtils.HEADERS_VALUE_GROUP_NAME].Value, @"\n|\r", "");
                headers.Add(key, value);
            }

            Match dataMatch = new Regex(RegexUtils.REQUEST_BODY).Match(text);

            string httpMethod = requestMatch.Groups[RegexUtils.HTTP_METHOD_GROUP_NAME].Value;

            if (!Enum.TryParse(httpMethod, out EHttpMethod eHttpMethod)) {
                // TODO: throw method not allowed exception
            }

            string apiEndpoint = requestMatch.Groups[RegexUtils.ENDPOINT_GROUP_NAME].Value;
            return new HttpRequest(new(eHttpMethod, apiEndpoint), headers, new JsonTextReader(new StringReader(dataMatch.Value)));
        }

        private static void SendResponse(Socket client, Response response) {
            int length = response.Body == null ? 0 : response.Body.Length;

            StringBuilder sb = new($"HTTP/1.1 {response.HttpCode.Code} {response.HttpCode.Message}\r\n");
            sb.Append($"Date: {DateTime.Now}\r\n");
            sb.Append("Connection: close\r\n");
            sb.Append("Server: MonsterTradingCardGame .NET/6.0\r\n");
            sb.Append($"Content-Length: {length}\r\n");

            if (response.Body != null) {
                sb.Append("Content-Type: application/json\r\n\r\n");
                sb.Append(response.Body);
            } else {
                sb.Append("\r\n");
            }

            client.Send(Encoding.ASCII.GetBytes(sb.ToString()));
        }
    }
}
