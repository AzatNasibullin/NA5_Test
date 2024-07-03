using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NA5Client.Interface
{
    public interface IMessageSourceClient
    {
        void SendMessage(string message);
    }
}
