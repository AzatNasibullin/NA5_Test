using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NA5.Interface
{

   public interface IMessageSource
    {
        void Start();
        void Stop();
        event EventHandler<string> MessageReceived;
    }
 
}
