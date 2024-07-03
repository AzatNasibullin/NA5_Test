
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NA5.Models
{
    public class Message
    {
        public int MessageId { get; set; }
        public string? Text { get; set; }
        public DateTime DateSend { get; set; }
        public bool IsSent { get; set; }
        public bool IsRead { get; set; }
        public int? UserToId { get; set; }
        public int? UserFromId { get; set; }
        public virtual User? UserTo { get; set; }
        public virtual User? UserFrom { get; set; }

        public override string ToString()
        {
            return $"{DateSend}.Получено сообщение {Text}";
        }
        public string SerializeMessageToJson()
        {
            return JsonSerializer.Serialize(this);
        }

        public static Message? DeserializeFromJson(string message) => JsonSerializer.Deserialize<Message>(message);

        public void Print()
        {
            Console.WriteLine(ToString());
        }
    }
}
