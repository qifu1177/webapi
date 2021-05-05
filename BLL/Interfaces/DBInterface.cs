using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Interfaces
{
    interface DBInterface
    {
        void Insert(Dictionary<string, StringValues> dic);
        void Update(Dictionary<string, StringValues> dic);
        void DeleteWithId(int id);
    }
}
