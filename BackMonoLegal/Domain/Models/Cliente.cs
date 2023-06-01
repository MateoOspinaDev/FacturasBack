using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace BackMonoLegal.Domain.Models
{
    public class Cliente
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string? Nombre { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        public List<Factura>? Facturas;
    }
}
