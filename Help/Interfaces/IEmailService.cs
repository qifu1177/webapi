using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Help.Interfaces
{
    public interface IEmailService
    {
        public void Send(string receiver, string subject, string bodyText);
    }
}
