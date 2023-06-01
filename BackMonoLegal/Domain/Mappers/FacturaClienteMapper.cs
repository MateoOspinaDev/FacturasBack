using BackMonoLegal.Domain.Models;
using System;
using System.Linq;

namespace BackMonoLegal.Domain.Mappers
{

    public static class FacturaClienteMapper
    {
        public static List<FacturaClienteDTO> MapClientesToDTO(List<Cliente> clientes)
        {
            List<FacturaClienteDTO> dtos = new List<FacturaClienteDTO>();

            foreach (var cliente in clientes)
            {
                IEnumerable<FacturaClienteDTO> collection()
                {
                    foreach (Factura factura in cliente.Facturas)
                    {
                        var dto = new FacturaClienteDTO
                              {
                                  Cliente = cliente.Nombre,
                                  CodigoFactura = factura.CodigoFactura,
                                  TotalFactura = factura.TotalFactura,
                                  Subtotal = factura.Subtotal,
                                  IVA = factura.IVA,
                                  Retencion = factura.Retencion,
                                  FechaDeCreacion = factura.FechaDeCreacion,
                                  Estado = factura.Estado,
                                  Ciudad = factura.Ciudad,
                                  Pagada = factura.Pagada,
                                  FechaDePago = factura.FechaDePago
                              };
                        yield return dto;
                    }
                }

                dtos.AddRange(collection: collection());
            }

            return dtos;
        }
    }
}
