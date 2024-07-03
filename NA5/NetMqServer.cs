using System;
using System.Threading;
using System.Threading.Tasks;
using NA5.Models;
using NA5.Interface;
using NetMQ;
using NetMQ.Sockets;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

public class NetMqServer : IMessageSource
{
    private readonly string _address;
    private bool _running;
    private Thread _listenerThread;

    public event EventHandler<string> MessageReceived;

    public NetMqServer(string address)
    {
        _address = address;
    }

    public void Start()
    {
        _running = true;
        _listenerThread = new Thread(ListenForMessages) { IsBackground = true };
        _listenerThread.Start();
    }

    public void Stop()
    {
        _running = false;
        _listenerThread?.Join();
    }

    private void ListenForMessages()
    {
        using (var subscriber = new SubscriberSocket())
        {
            subscriber.Bind(_address);
            subscriber.SubscribeToAnyTopic();

            while (_running)
            {
                string message = subscriber.ReceiveFrameString();
                MessageReceived?.Invoke(this, message);

                using (var responseSocket = new ResponseSocket())
                {
                    responseSocket.Bind("tcp://localhost:12345");
                    responseSocket.SendFrame("Сообщение успешно обработано на сервере");

                    if (message.ToLower() == "exit")
                    {
                        _running = false;
                        Console.WriteLine("Завершение работы...");
                    }
                }
            }
        }
    }

}

public class ServerNet
{
    public List<string> messageList = new List<string>();
    public string msg = string.Empty;
    public CancellationTokenSource cts = new CancellationTokenSource();

    public static async Task Server(string message, CancellationTokenSource cancellationToken)
    {
        NetMqServer netMqServer = new NetMqServer("tcp://localhost:12345");

        netMqServer.MessageReceived += (sender, messageText) =>
        {
            var message1 = Message.DeserializeFromJson(messageText);
            message1.Print();

            if (message1.Text.ToLower() == "exit")
            {
                cancellationToken.Cancel();
                Console.WriteLine("Завершение работы...");
            }
        };

        netMqServer.Start();

        Console.WriteLine("Сервер ждет сообщение от клиента. Нажмите любую клавишу для выхода.");

        while (!cancellationToken.IsCancellationRequested)
        {
            await Task.Delay(1000);
        }

        netMqServer.Stop();
    }
}