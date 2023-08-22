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
        public HttpResponseMessage ConsultarCategoria(long q)
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
                        var successResponse = new
                        {
                            mensaje = "Consulta de categoría exitosa.",
                            data = new CategoriaEnt
                            {
                                IdCategoria = categoria.IdCategoria,
                                Descripcion = categoria.Descripcion,
                                Estado = categoria.Estado
                            }
                        };

                        return Request.CreateResponse(HttpStatusCode.OK, successResponse);
                    }

                    var notFoundResponse = new
                    {
                        mensaje = $"Categoría con ID '{q}' no encontrada en la base de datos."
                    };

                    return Request.CreateResponse(HttpStatusCode.BadRequest, notFoundResponse);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al consultar categoría: " + ex.Message);
                var errorResponse = new
                {
                    mensaje = "Error interno del servidor."
                };
                return Request.CreateResponse(HttpStatusCode.InternalServerError, errorResponse);
            }
        }

        [HttpPost]
        [Route("api/AgregarCategoria")]
        public HttpResponseMessage AgregarCategoria(CategoriaEnt entidad)
        {
            try
            {
                if (entidad == null)
                {
                    var errorResponse = new
                    {
                        mensaje = "El objeto categoría no puede ser nulo."
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
                        mensaje = "Error de validación en el objeto categoría.",
                        errores = errors
                    };

                    return Request.CreateResponse(HttpStatusCode.BadRequest, errorResponse);
                }

                using (var bd = new Tienda_G6Entities1())
                {
                    if (bd.Categoria.Any(c => c.Descripcion == entidad.Descripcion))
                    {
                        ModelState.AddModelError("Descripcion", "La descripción de categoría ya existe en la base de datos.");
                        var validationErrors = ModelState.Values.SelectMany(v => v.Errors)
                                                                .Select(e => e.ErrorMessage)
                                                                .ToList();
                        var errorResponse = new
                        {
                            mensaje = "Error al agregar categoría.",
                            errores = validationErrors
                        };

                        return Request.CreateResponse(HttpStatusCode.BadRequest, errorResponse);
                    }

                    var categoria = new Categoria
                    {
                        Descripcion = entidad.Descripcion,
                        Estado = entidad.Estado
                    };

                    bd.Categoria.Add(categoria);
                    bd.SaveChanges();

                    var successResponse = new
                    {
                        mensaje = "Categoría agregada exitosamente."
                    };

                    return Request.CreateResponse(HttpStatusCode.OK, successResponse);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al registrar categoría: " + ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { mensaje = "Error interno del servidor." });
            }
        }

        [HttpPut]
        [Route("api/ActualizarCategoria")]
        public HttpResponseMessage ActualizarCategoria(CategoriaEnt entidad)
        {
            try
            {
                if (entidad == null)
                {
                    var badRequestResponse = new
                    {
                        mensaje = "El objeto categoría no puede ser nulo."
                    };
                    return Request.CreateResponse(HttpStatusCode.BadRequest, badRequestResponse);
                }

                if (!ModelState.IsValid)
                {
                    var errorResponse = new
                    {
                        mensaje = "Error de validación en el objeto categoría.",
                        errores = ModelState.Values.SelectMany(v => v.Errors)
                                                   .Select(e => e.ErrorMessage)
                                                   .ToList()
                    };

                    return Request.CreateResponse(HttpStatusCode.BadRequest, errorResponse);
                }

                using (var bd = new Tienda_G6Entities1())
                {
                    // Verificar si la descripción de categoría ya existe en la base de datos (sin importar el IdCategoria)
                    var nombreExistente = bd.Categoria.FirstOrDefault(c => c.Descripcion == entidad.Descripcion);

                    if (nombreExistente != null)
                    {
                        // Si el nombre existe y no pertenece al registro actual, mostrar un error.
                        if (nombreExistente.IdCategoria != entidad.IdCategoria)
                        {
                            ModelState.AddModelError("Descripcion", "La descripción de categoría ya existe en la base de datos.");
                            var validationErrors = ModelState.Values.SelectMany(v => v.Errors)
                                                                    .Select(e => e.ErrorMessage)
                                                                    .ToList();
                            var errorResponse = new
                            {
                                mensaje = "Error al agregar categoría.",
                                errores = validationErrors
                            };

                            return Request.CreateResponse(HttpStatusCode.BadRequest, errorResponse);
                        }
                    }

                    var categoria = bd.Categoria.FirstOrDefault(c => c.IdCategoria == entidad.IdCategoria);

                    if (categoria != null)
                    {
                        categoria.Descripcion = entidad.Descripcion;
                        categoria.Estado = entidad.Estado;
                        bd.SaveChanges();

                        var successResponse = new
                        {
                            mensaje = "Categoría actualizada con éxito."
                        };

                        return Request.CreateResponse(HttpStatusCode.OK, successResponse);
                    }

                    var notFoundResponse = new
                    {
                        mensaje = $"Categoría con ID '{entidad.IdCategoria}' no encontrada en la base de datos."
                    };

                    return Request.CreateResponse(HttpStatusCode.BadRequest, notFoundResponse);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al actualizar categoría: " + ex.Message);
                var errorResponse = new
                {
                    mensaje = "Error interno del servidor."
                };
                return Request.CreateResponse(HttpStatusCode.InternalServerError, errorResponse);
            }
        }

        [HttpDelete]
        [Route("api/EliminarCategoria")]
        public HttpResponseMessage EliminarCategoria(long q)
        {
            try
            {
                using (var bd = new Tienda_G6Entities1())
                {
                    var categoria = bd.Categoria.FirstOrDefault(c => c.IdCategoria == q);

                    if (categoria != null)
                    {
                        bd.Categoria.Remove(categoria);
                        bd.SaveChanges();

                        var successResponse = new
                        {
                            mensaje = "Categoría eliminada con éxito."
                        };

                        return Request.CreateResponse(HttpStatusCode.OK, successResponse);
                    }

                    var notFoundResponse = new
                    {
                        mensaje = $"Categoría con ID '{q}' no encontrada en la base de datos."
                    };

                    return Request.CreateResponse(HttpStatusCode.BadRequest, notFoundResponse);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al eliminar categoría: " + ex.Message);
                var errorResponse = new
                {
                    mensaje = "Error interno del servidor."
                };
                return Request.CreateResponse(HttpStatusCode.InternalServerError, errorResponse);
            }
        }

    }

}
