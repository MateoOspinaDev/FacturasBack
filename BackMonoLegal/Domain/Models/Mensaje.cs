using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace BackMonoLegal.Models
{
    public class Mensaje
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Notificacion { get; set; }

        public Cliente Cliente { get; set; }
    }
}
