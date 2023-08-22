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
                                Precio = item.Precio,
                                Existencia = item.Existencia,
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
        public HttpResponseMessage ConsultarArticulo(long q)
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
                        var successResponse = new
                        {
                            mensaje = "Consulta de artículo exitosa.",
                            data = new ArticuloEnt
                            {
                                IdArticulo = articulo.IdArticulo,
                                IdCategoria = articulo.IdCategoria,
                                Descripcion = articulo.Descripcion,
                                Detalle = articulo.Detalle,
                                Precio = articulo.Precio,
                                Existencia = articulo.Existencia,
                                Estado = articulo.Estado
                            }
                        };

                        return Request.CreateResponse(HttpStatusCode.OK, successResponse);
                    }

                    var notFoundResponse = new
                    {
                        mensaje = $"Artículo con ID '{q}' no encontrado en la base de datos."
                    };

                    return Request.CreateResponse(HttpStatusCode.BadRequest, notFoundResponse);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al consultar artículo: " + ex.Message);
                var errorResponse = new
                {
                    mensaje = "Error interno del servidor."
                };
                return Request.CreateResponse(HttpStatusCode.InternalServerError, errorResponse);
            }
        }

        [HttpPost]
        [Route("api/AgregarArticulo")]
        public HttpResponseMessage AgregarArticulo(ArticuloEnt entidad)
        {
            try
            {
                if (entidad == null)
                {
                    var errorResponse = new
                    {
                        mensaje = "El objeto artículo no puede ser nulo."
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
                        mensaje = "Error de validación en el objeto artículo.",
                        errores = errors
                    };

                    return Request.CreateResponse(HttpStatusCode.BadRequest, errorResponse);
                }

                using (var bd = new Tienda_G6Entities1())
                {
                    if (bd.Articulo.Any(a => a.Descripcion == entidad.Descripcion))
                    {
                        ModelState.AddModelError("Descripcion", "La descripción de artículo ya existe en la base de datos.");
                        var validationErrors = ModelState.Values.SelectMany(v => v.Errors)
                                                                .Select(e => e.ErrorMessage)
                                                                .ToList();
                        var errorResponse = new
                        {
                            mensaje = "Error al agregar artículo.",
                            errores = validationErrors
                        };

                        return Request.CreateResponse(HttpStatusCode.BadRequest, errorResponse);
                    }

                    var articulo = new Articulo
                    {
                        IdCategoria = entidad.IdCategoria,
                        Descripcion = entidad.Descripcion,
                        Detalle = entidad.Detalle,
                        Precio = entidad.Precio,
                        Existencia = entidad.Existencia,
                        Estado = entidad.Estado
                    };

                    bd.Articulo.Add(articulo);
                    bd.SaveChanges();

                    var successResponse = new
                    {
                        mensaje = "Artículo agregado exitosamente."
                    };

                    return Request.CreateResponse(HttpStatusCode.OK, successResponse);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al registrar artículo: " + ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { mensaje = "Error interno del servidor." });
            }
        }

        [HttpPut]
        [Route("api/ActualizarArticulo")]
        public HttpResponseMessage ActualizarArticulo(ArticuloEnt entidad)
        {
            try
            {
                if (entidad == null)
                {
                    var badRequestResponse = new
                    {
                        mensaje = "El objeto artículo no puede ser nulo."
                    };
                    return Request.CreateResponse(HttpStatusCode.BadRequest, badRequestResponse);
                }

                if (!ModelState.IsValid)
                {
                    var errorResponse = new
                    {
                        mensaje = "Error de validación en el objeto artículo.",
                        errores = ModelState.Values.SelectMany(v => v.Errors)
                                                   .Select(e => e.ErrorMessage)
                                                   .ToList()
                    };

                    return Request.CreateResponse(HttpStatusCode.BadRequest, errorResponse);
                }

                using (var bd = new Tienda_G6Entities1())
                {
                    // Verificar si la descripción de artículo ya existe en la base de datos (sin importar el IdArticulo)
                    var descripcionExistente = bd.Articulo.FirstOrDefault(a => a.Descripcion == entidad.Descripcion);

                    if (descripcionExistente != null)
                    {
                        // Si la descripción existe y no pertenece al registro actual, mostrar un error.
                        if (descripcionExistente.IdArticulo != entidad.IdArticulo)
                        {
                            ModelState.AddModelError("Descripcion", "La descripción de artículo ya existe en la base de datos.");
                            var validationErrors = ModelState.Values.SelectMany(v => v.Errors)
                                                                    .Select(e => e.ErrorMessage)
                                                                    .ToList();
                            var errorResponse = new
                            {
                                mensaje = "Error al agregar artículo.",
                                errores = validationErrors
                            };

                            return Request.CreateResponse(HttpStatusCode.BadRequest, errorResponse);
                        }
                    }

                    var articulo = bd.Articulo.FirstOrDefault(a => a.IdArticulo == entidad.IdArticulo);

                    if (articulo != null)
                    {
                        articulo.IdCategoria = entidad.IdCategoria;
                        articulo.Descripcion = entidad.Descripcion;
                        articulo.Detalle = entidad.Detalle;
                        articulo.Precio = entidad.Precio;
                        articulo.Existencia = entidad.Existencia;
                        articulo.Estado = entidad.Estado;
                        bd.SaveChanges();

                        var successResponse = new
                        {
                            mensaje = "Artículo actualizado con éxito."
                        };

                        return Request.CreateResponse(HttpStatusCode.OK, successResponse);
                    }

                    var notFoundResponse = new
                    {
                        mensaje = $"Artículo con ID '{entidad.IdArticulo}' no encontrado en la base de datos."
                    };

                    return Request.CreateResponse(HttpStatusCode.BadRequest, notFoundResponse);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al actualizar artículo: " + ex.Message);
                var errorResponse = new
                {
                    mensaje = "Error interno del servidor."
                };
                return Request.CreateResponse(HttpStatusCode.InternalServerError, errorResponse);
            }
        }

        [HttpDelete]
        [Route("api/EliminarArticulo")]
        public HttpResponseMessage EliminarArticulo(long q)
        {
            try
            {
                using (var bd = new Tienda_G6Entities1())
                {
                    var articulo = bd.Articulo.FirstOrDefault(a => a.IdArticulo == q);

                    if (articulo != null)
                    {
                        bd.Articulo.Remove(articulo);
                        bd.SaveChanges();

                        var successResponse = new
                        {
                            mensaje = "Artículo eliminado con éxito."
                        };

                        return Request.CreateResponse(HttpStatusCode.OK, successResponse);
                    }

                    var notFoundResponse = new
                    {
                        mensaje = $"Artículo con ID '{q}' no encontrado en la base de datos."
                    };

                    return Request.CreateResponse(HttpStatusCode.BadRequest, notFoundResponse);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al eliminar artículo: " + ex.Message);
                var errorResponse = new
                {
                    mensaje = "Error interno del servidor."
                };
                return Request.CreateResponse(HttpStatusCode.InternalServerError, errorResponse);
            }
        }
    }

}
