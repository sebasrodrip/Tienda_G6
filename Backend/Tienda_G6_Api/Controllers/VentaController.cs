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
        public IHttpActionResult ConsultarVenta(long q)
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
                        return Ok(new VentaEnt
                        {
                            IdVenta = venta.IdVenta,
                            IdFactura = venta.IdFactura,
                            IdArticulo = venta.IdArticulo,
                            Precio = venta.Precio,
                            Cantidad = venta.Cantidad
                        });
                    }

                    return BadRequest($"Venta con ID '{q}' no encontrada en la base de datos.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al consultar venta: " + ex.Message);
                return InternalServerError();
            }
        }

        [HttpPost]
        [Route("api/AgregarVenta")]
        public IHttpActionResult AgregarVenta(VentaEnt entidad)
        {
            try
            {
                if (entidad == null)
                {
                    return BadRequest("El objeto venta no puede ser nulo.");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
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

                    return Ok("Venta agregada exitosamente.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al registrar venta: " + ex.Message);
                return InternalServerError();
            }
        }

        [HttpPut]
        [Route("api/ActualizarVenta")]
        public IHttpActionResult ActualizarVenta(VentaEnt entidad)
        {
            try
            {
                if (entidad == null)
                {
                    return BadRequest("El objeto venta no puede ser nulo.");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                using (var bd = new Tienda_G6Entities1())
                {
                    var venta = (from x in bd.Venta
                                 where x.IdVenta == entidad.IdVenta
                                 select x).FirstOrDefault();

                    if (venta != null)
                    {
                        venta.IdFactura = entidad.IdFactura;
                        venta.IdArticulo = entidad.IdArticulo;
                        venta.Precio = entidad.Precio;
                        venta.Cantidad = entidad.Cantidad;
                        bd.SaveChanges();
                        return Ok("Venta actualizada con éxito.");
                    }

                    return BadRequest($"Venta con ID '{entidad.IdVenta}' no encontrada en la base de datos.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al actualizar venta: " + ex.Message);
                return InternalServerError();
            }
        }

        [HttpDelete]
        [Route("api/EliminarVenta")]
        public IHttpActionResult EliminarVenta(long q)
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
                        bd.Venta.Remove(venta);
                        bd.SaveChanges();
                        return Ok("Venta eliminada con éxito.");
                    }

                    return BadRequest($"Venta con ID '{q}' no encontrada en la base de datos.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al eliminar venta: " + ex.Message);
                return InternalServerError();
            }
        }
    }

}
