using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.Responses
{
    public class IdResponse:SessionResponse
    {
        public object Id { get; set; }
    }
}
