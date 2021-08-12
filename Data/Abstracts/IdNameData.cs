using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Abstracts
{
    public abstract class IdNameData : IdData
    {
        [BsonElement("Name")]
        public string Name { get; set; }
    }
}
