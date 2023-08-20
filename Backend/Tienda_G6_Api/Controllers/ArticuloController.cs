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
    public class ArticuloController : ApiController
    {
        [HttpGet]
        [Route("api/ConsultarArticulos")]
        public IHttpActionResult ConsultarArticulos()
        {
            try
            {
                using (var bd = new Tienda_G6Entities1())
                {
                    var articulos = (from x in bd.Articulo
                                     select x).ToList();

                    if (articulos.Count > 0)
                    {
                        List<ArticuloEnt> res = new List<ArticuloEnt>();
                        foreach (var item in articulos)
                        {
                            res.Add(new ArticuloEnt
                            {
                                IdArticulo = item.IdArticulo,
                                IdCategoria = item.IdCategoria,
                                Descripcion = item.Descripcion,
                                Detalle = item.Detalle,
                                Existencia = item.Existencia,
                                Precio = item.Precio,
                                Estado = item.Estado
                            });
                        }

                        return Ok(res);
                    }

                    return BadRequest("No se encontraron artículos en la base de datos.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al Consultar artículos: " + ex.Message);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("api/ConsultarArticulo")]
        public IHttpActionResult ConsultarArticulo(long q)
        {
            try
            {
                using (var bd = new Tienda_G6Entities1())
                {
                    var articulo = (from x in bd.Articulo
                                    where x.IdArticulo == q
                                    select x).FirstOrDefault();

                    if (articulo != null)
                    {
                        return Ok(new ArticuloEnt
                        {
                            IdArticulo = articulo.IdArticulo,
                            IdCategoria = articulo.IdCategoria,
                            Descripcion = articulo.Descripcion,
                            Detalle = articulo.Detalle,
                            Existencia = articulo.Existencia,
                            Precio = articulo.Precio,
                            Estado = articulo.Estado
                        });
                    }

                    return BadRequest($"Artículo con ID '{q}' no encontrado en la base de datos.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al consultar artículo: " + ex.Message);
                return InternalServerError();
            }
        }

        [HttpPost]
        [Route("api/AgregarArticulo")]
        public IHttpActionResult AgregarArticulo(ArticuloEnt entidad)
        {
            try
            {
                if (entidad == null)
                {
                    return BadRequest("El objeto artículo no puede ser nulo.");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                using (var bd = new Tienda_G6Entities1())
                {
                    var articulo = new Articulo
                    {
                        IdCategoria = entidad.IdCategoria,
                        Descripcion = entidad.Descripcion,
                        Detalle = entidad.Detalle,
                        Existencia = entidad.Existencia,
                        Precio = entidad.Precio,
                        Estado = entidad.Estado
                    };

                    bd.Articulo.Add(articulo);
                    bd.SaveChanges();

                    return Ok("Artículo agregado exitosamente.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al registrar artículo: " + ex.Message);
                return InternalServerError();
            }
        }

        [HttpPut]
        [Route("api/ActualizarArticulo")]
        public IHttpActionResult ActualizarArticulo(ArticuloEnt entidad)
        {
            try
            {
                if (entidad == null)
                {
                    return BadRequest("El objeto artículo no puede ser nulo.");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                using (var bd = new Tienda_G6Entities1())
                {
                    var articulo = (from x in bd.Articulo
                                    where x.IdArticulo == entidad.IdArticulo
                                    select x).FirstOrDefault();

                    if (articulo != null)
                    {
                        articulo.IdCategoria = entidad.IdCategoria;
                        articulo.Descripcion = entidad.Descripcion;
                        articulo.Detalle = entidad.Detalle;
                        articulo.Existencia = entidad.Existencia;
                        articulo.Precio = entidad.Precio;
                        articulo.Estado = entidad.Estado;
                        bd.SaveChanges();
                        return Ok("Artículo actualizado con éxito.");
                    }

                    return BadRequest($"Artículo con ID '{entidad.IdArticulo}' no encontrado en la base de datos.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al actualizar artículo: " + ex.Message);
                return InternalServerError();
            }
        }

        [HttpDelete]
        [Route("api/EliminarArticulo")]
        public IHttpActionResult EliminarArticulo(long q)
        {
            try
            {
                using (var bd = new Tienda_G6Entities1())
                {
                    var articulo = (from x in bd.Articulo
                                    where x.IdArticulo == q
                                    select x).FirstOrDefault();

                    if (articulo != null)
                    {
                        bd.Articulo.Remove(articulo);
                        bd.SaveChanges();
                        return Ok("Artículo eliminado con éxito.");
                    }

                    return BadRequest($"Artículo con ID '{q}' no encontrado en la base de datos.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al eliminar artículo: " + ex.Message);
                return InternalServerError();
            }
        }
    }

}
