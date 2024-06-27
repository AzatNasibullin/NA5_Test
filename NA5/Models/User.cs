using NA5.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NA5.Models
{
    public class User
    {

        public string FullName { get; set; }
        public int Id { get; set; }
        public virtual List<Message>? MessagesTo { get; set; } = new List<Message>();
        public virtual List<Message>? MessagesFrom { get; set; } = new List<Message>();

    }
}
