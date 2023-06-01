using BackMonoLegal.Domain.Models;
using BackMonoLegal.Domain.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace BackMonoLegal.RestAdapter.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FacturaController : ControllerBase
    {

        private readonly IFacturaClienteService _facturaService;

        public FacturaController(IFacturaClienteService facturaService)
        {
            _facturaService = facturaService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            await _facturaService.UpdateEstadoAll();
            var facturas = await _facturaService.GetAllFacturasClienteDTO();

            if (facturas == null)
            {
                return NotFound();
            }

            return Ok(facturas);
        }
    }
}