using System;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите ваше имя: ");
            string userName = Console.ReadLine();

            Console.WriteLine("Введите ваш ID: ");
            int userId = int.Parse(Console.ReadLine());

           // Console.WriteLine("Введите IP адрес сервера: ");
           // string ip = Console.ReadLine();

            NetClient.SentMessage(userId, userName, "localhost");

            Console.WriteLine("Нажмите любую клавишу для завершения...");
            Console.ReadKey();
        }
    }
}