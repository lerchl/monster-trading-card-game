using System.Text.RegularExpressions;
using System.Text;
using System.Net;
using System.Net.Sockets;
using Api;
using Api.Endpoints;
using Newtonsoft.Json.Linq;
using MonsterTradingCardGame.Server;
using MonsterTradingCardGame.Data.Player;

namespace MonsterTradingCardGame {

    class Start {

        public static void Main(String[] args) {
            // ServerSocket server = new(10001);

            PlayerRepository playerRepository = new();
            playerRepository.FindById("bdac2854-04d1-4c6d-82fe-979dda81f9fa");

            // SERVER STUFF

            // Socket server = new Socket(AddressFamily.InterNetwork,
            //                            SocketType.Stream,
            //                            ProtocolType.Tcp);

            // server.Bind(new IPEndPoint(IPAddress.Loopback, 10001));
            // server.Listen(5);

            // Console.WriteLine("Accepting connection...");

            // Socket client = server.Accept();
            // byte[] buffer = new byte[9999];
            // int lenght = client.Receive(buffer);
            // string text = Encoding.ASCII.GetString(buffer, 0, lenght);
            // Console.WriteLine(text);


            // string requestPattern = @"^(?'httpMethod'\w+) (?'endpoint'/\w*)";
            // Regex requestRegex = new Regex(requestPattern);
            // Match requestMatch = requestRegex.Match(text);

            // string dataPattern = @"(\{.*\})";
            // Regex dataRegex = new Regex(dataPattern);
            // Match dataMatch = dataRegex.Match(text);

            // string httpMethod = requestMatch.Groups["httpMethod"].Value;
            // string apiEndpoint = requestMatch.Groups["endpoint"].Value;
            // string data = dataMatch.Value;

            // Console.WriteLine("Read Data: " + data);
            // JObject json = JObject.Parse(data);

            // er.execute(new Destination(convert(httpMethod), apiEndpoint), json);

            // client.Disconnect(false);
            // Console.WriteLine("Connection closed...");
        }
    }
}
