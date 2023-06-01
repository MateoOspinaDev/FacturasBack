using BackMonoLegal.Domain.Constants;
using BackMonoLegal.Domain.Models;
using BackMonoLegal.NotificationAdapter.EmailNotification;
using BackMonoLegal.PersistenceAdapter.Repository;
using BackMonoLegal.ServicesImplementation;
using Moq;


namespace BackMonoLegalUnitTest
{
    public class FacturaClienteServiceTest
    {
        private readonly Mock<IClienteRepository> _clienteRepositoryMock;
        private readonly Mock<IEmailService> _emailServiceMock;
        private readonly FacturaClienteService facturaClienteService;

        public FacturaClienteServiceTest()
        {
            _clienteRepositoryMock = new Mock<IClienteRepository>();
            _emailServiceMock = new Mock<IEmailService>();
            facturaClienteService = new FacturaClienteService(_emailServiceMock.Object, _clienteRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAllClientes_ReturnsClientes()
        {
            var clientes = new List<Cliente>
            {
                new Cliente { Id = "1", Nombre = "Cliente 1", Email = "Email@Email.Email", Facturas = new List<Factura>{ } },
                new Cliente { Id = "2", Nombre = "Cliente 2", Email = "Email@Email.Email", Facturas = new List<Factura>{ } },
                new Cliente { Id = "3", Nombre = "Cliente 3", Email = "Email@Email.Email", Facturas = new List<Factura>{ } }
            };

            _clienteRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(clientes);

            var result = await facturaClienteService.GetAllClientes();

            Assert.Equal(clientes, result);
        }

        [Fact]
        public async Task CambiarEstadoFacturaValidClienteIdUpdatesFacturasAndSendEmailsTwoTimes()
        {
            var clienteId = "1";
            var cliente = new Cliente { Id = clienteId, Nombre = "Cliente 1" };
            var factura1 = new Factura { Id = "1", Estado = EstadosConstants.PrimerRecordatorio };
            var factura2 = new Factura { Id = "2", Estado = EstadosConstants.SegundoRecordatorio };
            cliente.Facturas = new List<Factura> { factura1, factura2 };

            _clienteRepositoryMock.Setup(repo => repo.GetById(clienteId)).ReturnsAsync(cliente);

            await facturaClienteService.CambiarEstadoFactura(clienteId);

            _clienteRepositoryMock.Verify(repo => repo.GetById(clienteId), Times.Once);
            _clienteRepositoryMock.Verify(repo => repo.ActualizarEstadoFacturaCliente(clienteId, It.IsAny<Factura>()), Times.Exactly(2));
            _emailServiceMock.Verify(emailService => emailService.SendEmail(It.IsAny<EmailDTO>()), Times.Exactly(2));
            Assert.Equal(EstadosConstants.SegundoRecordatorio, factura1.Estado);
            Assert.Equal(EstadosConstants.Desactivado, factura2.Estado);
        }

        [Fact]
        public async Task CambiarEstadoFacturaValidClienteIdUpdatesFacturasAndSendEmailsOneTime()
        {
            var clienteId = "1";
            var cliente = new Cliente { Id = clienteId, Nombre = "Cliente 1" };
            var factura1 = new Factura { Id = "1", Estado = EstadosConstants.PrimerRecordatorio };
            var factura2 = new Factura { Id = "2", Estado = EstadosConstants.Desactivado };
            cliente.Facturas = new List<Factura> { factura1, factura2 };

            _clienteRepositoryMock.Setup(repo => repo.GetById(clienteId)).ReturnsAsync(cliente);

            await facturaClienteService.CambiarEstadoFactura(clienteId);

            _clienteRepositoryMock.Verify(repo => repo.GetById(clienteId), Times.Once);
            _clienteRepositoryMock.Verify(repo => repo.ActualizarEstadoFacturaCliente(clienteId, It.IsAny<Factura>()), Times.Exactly(1));
            _emailServiceMock.Verify(emailService => emailService.SendEmail(It.IsAny<EmailDTO>()), Times.Exactly(1));
            Assert.Equal(EstadosConstants.SegundoRecordatorio, factura1.Estado);
            Assert.Equal(EstadosConstants.Desactivado, factura2.Estado);
            
        }

        [Fact]
        public async Task CambiarEstadoFacturaValidClienteIdUpdatesFacturasAndNotSendEmails()
        {
            var clienteId = "1";
            var cliente = new Cliente { Id = clienteId, Nombre = "Cliente 1" };
            var factura1 = new Factura { Id = "1", Estado = EstadosConstants.Desactivado };
            var factura2 = new Factura { Id = "2", Estado = EstadosConstants.Desactivado };
            cliente.Facturas = new List<Factura> { factura1, factura2 };

            _clienteRepositoryMock.Setup(repo => repo.GetById(clienteId)).ReturnsAsync(cliente);

            await facturaClienteService.CambiarEstadoFactura(clienteId);

            _clienteRepositoryMock.Verify(repo => repo.GetById(clienteId), Times.Once);
            _clienteRepositoryMock.Verify(repo => repo.ActualizarEstadoFacturaCliente(clienteId, It.IsAny<Factura>()), Times.Exactly(0));
            _emailServiceMock.Verify(emailService => emailService.SendEmail(It.IsAny<EmailDTO>()), Times.Exactly(0));
            Assert.Equal(EstadosConstants.Desactivado, factura1.Estado);
            Assert.Equal(EstadosConstants.Desactivado, factura2.Estado);
        }

        [Fact]
        public async Task UpdateEstadoAll_UpdatesAllFacturas()
        {
            var clientes = new List<Cliente>
            {
                new Cliente { Id = "1", Nombre = "Cliente 1", Facturas = new List<Factura> { new Factura { Id = "1", Estado=EstadosConstants.PrimerRecordatorio } } },
                new Cliente { Id = "2", Nombre = "Cliente 2", Facturas = new List<Factura> { new Factura { Id = "2", Estado = EstadosConstants.SegundoRecordatorio } } },
                new Cliente { Id = "3", Nombre = "Cliente 3", Facturas = new List<Factura> { new Factura { Id = "3", Estado = EstadosConstants.Desactivado } } }
            };

            _clienteRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(clientes);
            _clienteRepositoryMock.Setup(repo=> repo.GetById("1")).ReturnsAsync(clientes[0]);
            _clienteRepositoryMock.Setup(repo => repo.GetById("2")).ReturnsAsync(clientes[1]);
            _clienteRepositoryMock.Setup(repo => repo.GetById("3")).ReturnsAsync(clientes[2]);

            await facturaClienteService.UpdateEstadoAll();

            _clienteRepositoryMock.Verify(repo => repo.GetAll(), Times.Once);
            _clienteRepositoryMock.Verify(repo => repo.ActualizarEstadoFacturaCliente(It.IsAny<string>(), It.IsAny<Factura>()), Times.Exactly(2));
        }

        [Fact]
        public async Task GetAllFacturasClienteDTO_ReturnsFacturasDTO()
        {
            var clientes = new List<Cliente>
        {
            new Cliente { Id = "1", Nombre = "Cliente 1", Facturas = new List<Factura> { new Factura { Id = "1" } } },
            new Cliente { Id = "2", Nombre = "Cliente 2", Facturas = new List<Factura> { new Factura { Id = "2" } } },
            new Cliente { Id = "3", Nombre = "Cliente 3", Facturas = new List<Factura> { new Factura { Id = "3" } } }
        };

            _clienteRepositoryMock.Setup(repo => repo.GetAll()).ReturnsAsync(clientes);

            var result = await facturaClienteService.GetAllFacturasClienteDTO();

            Assert.NotNull(result);
            Assert.Equal(clientes.Count, result.Count());
            Assert.IsAssignableFrom<IEnumerable<FacturaClienteDTO>>(result);
        }

        [Fact]
        public void SendEstadoFacturaEmail_SendsEmailWithCorrectParameters()
        {
            var factura = new Factura
            {
                CodigoFactura = "F-0001",
                Estado = EstadosConstants.SegundoRecordatorio
            };
            var cliente = new Cliente
            {
                Email = "Mateo@example.com",
                Nombre = "Mateo Ospina"
            };
            var estadoAnterior = EstadosConstants.PrimerRecordatorio;

            EmailDTO capturedEmail = null;
            _emailServiceMock.Setup(e => e.SendEmail(It.IsAny<EmailDTO>())).Callback<EmailDTO>(email => capturedEmail = email);

            facturaClienteService.SendEstadoFacturaEmail(factura, cliente, estadoAnterior);

            Assert.NotNull(capturedEmail);
            Assert.Equal(cliente.Email, capturedEmail.To);
            Assert.Equal("<h1>Cambio de estado en la factura</h1>", capturedEmail.Subject);
            Assert.Equal($"Hola {cliente.Nombre}<h2>Su factura con ID {factura.CodigoFactura} ha cambiado de estado: {estadoAnterior} a estado: {factura.Estado}.</h2>", capturedEmail.Body);
        }

        [Fact]
        public void ShouldReturnFactureWithStateinSegundoRecordatorio()
        { 
            var facturaResult = new Factura
            {
                CodigoFactura = "F-0001",
                Estado = EstadosConstants.SegundoRecordatorio
            };

            var factura = new Factura
            {
                CodigoFactura = "F-0001",
                Estado = EstadosConstants.PrimerRecordatorio
            };

            var result = facturaClienteService.CambiarEstado(factura);
            Assert.NotNull(result);
            Assert.Equal(facturaResult.Estado, result.Estado);
        }

        [Fact]
        public void ShouldReturnFactureWithSameStateWhenIsDesactivado()
        {
            var factura = new Factura
            {
                CodigoFactura = "F-0001",
                Estado = EstadosConstants.Desactivado
            };

            var result = facturaClienteService.CambiarEstado(factura);
            Assert.NotNull(result);
            Assert.Equal(factura.Estado, result.Estado);
        }
    }
}
