using BLL.Interfaces;
using BLL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BLL
{
    public class RoomLogic : AbstractLogic<RoomLogic>, DBInterface
    {
       
        public RoomLogic() : base()
        {
        }
        public RoomLogic(string conStr)
        {
            _connectStr = conStr;
        }
        public MessageData<dynamic> All(string sessionId, string path, string url, DateTime updateTs)
        {           
            //string str = "";
            MessageData<dynamic> messageData = new MessageData<dynamic>("OK",updateTs,new List<dynamic>());
            using (Dal.Models.DataContext context = new Dal.Models.DataContext(_connectStr))
            {
                messageData.Datas.AddRange( context.Rooms.Include(r => r.Cupboards)
                        .Select(room => new Room(sessionId)
                        {
                            FileDir = path,
                            BaseUrl = url,
                            RoomId = room.RoomId,
                            Name = room.Name
                        ,
                            ImagePath = room.ImagePath
                        ,
                            Dal_Cupboard = room.Cupboards
                        ,
                            TS = room.CreateTs.ToJsTime()
                        }).ToList());               
            }
            //str = JsonConvert.SerializeObject(list, Formatting.None);
            return messageData;
        }

        public MessageObject DeleteWithId(int id, DateTime updateTs)
        {
            MessageObject messageData = new MessageObject("OK", updateTs);
            using (Dal.Models.DataContext context = new Dal.Models.DataContext(_connectStr))
            {
                var room = context.Rooms.Single(r => r.RoomId == id);
                context.Rooms.Remove(room);
                context.SaveChanges();
            }
            return messageData;
        }

        public Dal.Models.Room CreateRoom(Dictionary<string, StringValues> dic, DateTime updateTs)
        {            
            Dal.Models.Room room = new Dal.Models.Room();
            string sessionId = "";
            if (dic.ContainsKey("SessionId") && dic["SessionId"].Count > 0 && dic.ContainsKey("ImagePath") && dic["ImagePath"].Count > 0)
            {
                sessionId = dic["SessionId"][0];
                string userId = UserLogic.Instance.GetUserIdAndUpdateUpdateTs(sessionId,updateTs);
                if (string.IsNullOrEmpty(userId))
                    throw new ArgumentException("SesseionId is not found.", "sessionid");
                room.ImagePath = dic["ImagePath"][0];
                //room.UserId = userId;
            }
            if (dic.ContainsKey("RoomId") && dic["RoomId"].Count > 0)
            {
                room.RoomId = Convert.ToInt32(dic["RoomId"][0]);
            }
            if (dic.ContainsKey("Name") && dic["Name"].Count > 0)
            {
                room.Name = dic["Name"][0];
            }           

            return room;
        }
        public MessageObject Insert(Dictionary<string, StringValues> dic, DateTime updateTs)
        {
            MessageObject messageData = new MessageObject("OK", updateTs);
            using (Dal.Models.DataContext context = new Dal.Models.DataContext(_connectStr))
            {
                context.Rooms.Add(this.CreateRoom(dic, updateTs));
                context.SaveChanges();
            }
            return messageData;
        }

        public MessageObject Update(Dictionary<string, StringValues> dic, DateTime updateTs)
        {
            MessageObject messageData = new MessageObject("OK",updateTs);
            using (Dal.Models.DataContext context = new Dal.Models.DataContext(_connectStr))
            {
                Dal.Models.Room room = this.CreateRoom(dic, updateTs);
                var old = context.Rooms.Single(r => r.RoomId == room.RoomId);
                old.Name = room.Name;
                old.ImagePath = room.ImagePath;

                context.SaveChanges();
            }
            return messageData;
        }

    }
}
