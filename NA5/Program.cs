using NA5;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NA5
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var cts = new CancellationTokenSource();

            Task serverTask = Task.Run(() => ServerNet.Server("Салам пополам клиент", cts), cts.Token);
            await serverTask;
            Console.WriteLine("Для выхода нажмите любую клавишу...");
            Console.ReadKey();
            cts.Cancel();

            //await serverTask;
        }
    }
}