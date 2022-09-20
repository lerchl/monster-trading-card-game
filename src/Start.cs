using System.Text.RegularExpressions;
using System.Text;
using System.Net;
using System.Net.Sockets;
using Api;
using Api.Endpoints;
using Newtonsoft.Json.Linq;

class Start {

    public static void Main(String[] args) {
        ApiEndpointRegister er = new ApiEndpointRegister(typeof(Users));




        // SERVER STUFF

        Socket server = new Socket(AddressFamily.InterNetwork,
                                   SocketType.Stream,
                                   ProtocolType.Tcp);

        server.Bind(new IPEndPoint(IPAddress.Loopback, 2000));
        server.Listen(5);

        Console.WriteLine("Accepting connection...");

        Socket client = server.Accept();
        byte[] buffer = new byte[9999];
        int lenght = client.Receive(buffer);
        string text = Encoding.ASCII.GetString(buffer, 0, lenght);
        Console.WriteLine(text);


        string requestPattern = @"^(?'httpMethod'\w+) (?'endpoint'/\w*)";
        Regex requestRegex = new Regex(requestPattern);
        Match requestMatch = requestRegex.Match(text);

        string dataPattern = @"^.*\z";
        Regex dataRegex = new Regex(dataPattern);
        Match dataMatch = dataRegex.Match(text);

        string httpMethod = requestMatch.Groups["httpMethod"].Value;
        string apiEndpoint = requestMatch.Groups["endpoint"].Value;
        string data = dataMatch.Value;

        JObject json = JObject.Parse(data);

        er.execute(new Destination(convert(httpMethod), apiEndpoint));

        client.Disconnect(false);
        Console.WriteLine("Connection closed...");
    }

    private static Api.HttpMethod convert(string httpMethod) {
        switch (httpMethod) {
            case "GET":
                return Api.HttpMethod.GET;
            case "POST":
                return Api.HttpMethod.POST;
            case "PUT":
                return Api.HttpMethod.PUT;
            case "DELETE":
                return Api.HttpMethod.DELETE;
            default:
                throw new NotSupportedException();
        }
    }
}