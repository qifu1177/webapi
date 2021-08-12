using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Help.Exceptions
{
    public class MessagesException:Exception
    {
        public MessagesException(List<string> messages):base(string.Join(";",messages))
        {
            Messages = messages;
        }
        public List<string> Messages { get; set; }
    }
}
