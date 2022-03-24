using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace proje1b.Models
{
    [BsonIgnoreExtraElements]
    public class Araba
    {
        [BsonId]
        public ObjectId Id { get; set; }
        [BsonElement("date")]
        public DateTime Date { get; set; }
        [BsonElement("lat")]
        public string Lat { get; set; }
        [BsonElement("long")]
        public string Long { get; set; }
        [BsonElement("aracid")]
        public string ArabaID { get; set; }
    }
}