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
    public class CarritoController : ApiController
    {
        [HttpGet]
        [Route("api/ConsultarCarrito")]
        public List<CarritoEnt> ConsultarCarrito(long q)
        {
            using (var bd = new Tienda_G6Entities1())
            {
                var datos = (from x in bd.Carrito
                             join y in bd.Articulo on x.IdArticulo equals y.IdArticulo
                             where x.IdCliente == q
                             select new
                             {
                                 x.IdCarrito,
                                 x.IdArticulo,
                                 x.IdCliente,
                                 y.Precio,
                                 y.Descripcion,
                                 y.Detalle
                             }).ToList();

                if (datos.Count > 0)
                {
                    List<CarritoEnt> res = new List<CarritoEnt>();
                    foreach (var item in datos)
                    {
                        res.Add(new CarritoEnt
                        {
                            IdCarrito = item.IdCarrito,
                            IdArticulo = item.IdArticulo,
                            IdCliente = item.IdCliente,
                            Precio = item.Precio,
                            Descripcion = item.Descripcion,
                            Detalle = item.Detalle
                        });
                    }

                    return res;
                }

                return new List<CarritoEnt>();
            }
        }

        [HttpGet]
        [Route("api/ConsultarArticuloCliente")]
        public List<CarritoEnt> ConsultarArticuloCliente(long q)
        {
            using (var bd = new Tienda_G6Entities1())
            {
                var datos = (from x in bd.Cliente
                             join y in bd.Articulo on x.IdArticulo equals y.IdArticulo
                             where x.IdCliente == q
                             select new
                             {
                                 x.IdCliente,
                                 x.IdArticulo,
                                 x.IdUsuario,
                                 x.PrecioPago,
                                 y.Descripcion,
                                 y.Detalle
                             }).ToList();

                if (datos.Count > 0)
                {
                    List<CarritoEnt> res = new List<CarritoEnt>();
                    foreach (var item in datos)
                    {
                        res.Add(new CarritoEnt
                        {
                            IdCarrito = item.IdCliente,
                            IdArticulo = item.IdArticulo,
                            IdCliente = item.IdCliente,
                            Precio = item.PrecioPago,
                            Descripcion = item.Descripcion,
                            Detalle = item.Detalle
                        });
                    }

                    return res;
                }

                return new List<CarritoEnt>();
            }
        }

        [HttpPost]
        [Route("api/AgregarCarrito")]
        public int AgregarCarrito(CarritoEnt entidad)
        {
            using (var bd = new Tienda_G6Entities1())
            {
                var carrito = (from cc in bd.Carrito
                               where cc.IdArticulo == entidad.IdArticulo
                                  && cc.IdCliente == entidad.IdCliente
                               select cc).ToList();

                var pagado = (from cu in bd.Cliente
                              where cu.IdArticulo == entidad.IdArticulo
                                 && cu.IdCliente == entidad.IdCliente
                              select cu).ToList();

                if (carrito.Count > 0 || pagado.Count > 0)
                {
                    return 0;
                }

                Carrito tabla = new Carrito();
                tabla.IdCliente = entidad.IdCliente;
                tabla.IdArticulo = entidad.IdArticulo;
                tabla.Fecha = entidad.Fecha;

                bd.Carrito.Add(tabla);
                return bd.SaveChanges();
            }
        }

        [HttpDelete]
        [Route("api/RemoverCarrito")]
        public int RemoverCarrito(long q)
        {
            using (var bd = new Tienda_G6Entities1())
            {
                var carrito = (from cc in bd.Carrito
                               where cc.IdCarrito == q
                               select cc).FirstOrDefault();

                if (carrito != null)
                {
                    bd.Carrito.Remove(carrito);
                    return bd.SaveChanges();
                }

                return 0;
            }
        }

        [HttpPost]
        [Route("api/PagarArticuloCarrito")]
        public int PagarCursosCarrito(CarritoEnt entidad)
        {
            using (var bd = new Tienda_G6Entities1())
            {
                //Busco el carrito para pasarlo a la tabla de usuarios
                var datos = (from cc in bd.Carrito
                             join c in bd.Articulo on cc.IdArticulo equals c.IdArticulo
                             where cc.IdCliente == entidad.IdCliente
                             select new
                             {
                                 cc.IdArticulo,
                                 cc.IdCliente,
                                 c.Precio
                             }).ToList();

                if (datos.Count > 0)
                {
                    foreach (var item in datos)
                    {
                        Cliente cu = new Cliente();
                        cu.IdArticulo = item.IdArticulo;
                        cu.IdCliente = item.IdCliente;
                        cu.FechaPago = DateTime.Now;
                        cu.PrecioPago = (Decimal)item.Precio;
                        bd.Cliente.Add(cu);
                    }

                    //Busco el carrito para limpiarlo
                    var carritoActual = (from cc in bd.Carrito
                                         where cc.IdCliente == entidad.IdCliente
                                         select cc).ToList();

                    foreach (var item in carritoActual)
                    {
                        bd.Carrito.Remove(item);
                    }

                    return bd.SaveChanges();
                }

                return 0;
            }
        }

    }
}
