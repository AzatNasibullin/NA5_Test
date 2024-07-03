using NA5.Interface;
using NetMQ;
using NetMQ.Sockets;
using System;

namespace NA5
{
    public class NetMqMessageSourceClient : IMessageSourceClient
    {
        private readonly string _address;

        public NetMqMessageSourceClient(string address)
        {
            _address = address;
        }

        public void SendMessage(string message)
        {
            using (var requestSocket = new RequestSocket())
            {
                requestSocket.Connect(_address);
                requestSocket.SendFrame(message);

                // Получаем подтверждение от сервера
                string response = requestSocket.ReceiveFrameString();
                Console.WriteLine($"Ответ от сервера: {response}");
            }
        }
    }
}