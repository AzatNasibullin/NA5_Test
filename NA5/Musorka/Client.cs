using System;
using NetMQ;
using NetMQ.Sockets;

public class Client
{
    private readonly string _address;

    public Client(string address)
    {
        _address = address;
    }

    public void SendMessage(string message)
    {
        using (var requestSocket = new RequestSocket())
        {
            requestSocket.Connect(_address);
            requestSocket.SendFrame(message);


            string response = requestSocket.ReceiveFrameString();
            Console.WriteLine($"Сервер: {response}");
        }
    }
}