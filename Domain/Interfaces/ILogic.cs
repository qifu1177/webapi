using Domain.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ILogic<Request,Response>where Request:IRequest where Response: IResponse
    {
        MessageSessionResponse Update(string language, string sessionId, Request request);
        IdResponse Insert(string language, string sessionId, Request request);
        Response LoadAll(string language, string sessionId);
        Response Load(string language, string sessionId, object parentId);
        Response LoadWithId(string language, string sessionId, object id);
        MessageSessionResponse DeleteWithId(string language, string sessionId, object id);
    }
}
