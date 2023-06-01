using BackMonoLegal.Domain.Models;
using BackMonoLegal.Domain.Servicios;
using BackMonoLegal.RestAdapter.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BackMonoLegalUnitTest;
public class FacturaControllerTests
{
    private readonly Mock<IFacturaClienteService> _facturaServiceMock;
    private readonly FacturaController _facturaController;

    public FacturaControllerTests()
    {
        _facturaServiceMock = new Mock<IFacturaClienteService>();
        _facturaController = new FacturaController(_facturaServiceMock.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsFacturas()
    {
        var facturasDto = new List<FacturaClienteDTO>
        {

            new FacturaClienteDTO
            {
                  Cliente = "Mateo Ospina",
                  CodigoFactura = "P-00001",
                  TotalFactura = 18000,
                  Subtotal = 15400,
                  IVA = 2600,
                  Retencion = null,
                  FechaDeCreacion = DateTime.MinValue,
                  Estado = "desactivado",
                  Ciudad = "Medellin",
                  Pagada = false,
                  FechaDePago = DateTime.MinValue
            }
        };
                        
        _facturaServiceMock.Setup(s => s.UpdateEstadoAll()).Verifiable();
        _facturaServiceMock.Setup(s => s.GetAllFacturasClienteDTO()).ReturnsAsync(facturasDto);

        var result = await _facturaController.GetAll();

        _facturaServiceMock.Verify(s => s.UpdateEstadoAll(), Times.Once);
        _facturaServiceMock.Verify(s => s.GetAllFacturasClienteDTO(), Times.Once);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var facturasResult = Assert.IsAssignableFrom<IEnumerable<FacturaClienteDTO>>(okResult.Value);
        Assert.Equal(facturasDto.Count, facturasResult.Count());
    }

    [Fact]
    public async Task GetAll_ReturnsNotFound_WhenFacturasIsNull()
    {
        // Arrange
        List<FacturaClienteDTO> facturasDto = null;
        _facturaServiceMock.Setup(s => s.UpdateEstadoAll()).Verifiable();
        _facturaServiceMock.Setup(s => s.GetAllFacturasClienteDTO()).ReturnsAsync(facturasDto);

        // Act
        var result = await _facturaController.GetAll();

        // Assert
        var notFoundResult = Assert.IsType<NotFoundResult>(result);
        _facturaServiceMock.Verify(s => s.UpdateEstadoAll(), Times.Once);
        _facturaServiceMock.Verify(s => s.GetAllFacturasClienteDTO(), Times.Once);
    }

}
