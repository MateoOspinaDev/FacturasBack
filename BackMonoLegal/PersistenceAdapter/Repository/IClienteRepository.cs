using BackMonoLegal.Domain.Models;

namespace BackMonoLegal.PersistenceAdapter.Repository
{
    public interface IClienteRepository
    {
        Task<IEnumerable<Cliente>> GetAll();
        Task<Cliente> GetById(string id);
        Task ActualizarEstadoFacturaCliente(string clienteId, Factura factura);
    }
}
