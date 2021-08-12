using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Responses
{
    public class MessageResponse:IResponse
    {
        public string Message { get; set; }
    }
}
