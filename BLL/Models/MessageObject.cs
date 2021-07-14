using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Models
{
   
    public class MessageObject
    {
        public MessageObject(string message, DateTime ts)
        {
            Message = message;
            LastUpdateMs = ts.ToJsTime();           
        }
        public string Message { get; set; }
        public double LastUpdateMs { get; set; }       
    }
}
