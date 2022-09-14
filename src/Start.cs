using System.Text.RegularExpressions;
using System.Text;
using System.Net;
using System.Net.Sockets;

class Start {

    public static void Main(String[] args) {
        Socket server = new Socket(AddressFamily.InterNetwork,
                                   SocketType.Stream,
                                   ProtocolType.Tcp);

        server.Bind(new IPEndPoint(IPAddress.Loopback, 3000));
        server.Listen(5);

        Console.WriteLine("Accepting connection...");

        Socket client = server.Accept();
        byte[] buffer = new byte[9999];
        int lenght = client.Receive(buffer);
        string text = Encoding.ASCII.GetString(buffer, 0, lenght);
        Console.WriteLine(text);


        string pattern = @"^(?'httpMethod'\w+) (?'endpoint'/\w*)";
        Regex regex = new Regex(pattern);
        Match match = regex.Match(text);
        string httpMethod = match.Groups["httpMethod"].Value;
        string apiEndpoint = match.Groups["endpoint"].Value;

        if (apiEndpoint.Equals("/users")) {
            Console.WriteLine($"{httpMethod}-Request an /users");
            client.Send(Encoding.ASCII.GetBytes("200 USER CREATED"));
        } else if (apiEndpoint.Equals("/cards")) {
            Console.WriteLine($"{httpMethod}-Request an /cards");
            client.Send(Encoding.ASCII.GetBytes("200 CARD CREATED"));
        } else {
            Console.WriteLine("Anfrage an Endpoint der nicht existiert.");
            client.Send(Encoding.ASCII.GetBytes("400 ENDPOINT DOES NOT EXIST"));
        }

        client.Disconnect(false);
        Console.WriteLine("Connection closed...");
    }
}