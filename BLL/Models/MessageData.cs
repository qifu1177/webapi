using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Models
{
    public class MessageData<T>: MessageObject
    {
        public MessageData(string message,DateTime ts, List<T> datas):base(message,ts)
        {           
            Datas = datas;
        }        
        public List<T> Datas { get; set; }
    }
}
