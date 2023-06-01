using BackMonoLegal.Domain.Constants;
using BackMonoLegal.Domain.Mappers;
using BackMonoLegal.Domain.Models;
using BackMonoLegal.Domain.Servicios;
using BackMonoLegal.NotificationAdapter.EmailNotification;
using BackMonoLegal.PersistenceAdapter.Repository;

namespace BackMonoLegal.ServicesImplementation
{
    public class FacturaClienteService : IFacturaClienteService
    {
        private readonly IEmailService emailService;
        private readonly IClienteRepository clienteRepository;

        public FacturaClienteService(IEmailService emailService, IClienteRepository clienteRepository)
        {
            this.emailService = emailService;
            this.clienteRepository = clienteRepository;
        }

        public async Task<IEnumerable<Cliente>> GetAllClientes()
        {
            return await clienteRepository.GetAll();
        }

        public async Task CambiarEstadoFactura(string clienteId)
        {
            var cliente = await clienteRepository.GetById(clienteId);

            if (cliente != null)
            {
                var tareasActualizacion = new List<Task>();

                foreach (var factura in cliente.Facturas)
                {
                    Console.WriteLine("Estado antes del metodo: " + factura.Estado);
                    var anteriorEstado = factura.Estado;
                    var facturaUpdated = CambiarEstado(factura);
                    Console.WriteLine("Factura original: " + factura.Estado + " Factura nueva: " + facturaUpdated.Estado);
                    if (facturaUpdated.Estado != anteriorEstado)
                    {
                        Console.WriteLine("Entra en el if para actualizar");
                        tareasActualizacion.Add(clienteRepository.ActualizarEstadoFacturaCliente(clienteId, facturaUpdated));
                        SendEstadoFacturaEmail(facturaUpdated, cliente, anteriorEstado);
                    }
                }

                await Task.WhenAll(tareasActualizacion);
            }
        }


        public async Task UpdateEstadoAll()
        {
            var facturas = await GetAllClientes();

            foreach (var factura in facturas)
            {
                await CambiarEstadoFactura(factura.Id);
            }
        }

        public async Task<IEnumerable<FacturaClienteDTO>> GetAllFacturasClienteDTO()
        {
            var facturas = await GetAllClientes();
            List<Cliente> clientes = new(facturas);
            var facturasDTO = FacturaClienteMapper.MapClientesToDTO(clientes);
            return facturasDTO;
        }



        public void SendEstadoFacturaEmail(Factura factura, Cliente cliente, string estadoAnterior)
        {
            EmailDTO emailDTO = new()
            {
                To = cliente.Email,
                Subject = "Cambio de estado en la factura",
                Body = $"<h1>Hola {cliente.Nombre}</h1>" +
                $"<h2>Su factura con ID {factura.CodigoFactura} ha cambiado de estado: {estadoAnterior} a estado: {factura.Estado}.</h2>"
            };
            emailService.SendEmail(emailDTO);
        }

        public Factura CambiarEstado(Factura factura)
        {
            var facturaUpdated = factura;
            Console.WriteLine("En el case");
            switch (facturaUpdated.Estado)
            {
                case EstadosConstants.PrimerRecordatorio:
                    Console.WriteLine("En el case 1");
                    facturaUpdated.Estado = EstadosConstants.SegundoRecordatorio;
                    break;
                case EstadosConstants.SegundoRecordatorio:
                    Console.WriteLine("En el case 2");
                    facturaUpdated.Estado = EstadosConstants.Desactivado;
                    break;
                default:
                    Console.WriteLine("En el case default");
                    break;
            }

            return facturaUpdated;
        }
    }
}
