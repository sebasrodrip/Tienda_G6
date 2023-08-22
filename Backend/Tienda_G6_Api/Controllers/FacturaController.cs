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
        public HttpResponseMessage ConsultarFactura(long q)
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
                        var successResponse = new
                        {
                            mensaje = "Consulta de factura exitosa.",
                            data = new FacturaEnt
                            {
                                IdFactura = factura.IdFactura,
                                IdCliente = factura.IdCliente,
                                Fecha = factura.Fecha,
                                Total = factura.Total,
                                Estado = factura.Estado
                            }
                        };

                        return Request.CreateResponse(HttpStatusCode.OK, successResponse);
                    }

                    var notFoundResponse = new
                    {
                        mensaje = $"Factura con ID '{q}' no encontrada en la base de datos."
                    };

                    return Request.CreateResponse(HttpStatusCode.BadRequest, notFoundResponse);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al consultar factura: " + ex.Message);
                var errorResponse = new
                {
                    mensaje = "Error interno del servidor."
                };
                return Request.CreateResponse(HttpStatusCode.InternalServerError, errorResponse);
            }
        }

        [HttpPost]
        [Route("api/AgregarFactura")]
        public HttpResponseMessage AgregarFactura(FacturaEnt entidad)
        {
            try
            {
                if (entidad == null)
                {
                    var errorResponse = new
                    {
                        mensaje = "El objeto factura no puede ser nulo."
                    };

                    return Request.CreateResponse(HttpStatusCode.BadRequest, errorResponse);
                }

                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors)
                                                   .Select(e => e.ErrorMessage)
                                                   .ToList();

                    var errorResponse = new
                    {
                        mensaje = "Error de validación en el objeto factura.",
                        errores = errors
                    };

                    return Request.CreateResponse(HttpStatusCode.BadRequest, errorResponse);
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

                    var successResponse = new
                    {
                        mensaje = "Factura agregada exitosamente."
                    };

                    return Request.CreateResponse(HttpStatusCode.OK, successResponse);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al registrar factura: " + ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { mensaje = "Error interno del servidor." });
            }
        }

        [HttpPut]
        [Route("api/ActualizarFactura")]
        public HttpResponseMessage ActualizarFactura(FacturaEnt entidad)
        {
            try
            {
                if (entidad == null)
                {
                    var badRequestResponse = new
                    {
                        mensaje = "El objeto factura no puede ser nulo."
                    };
                    return Request.CreateResponse(HttpStatusCode.BadRequest, badRequestResponse);
                }

                if (!ModelState.IsValid)
                {
                    var errorResponse = new
                    {
                        mensaje = "Error de validación en el objeto factura.",
                        errores = ModelState.Values.SelectMany(v => v.Errors)
                                                   .Select(e => e.ErrorMessage)
                                                   .ToList()
                    };

                    return Request.CreateResponse(HttpStatusCode.BadRequest, errorResponse);
                }

                using (var bd = new Tienda_G6Entities1())
                {
                    var factura = bd.Factura.FirstOrDefault(f => f.IdFactura == entidad.IdFactura);

                    if (factura != null)
                    {
                        factura.IdCliente = entidad.IdCliente;
                        factura.Fecha = entidad.Fecha;
                        factura.Total = entidad.Total;
                        factura.Estado = entidad.Estado;
                        bd.SaveChanges();

                        var successResponse = new
                        {
                            mensaje = "Factura actualizada con éxito."
                        };

                        return Request.CreateResponse(HttpStatusCode.OK, successResponse);
                    }

                    var notFoundResponse = new
                    {
                        mensaje = $"Factura con ID '{entidad.IdFactura}' no encontrada en la base de datos."
                    };

                    return Request.CreateResponse(HttpStatusCode.BadRequest, notFoundResponse);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al actualizar factura: " + ex.Message);
                var errorResponse = new
                {
                    mensaje = "Error interno del servidor."
                };
                return Request.CreateResponse(HttpStatusCode.InternalServerError, errorResponse);
            }
        }

        [HttpDelete]
        [Route("api/EliminarFactura")]
        public HttpResponseMessage EliminarFactura(long q)
        {
            try
            {
                using (var bd = new Tienda_G6Entities1())
                {
                    var factura = bd.Factura.FirstOrDefault(f => f.IdFactura == q);

                    if (factura != null)
                    {
                        bd.Factura.Remove(factura);
                        bd.SaveChanges();

                        var successResponse = new
                        {
                            mensaje = "Factura eliminada con éxito."
                        };

                        return Request.CreateResponse(HttpStatusCode.OK, successResponse);
                    }

                    var notFoundResponse = new
                    {
                        mensaje = $"Factura con ID '{q}' no encontrada en la base de datos."
                    };

                    return Request.CreateResponse(HttpStatusCode.BadRequest, notFoundResponse);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al eliminar factura: " + ex.Message);
                var errorResponse = new
                {
                    mensaje = "Error interno del servidor."
                };
                return Request.CreateResponse(HttpStatusCode.InternalServerError, errorResponse);
            }
        }
    }

}
