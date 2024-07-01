using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class Message
{
    public int Id { get; set; }
    public string Content { get; set; }
    public bool Read { get; set; }
    public DateTime Timestamp { get; set; }
}

class Program
{
    static async Task Main(string[] args)
    {
        var client = new HttpClient();
        var serverUrl = "http://localhost/api/messages";

        await SendMessage(client, serverUrl, "Тест 123!");

        var messages = await ListMessages(client, serverUrl);
        ShowMessages(messages);
    }

    public static async Task SendMessage(HttpClient client, string url, string content)
    {
        var message = new Message { Content = content };
        var response = await client.PostAsJsonAsync(url + "/send_message", message);

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("Message sent!");
        }
        else
        {
            Console.WriteLine("Failed to send message: " + response.StatusCode);
        }
    }

    public static async Task<List<Message>> ListMessages(HttpClient client, string url)
    {
        var messages = await client.GetFromJsonAsync<List<Message>>(url + "/list_messages");
        return messages;
    }

    public static void ShowMessages(List<Message> messages)
    {
        if (messages != null && messages.Count > 0)
        {
            Console.WriteLine("Unread messages:");
            foreach (var message in messages)
            {
                Console.WriteLine($"[{message.Timestamp}] {message.Content}");
            }
        }
        else
        {
            Console.WriteLine("No unread messages.");
        }
    }
}