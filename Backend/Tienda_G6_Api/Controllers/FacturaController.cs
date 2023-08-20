using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Tienda_G6_Api.Entities;
using Tienda_G6_Api.Models;

namespace Tienda_G6_Api.Controllers
{
    public class FacturaController : ApiController
    {
        [HttpGet]
        [Route("api/ConsultarFacturas")]
        public IHttpActionResult ConsultarFacturas()
        {
            try
            {
                using (var bd = new Tienda_G6Entities1())
                {
                    var facturas = (from x in bd.Factura
                                    select x).ToList();

                    if (facturas.Count > 0)
                    {
                        List<FacturaEnt> res = new List<FacturaEnt>();
                        foreach (var item in facturas)
                        {
                            res.Add(new FacturaEnt
                            {
                                IdFactura = item.IdFactura,
                                IdCliente = item.IdCliente,
                                Fecha = item.Fecha,
                                Total = item.Total,
                                Estado = item.Estado
                            });
                        }

                        return Ok(res);
                    }

                    return BadRequest("No se encontraron facturas en la base de datos.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al Consultar facturas: " + ex.Message);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("api/ConsultarFactura")]
        public IHttpActionResult ConsultarFactura(long q)
        {
            try
            {
                using (var bd = new Tienda_G6Entities1())
                {
                    var factura = (from x in bd.Factura
                                   where x.IdFactura == q
                                   select x).FirstOrDefault();

                    if (factura != null)
                    {
                        return Ok(new FacturaEnt
                        {
                            IdFactura = factura.IdFactura,
                            IdCliente = factura.IdCliente,
                            Fecha = factura.Fecha,
                            Total = factura.Total,
                            Estado = factura.Estado
                        });
                    }

                    return BadRequest($"Factura con ID '{q}' no encontrada en la base de datos.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al consultar factura: " + ex.Message);
                return InternalServerError();
            }
        }

        [HttpPost]
        [Route("api/AgregarFactura")]
        public IHttpActionResult AgregarFactura(FacturaEnt entidad)
        {
            try
            {
                if (entidad == null)
                {
                    return BadRequest("El objeto factura no puede ser nulo.");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                using (var bd = new Tienda_G6Entities1())
                {
                    var factura = new Factura
                    {
                        IdCliente = entidad.IdCliente,
                        Fecha = entidad.Fecha,
                        Total = entidad.Total,
                        Estado = entidad.Estado
                    };

                    bd.Factura.Add(factura);
                    bd.SaveChanges();

                    return Ok("Factura agregada exitosamente.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al registrar factura: " + ex.Message);
                return InternalServerError();
            }
        }

        [HttpPut]
        [Route("api/ActualizarFactura")]
        public IHttpActionResult ActualizarFactura(FacturaEnt entidad)
        {
            try
            {
                if (entidad == null)
                {
                    return BadRequest("El objeto factura no puede ser nulo.");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                using (var bd = new Tienda_G6Entities1())
                {
                    var factura = (from x in bd.Factura
                                   where x.IdFactura == entidad.IdFactura
                                   select x).FirstOrDefault();

                    if (factura != null)
                    {
                        factura.IdCliente = entidad.IdCliente;
                        factura.Fecha = entidad.Fecha;
                        factura.Total = entidad.Total;
                        factura.Estado = entidad.Estado;
                        bd.SaveChanges();
                        return Ok("Factura actualizada con éxito.");
                    }

                    return BadRequest($"Factura con ID '{entidad.IdFactura}' no encontrada en la base de datos.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al actualizar factura: " + ex.Message);
                return InternalServerError();
            }
        }

        [HttpDelete]
        [Route("api/EliminarFactura")]
        public IHttpActionResult EliminarFactura(long q)
        {
            try
            {
                using (var bd = new Tienda_G6Entities1())
                {
                    var factura = (from x in bd.Factura
                                   where x.IdFactura == q
                                   select x).FirstOrDefault();

                    if (factura != null)
                    {
                        bd.Factura.Remove(factura);
                        bd.SaveChanges();
                        return Ok("Factura eliminada con éxito.");
                    }

                    return BadRequest($"Factura con ID '{q}' no encontrada en la base de datos.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al eliminar factura: " + ex.Message);
                return InternalServerError();
            }
        }
    }

}
