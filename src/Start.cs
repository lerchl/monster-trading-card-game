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

        client.Disconnect(false);

        Console.WriteLine("Connection closed...");
    }
}