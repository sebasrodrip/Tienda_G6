using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Tienda_G6_Api.Entities;
using Tienda_G6_Api.Models;

namespace Tienda_G6_Api.Controllers
{
    //[Authorize]
    public class RolController : ApiController
    {
        [HttpGet]
        [Route("api/ConsultarRoles")]
        public IHttpActionResult ConsultarRoles()
        {
            try
            {
                using (var bd = new Tienda_G6Entities1())
                {
                    var roles = (from x in bd.Rol
                                 select x).ToList();

                    if (roles.Count > 0)
                    {
                        List<RolEnt> res = new List<RolEnt>();
                        foreach (var item in roles)
                        {
                            res.Add(new RolEnt
                            {
                                IdRol = item.IdRol,
                                NombreRol = item.NombreRol
                            });
                        }

                        return Ok(res);
                    }

                    return BadRequest("No se encontraron roles en la base de datos.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al Consultar roles: " + ex.Message);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("api/ConsultarRol")]
        public HttpResponseMessage ConsultarRol(long q)
        {
            try
            {
                using (var bd = new Tienda_G6Entities1())
                {
                    var rol = (from x in bd.Rol
                               where x.IdRol == q
                               select x).FirstOrDefault();

                    if (rol != null)
                    {
                        var successResponse = new
                        {
                            mensaje = "Consulta de rol exitosa.",
                            data = new RolEnt
                            {
                                IdRol = rol.IdRol,
                                NombreRol = rol.NombreRol
                            }
                        };

                        return Request.CreateResponse(HttpStatusCode.OK, successResponse);
                    }

                    var notFoundResponse = new
                    {
                        mensaje = $"Rol con ID '{q}' no encontrado en la base de datos."
                    };

                    return Request.CreateResponse(HttpStatusCode.BadRequest, notFoundResponse);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al consultar rol: " + ex.Message);
                var errorResponse = new
                {
                    mensaje = "Error interno del servidor."
                };
                return Request.CreateResponse(HttpStatusCode.InternalServerError, errorResponse);
            }
        }

        [HttpPost]
        [Route("api/AgregarRol")]
        public HttpResponseMessage AgregarRol(RolEnt entidad)
        {
            try
            {
                if (entidad == null)
                {
                    var errorResponse = new
                    {
                        mensaje = "El objeto rol no puede ser nulo."
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
                        mensaje = "Error de validación en el objeto rol.",
                        errores = errors
                    };

                    return Request.CreateResponse(HttpStatusCode.BadRequest, errorResponse);
                }

                using (var bd = new Tienda_G6Entities1())
                {
                    if (bd.Rol.Any(r => r.NombreRol == entidad.NombreRol))
                    {
                        ModelState.AddModelError("NombreRol", "El nombre de rol ya existe en la base de datos.");
                        var validationErrors = ModelState.Values.SelectMany(v => v.Errors)
                                                                .Select(e => e.ErrorMessage)
                                                                .ToList();
                        var errorResponse = new
                        {
                            mensaje = "Error al agregar rol.",
                            errores = validationErrors
                        };

                        return Request.CreateResponse(HttpStatusCode.BadRequest, errorResponse);
                    }

                    var rol = new Rol
                    {
                        NombreRol = entidad.NombreRol
                    };

                    bd.Rol.Add(rol);
                    bd.SaveChanges();

                    var successResponse = new
                    {
                        mensaje = "Rol agregado exitosamente."
                    };

                    return Request.CreateResponse(HttpStatusCode.OK, successResponse);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al registrar rol: " + ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { mensaje = "Error interno del servidor." });
            }
        }

        [HttpPut]
        [Route("api/ActualizarRol")]
        public HttpResponseMessage ActualizarRol(RolEnt entidad)
        {
            try
            {
                if (entidad == null)
                {
                    var badRequestResponse = new
                    {
                        mensaje = "El objeto rol no puede ser nulo."
                    };
                    return Request.CreateResponse(HttpStatusCode.BadRequest, badRequestResponse);
                }

                if (!ModelState.IsValid)
                {
                    var errorResponse = new
                    {
                        mensaje = "Error de validación en el objeto rol.",
                        errores = ModelState.Values.SelectMany(v => v.Errors)
                                                   .Select(e => e.ErrorMessage)
                                                   .ToList()
                    };

                    return Request.CreateResponse(HttpStatusCode.BadRequest, errorResponse);
                }

                using (var bd = new Tienda_G6Entities1())
                {
                    // Verificar si el nombre de rol ya existe en la base de datos (sin importar el IdRol)
                    var nombreExistente = bd.Rol.FirstOrDefault(r => r.NombreRol == entidad.NombreRol);

                    if (nombreExistente != null)
                    {
                        // Si el nombre existe y no pertenece al registro actual, mostrar un error.
                        if (nombreExistente.IdRol != entidad.IdRol)
                        {
                            ModelState.AddModelError("NombreRol", "El nombre de rol ya existe en la base de datos.");
                            var validationErrors = ModelState.Values.SelectMany(v => v.Errors)
                                                                    .Select(e => e.ErrorMessage)
                                                                    .ToList();
                            var errorResponse = new
                            {
                                mensaje = "Error al agregar rol.",
                                errores = validationErrors
                            };

                            return Request.CreateResponse(HttpStatusCode.BadRequest, errorResponse);
                        }
                    }

                    var rol = bd.Rol.FirstOrDefault(r => r.IdRol == entidad.IdRol);

                    if (rol != null)
                    {
                        rol.NombreRol = entidad.NombreRol;
                        bd.SaveChanges();

                        var successResponse = new
                        {
                            mensaje = "Rol actualizado con éxito."
                        };

                        return Request.CreateResponse(HttpStatusCode.OK, successResponse);
                    }

                    var notFoundResponse = new
                    {
                        mensaje = $"Rol con ID '{entidad.IdRol}' no encontrado en la base de datos."
                    };

                    return Request.CreateResponse(HttpStatusCode.BadRequest, notFoundResponse);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al actualizar rol: " + ex.Message);
                var errorResponse = new
                {
                    mensaje = "Error interno del servidor."
                };
                return Request.CreateResponse(HttpStatusCode.InternalServerError, errorResponse);
            }
        }

        [HttpDelete]
        [Route("api/EliminarRol")]
        public HttpResponseMessage EliminarRol(long q)
        {
            try
            {
                using (var bd = new Tienda_G6Entities1())
                {
                    var rol = bd.Rol.FirstOrDefault(r => r.IdRol == q);

                    if (rol != null)
                    {
                        bd.Rol.Remove(rol);
                        bd.SaveChanges();

                        var successResponse = new
                        {
                            mensaje = "Rol eliminado con éxito."
                        };

                        return Request.CreateResponse(HttpStatusCode.OK, successResponse);
                    }

                    var notFoundResponse = new
                    {
                        mensaje = $"Rol con ID '{q}' no encontrado en la base de datos."
                    };

                    return Request.CreateResponse(HttpStatusCode.BadRequest, notFoundResponse);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al eliminar rol: " + ex.Message);
                var errorResponse = new
                {
                    mensaje = "Error interno del servidor."
                };
                return Request.CreateResponse(HttpStatusCode.InternalServerError, errorResponse);
            }
        }
    }

}