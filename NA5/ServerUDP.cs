using EFSeminar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NA5
{
    internal class ServerUDP
    {
        public List<string> messageList = new List<string>();
        public string msg = string.Empty;
        public CancellationTokenSource cts = new CancellationTokenSource();

        public static async Task Server(string message, CancellationTokenSource cancellationToken)
        {
            UdpClient udpClient = new UdpClient(12345);
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, 0);

            Console.WriteLine("Сервер ждет сообщение от клиента. Нажмите любую клавишу для выхода.");

            while (!cancellationToken.IsCancellationRequested)
            {
                UdpReceiveResult result = await udpClient.ReceiveAsync();
                byte[] buffer = result.Buffer;
                var messageText = Encoding.UTF8.GetString(buffer);

                NetMessage message1 = NetMessage.DeserializeFromJson(messageText);
                message1.Print();

                // Отправляем подтверждение клиенту
                string confirmationMessage = "Сообщение успешно обработано на сервере";
                byte[] confirmationBuffer = Encoding.UTF8.GetBytes(confirmationMessage);
                await udpClient.SendAsync(confirmationBuffer, confirmationBuffer.Length, result.RemoteEndPoint);

                if (message1.Text.ToLower() == "exit")
                {
                    cancellationToken.Cancel();
                    Console.WriteLine("Завершение работы...");
                }
            }

            udpClient.Close();
        }
    }
}
