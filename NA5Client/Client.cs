using System;
using System.Text;
using NA5.Models;
using NA5Client.Interface;
using NetMQ;
using NetMQ.Sockets;

namespace Client
{
    class NetClient
    {
        public static void SentMessage(int userFromId, string userFromName, string ip)
        {
            NetMqMessageSourceClient client = new NetMqMessageSourceClient($"tcp://{ip}:12345");

            while (true)
            {
                string messageText;
                do
                {
                    Console.WriteLine("Введите сообщение (для выхода введите \"Exit\"): ");
                    messageText = Console.ReadLine();
                    if (messageText.ToLower() == "exit")
                    {
                        Console.WriteLine("Завершение работы клиента.");
                        return;
                    }
                }
                while (string.IsNullOrEmpty(messageText));

                Message message = new Message
                {
                    Text = messageText,
                    UserFromId = userFromId,
                    DateSend = DateTime.Now,
                    UserFrom = new User { Id = userFromId, FullName = userFromName },
                    UserTo = new User { Id = 0, FullName = "Server" }
                };

                string json = message.SerializeMessageToJson();

                // Отправка сообщения и получение подтверждения через NetMqMessageSourceClient
                client.SendMessage(json);
            }
        }
    }

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
                Console.WriteLine($"Подтверждение от сервера: {response}");
            }
        }
    }
}
