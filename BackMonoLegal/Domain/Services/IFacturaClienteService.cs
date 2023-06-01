using BackMonoLegal.Domain.Models;

namespace BackMonoLegal.Domain.Servicios
{
    public interface IFacturaClienteService
    {
        Task<IEnumerable<Cliente>> GetAllClientes();
        Task<IEnumerable<FacturaClienteDTO>> GetAllFacturasClienteDTO();
        Task CambiarEstadoFactura(string clienteId);
        Task UpdateEstadoAll();
    }
}
