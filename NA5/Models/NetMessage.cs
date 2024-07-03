using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace NA5.Models
{
    public enum Command
    {
        Register,
        Message,
        Confirmation
    }
    public class NetMessage
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime DateTime { get; set; }
        public string NicknameFrom { get; set; }
        public string NicknameTo { get; set; }
        public Command Command { get; set; }
        public string SerializeMessageToJson()
        {
            return JsonSerializer.Serialize(this);
        }

        public static NetMessage? DeserializeFromJson(string message) => JsonSerializer.Deserialize<NetMessage>(message);

        public void Print()
        {
            Console.WriteLine(ToString());
        }
        public override string ToString()
        {
            return $"{this.DateTime} получено сообщение {this.Text}  от  {this.NicknameFrom}";
        }
    }
}
