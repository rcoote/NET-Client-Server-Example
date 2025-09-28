using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class SimpleServer
{
    private TcpListener listener;
    private List<TcpClient> clients = new List<TcpClient>(); // Keeps track of connected clients

    public void Start(int port)
    {
        listener = new TcpListener(IPAddress.Any, port);
        listener.Start();
        Console.WriteLine("Server started, waiting for connections...");

        Thread acceptThread = new Thread(AcceptClients);
        acceptThread.Start();
    }

    private void AcceptClients()
    {
        while (true)
        {
            try
            {
                TcpClient client = listener.AcceptTcpClient();
                clients.Add(client);
                Console.WriteLine("Client connected.");

                Thread clientThread = new Thread(() => HandleClient(client));
                clientThread.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error accepting client: " + ex.Message);
            }
        }
    }

    private void HandleClient(TcpClient client)
    {
        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[1024];
        int bytesRead;

        try
        {
            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine("Received: " + message);

                // Echo message back to the client
                byte[] response = Encoding.UTF8.GetBytes("Echo: " + message);
                stream.Write(response, 0, response.Length);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error communicating with client: " + ex.Message);
        }
        finally
        {
            stream.Close();
            client.Close();
            clients.Remove(client);
            Console.WriteLine("Client disconnected.");
        }
    }
}

class Program
{
    static void Main()
    {
        SimpleServer server = new SimpleServer();
        server.Start(5000); // Choose your port, any port!
    }
}
