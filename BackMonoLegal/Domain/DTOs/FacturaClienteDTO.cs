namespace BackMonoLegal.Domain.Models
{
    public class FacturaClienteDTO
    {
        public string? Cliente { get; set; }
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
