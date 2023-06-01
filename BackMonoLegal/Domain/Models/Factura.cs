using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace BackMonoLegal.Domain.Models
{
    public class Factura
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        public string? CodigoFactura { get; set; }

        public double? TotalFactura { get; set; }

        public double? Subtotal { get; set; }

        public double? IVA { get; set; }

        public double? Retencion { get; set; }

        public DateTime FechaDeCreacion { get; set; }

        public string? Estado { get; set; }

        public string? Ciudad { get; set; }

        public bool? Pagada { get; set; }

        public DateTime FechaDePago { get; set; }

    }
}
