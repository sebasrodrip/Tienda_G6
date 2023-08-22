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
    public class VentaController : ApiController
    {
        [HttpGet]
        [Route("api/ConsultarVentas")]
        public IHttpActionResult ConsultarVentas()
        {
            try
            {
                using (var bd = new Tienda_G6Entities1())
                {
                    var ventas = (from x in bd.Venta
                                  select x).ToList();

                    if (ventas.Count > 0)
                    {
                        List<VentaEnt> res = new List<VentaEnt>();
                        foreach (var item in ventas)
                        {
                            res.Add(new VentaEnt
                            {
                                IdVenta = item.IdVenta,
                                IdFactura = item.IdFactura,
                                IdArticulo = item.IdArticulo,
                                Precio = item.Precio,
                                Cantidad = item.Cantidad
                            });
                        }

                        return Ok(res);
                    }

                    return BadRequest("No se encontraron ventas en la base de datos.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al Consultar ventas: " + ex.Message);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("api/ConsultarVenta")]
        public HttpResponseMessage ConsultarVenta(long q)
        {
            try
            {
                using (var bd = new Tienda_G6Entities1())
                {
                    var venta = (from x in bd.Venta
                                 where x.IdVenta == q
                                 select x).FirstOrDefault();

                    if (venta != null)
                    {
                        var successResponse = new
                        {
                            mensaje = "Consulta de venta exitosa.",
                            data = new VentaEnt
                            {
                                IdVenta = venta.IdVenta,
                                IdFactura = venta.IdFactura,
                                IdArticulo = venta.IdArticulo,
                                Precio = venta.Precio,
                                Cantidad = venta.Cantidad
                            }
                        };

                        return Request.CreateResponse(HttpStatusCode.OK, successResponse);
                    }

                    var notFoundResponse = new
                    {
                        mensaje = $"Venta con ID '{q}' no encontrada en la base de datos."
                    };

                    return Request.CreateResponse(HttpStatusCode.BadRequest, notFoundResponse);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al consultar venta: " + ex.Message);
                var errorResponse = new
                {
                    mensaje = "Error interno del servidor."
                };
                return Request.CreateResponse(HttpStatusCode.InternalServerError, errorResponse);
            }
        }

        [HttpPost]
        [Route("api/AgregarVenta")]
        public HttpResponseMessage AgregarVenta(VentaEnt entidad)
        {
            try
            {
                if (entidad == null)
                {
                    var errorResponse = new
                    {
                        mensaje = "El objeto venta no puede ser nulo."
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
                        mensaje = "Error de validación en el objeto venta.",
                        errores = errors
                    };

                    return Request.CreateResponse(HttpStatusCode.BadRequest, errorResponse);
                }

                using (var bd = new Tienda_G6Entities1())
                {
                    var venta = new Venta
                    {
                        IdFactura = entidad.IdFactura,
                        IdArticulo = entidad.IdArticulo,
                        Precio = entidad.Precio,
                        Cantidad = entidad.Cantidad
                    };

                    bd.Venta.Add(venta);
                    bd.SaveChanges();

                    var successResponse = new
                    {
                        mensaje = "Venta agregada exitosamente."
                    };

                    return Request.CreateResponse(HttpStatusCode.OK, successResponse);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al registrar venta: " + ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { mensaje = "Error interno del servidor." });
            }
        }

        [HttpPut]
        [Route("api/ActualizarVenta")]
        public HttpResponseMessage ActualizarVenta(VentaEnt entidad)
        {
            try
            {
                if (entidad == null)
                {
                    var badRequestResponse = new
                    {
                        mensaje = "El objeto venta no puede ser nulo."
                    };
                    return Request.CreateResponse(HttpStatusCode.BadRequest, badRequestResponse);
                }

                if (!ModelState.IsValid)
                {
                    var errorResponse = new
                    {
                        mensaje = "Error de validación en el objeto venta.",
                        errores = ModelState.Values.SelectMany(v => v.Errors)
                                                   .Select(e => e.ErrorMessage)
                                                   .ToList()
                    };

                    return Request.CreateResponse(HttpStatusCode.BadRequest, errorResponse);
                }

                using (var bd = new Tienda_G6Entities1())
                {
                    var venta = bd.Venta.FirstOrDefault(v => v.IdVenta == entidad.IdVenta);

                    if (venta != null)
                    {
                        venta.IdFactura = entidad.IdFactura;
                        venta.IdArticulo = entidad.IdArticulo;
                        venta.Precio = entidad.Precio;
                        venta.Cantidad = entidad.Cantidad;
                        bd.SaveChanges();

                        var successResponse = new
                        {
                            mensaje = "Venta actualizada con éxito."
                        };

                        return Request.CreateResponse(HttpStatusCode.OK, successResponse);
                    }

                    var notFoundResponse = new
                    {
                        mensaje = $"Venta con ID '{entidad.IdVenta}' no encontrada en la base de datos."
                    };

                    return Request.CreateResponse(HttpStatusCode.BadRequest, notFoundResponse);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al actualizar venta: " + ex.Message);
                var errorResponse = new
                {
                    mensaje = "Error interno del servidor."
                };
                return Request.CreateResponse(HttpStatusCode.InternalServerError, errorResponse);
            }
        }

        [HttpDelete]
        [Route("api/EliminarVenta")]
        public HttpResponseMessage EliminarVenta(long q)
        {
            try
            {
                using (var bd = new Tienda_G6Entities1())
                {
                    var venta = bd.Venta.FirstOrDefault(v => v.IdVenta == q);

                    if (venta != null)
                    {
                        bd.Venta.Remove(venta);
                        bd.SaveChanges();

                        var successResponse = new
                        {
                            mensaje = "Venta eliminada con éxito."
                        };

                        return Request.CreateResponse(HttpStatusCode.OK, successResponse);
                    }

                    var notFoundResponse = new
                    {
                        mensaje = $"Venta con ID '{q}' no encontrada en la base de datos."
                    };

                    return Request.CreateResponse(HttpStatusCode.BadRequest, notFoundResponse);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al eliminar venta: " + ex.Message);
                var errorResponse = new
                {
                    mensaje = "Error interno del servidor."
                };
                return Request.CreateResponse(HttpStatusCode.InternalServerError, errorResponse);
            }
        }
    }

}
