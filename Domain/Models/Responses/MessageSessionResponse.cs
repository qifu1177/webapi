using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Responses
{
    public class MessageSessionResponse:SessionResponse
    {
        public string Message { get; set; }
    }
}
