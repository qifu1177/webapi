using BLL.Interfaces;
using BLL.Models;
using Dal;
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
        private string _conStr;
        public RoomLogic() : base()
        {
            int[] a = new int[10];
            Array.Resize(ref a, a.Length + 2);
        }
        public RoomLogic(string conStr)
        {
            _conStr = conStr;
        }
        public string All()
        {
            string str = "";
            List<Room> list = new List<Room>();
            using (DataContext context = new DataContext(_conStr))
            {
                list = context.Rooms.Include(r => r.Cupboards)
                        .Select(room => new Room
                        {
                            RoomId = room.RoomId,
                            Name = room.Name
                        ,
                            ImagePath = room.ImagePath
                        ,
                            Dal_Cupboard = room.Cupboards
                        ,
                            TS = room.TS.ToJsTime()
                        }).ToList();
            }
            str = JsonConvert.SerializeObject(list, Formatting.None);
            return str;
        }

        public void DeleteWithId(int id)
        {
            using (DataContext context = new DataContext(_conStr))
            {
                var room = context.Rooms.Single(r => r.RoomId == id);                
                context.Rooms.Remove(room);
                context.SaveChanges();
            }
        }
                
        public Dal.Models.Room CreateRoom(Dictionary<string, StringValues> dic)
        {
            Dal.Models.Room room = new Dal.Models.Room();
            string sessionId = "";
            if(dic.ContainsKey("SessionId") && dic["SessionId"].Count>0 && dic.ContainsKey("ImagePath") && dic["ImagePath"].Count > 0)
            {
                sessionId = dic["SessionId"][0];
                if(string.IsNullOrEmpty(sessionId))
                    throw new ArgumentException("SesseionId is not found.", "sessionid");
                room.ImagePath= dic["ImagePath"][0];
                if(!string.IsNullOrEmpty(room.ImagePath))
                {
                    room.ImagePath = CopyFile(sessionId, room.ImagePath);
                }
            }
            if (dic.ContainsKey("RoomId") && dic["RoomId"].Count > 0)
            {
                room.RoomId = Convert.ToInt32(dic["RoomId"][0]);
            }
            if (dic.ContainsKey("Name") && dic["Name"].Count > 0)
            {
                room.Name = dic["Name"][0];
            }
            if (dic.ContainsKey("TS") && dic["TS"].Count > 0)
            {
                 if(!string.IsNullOrEmpty(dic["TS"][0]))
                {
                    room.TS= DateTimeExtensions.CreateFromJsTime(Convert.ToDouble(dic["TS"][0]));
                }
            }
            return room;
        }
        public void Insert(Dictionary<string, StringValues> dic)
        {
            using (DataContext context = new DataContext(_conStr))
            {
                context.Rooms.Add(this.CreateRoom(dic));
                context.SaveChanges();
            }
        }

        public void Update(Dictionary<string, StringValues> dic)
        {
            using (DataContext context = new DataContext(_conStr))
            {
                Dal.Models.Room room = this.CreateRoom(dic);
                var old = context.Rooms.Single(r => r.RoomId == room.RoomId);
                old.Name = room.Name;
                old.ImagePath = room.ImagePath;
               
                context.SaveChanges();
            }
        }
    }
}
