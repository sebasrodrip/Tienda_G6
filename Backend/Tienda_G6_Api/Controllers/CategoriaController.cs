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
    public class CategoriaController : ApiController
    {
        [HttpGet]
        [Route("api/ConsultarCategorias")]
        public IHttpActionResult ConsultarCategorias()
        {
            try
            {
                using (var bd = new Tienda_G6Entities1())
                {
                    var categorias = (from x in bd.Categoria
                                      select x).ToList();

                    if (categorias.Count > 0)
                    {
                        List<CategoriaEnt> res = new List<CategoriaEnt>();
                        foreach (var item in categorias)
                        {
                            res.Add(new CategoriaEnt
                            {
                                IdCategoria = item.IdCategoria,
                                Descripcion = item.Descripcion,
                                Estado = item.Estado
                            });
                        }

                        return Ok(res);
                    }

                    return BadRequest("No se encontraron categorías en la base de datos.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al Consultar categorías: " + ex.Message);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("api/ConsultarCategoria")]
        public IHttpActionResult ConsultarCategoria(long q)
        {
            try
            {
                using (var bd = new Tienda_G6Entities1())
                {
                    var categoria = (from x in bd.Categoria
                                     where x.IdCategoria == q
                                     select x).FirstOrDefault();

                    if (categoria != null)
                    {
                        return Ok(new CategoriaEnt
                        {
                            IdCategoria = categoria.IdCategoria,
                            Descripcion = categoria.Descripcion,
                            Estado = categoria.Estado
                        });
                    }

                    return BadRequest($"Categoría con ID '{q}' no encontrada en la base de datos.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al consultar categoría: " + ex.Message);
                return InternalServerError();
            }
        }

        [HttpPost]
        [Route("api/AgregarCategoria")]
        public IHttpActionResult AgregarCategoria(CategoriaEnt entidad)
        {
            try
            {
                if (entidad == null)
                {
                    return BadRequest("El objeto categoría no puede ser nulo.");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                using (var bd = new Tienda_G6Entities1())
                {
                    // Verificar si la descripción de categoría ya existe en la base de datos
                    if (bd.Categoria.Any(c => c.Descripcion == entidad.Descripcion))
                    {
                        ModelState.AddModelError("Descripcion", "La descripción de categoría ya existe en la base de datos.");
                        return BadRequest(ModelState);
                    }

                    var categoria = new Categoria
                    {
                        Descripcion = entidad.Descripcion,
                        Estado = entidad.Estado
                    };

                    bd.Categoria.Add(categoria);
                    bd.SaveChanges();

                    return Ok("Categoría agregada exitosamente.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al registrar categoría: " + ex.Message);
                return InternalServerError();
            }
        }

        [HttpPut]
        [Route("api/ActualizarCategoria")]
        public IHttpActionResult ActualizarCategoria(CategoriaEnt entidad)
        {
            try
            {
                if (entidad == null)
                {
                    return BadRequest("El objeto categoría no puede ser nulo.");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                using (var bd = new Tienda_G6Entities1())
                {
                    // Verificar si la descripción de categoría ya existe en la base de datos (excepto la categoría actual)
                    if (bd.Categoria.Any(c => c.Descripcion == entidad.Descripcion && c.IdCategoria != entidad.IdCategoria))
                    {
                        ModelState.AddModelError("Descripcion", "La descripción de categoría ya existe en la base de datos.");
                        return BadRequest(ModelState);
                    }

                    var categoria = (from x in bd.Categoria
                                     where x.IdCategoria == entidad.IdCategoria
                                     select x).FirstOrDefault();

                    if (categoria != null)
                    {
                        categoria.Descripcion = entidad.Descripcion;
                        categoria.Estado = entidad.Estado;
                        bd.SaveChanges();
                        return Ok("Categoría actualizada con éxito.");
                    }

                    return BadRequest($"Categoría con ID '{entidad.IdCategoria}' no encontrada en la base de datos.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al actualizar categoría: " + ex.Message);
                return InternalServerError();
            }
        }

        [HttpDelete]
        [Route("api/EliminarCategoria")]
        public IHttpActionResult EliminarCategoria(long q)
        {
            try
            {
                using (var bd = new Tienda_G6Entities1())
                {
                    var categoria = (from x in bd.Categoria
                                     where x.IdCategoria == q
                                     select x).FirstOrDefault();

                    if (categoria != null)
                    {
                        bd.Categoria.Remove(categoria);
                        bd.SaveChanges();
                        return Ok("Categoría eliminada con éxito.");
                    }

                    return BadRequest($"Categoría con ID '{q}' no encontrada en la base de datos.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al eliminar categoría: " + ex.Message);
                return InternalServerError();
            }
        }
    }

}
