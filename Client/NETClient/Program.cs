using System;
using System.Net.Sockets;
using System.Text;

public class SimpleClient
{
    private TcpClient client;
    private NetworkStream stream;

    public void ConnectToServer(string ipAddress, int port)
    {
        client = new TcpClient(ipAddress, port);
        stream = client.GetStream();
        Console.WriteLine("Connected to the server.");

        // Start reading messages in the background
/*         ReadMessages(); */
    }

    private void ReadMessages()
    {
        byte[] buffer = new byte[1024];
        int bytesRead;

        while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
        {
            string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Console.WriteLine("Server says: " + message);
        }
    }

    public void SendMessage(string message)
    {
        if (stream != null && stream.CanWrite)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            stream.Write(data, 0, data.Length);
            Console.WriteLine("Message sent: " + message);
        }
    }
}

class Program
{
    static void Main()
    {
        SimpleClient client = new SimpleClient();
        client.ConnectToServer("37.120.167.27", 5000); // Connect to localhost

        Console.WriteLine("Type a message to send:");
        string message = Console.ReadLine();
        client.SendMessage(message);
    }
}