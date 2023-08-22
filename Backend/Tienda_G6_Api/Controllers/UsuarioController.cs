using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web.Http;
using Tienda_G6_Api.App_Start;
using Tienda_G6_Api.Entities;
using Tienda_G6_Api.Models;

namespace Tienda_G6_Api.Controllers
{
//    [Authorize]
    public class UsuarioController : ApiController
    {

        UtilitariosModel util = new UtilitariosModel();
        TokenGenerator tokGenerator = new TokenGenerator();

        [HttpPost]
        [AllowAnonymous]
        [Route("api/IniciarSesion")]
        public UsuarioEnt IniciarSesion(UsuarioEnt entidad)
        {
                using (var bd = new Tienda_G6Entities1())
                {
                    var datos = (from x in bd.Usuario
                                 join y in bd.Rol on x.IdRol equals y.IdRol
                                 where x.Email == entidad.Email
                                    && x.Contrasenna == entidad.Contrasenna
                                    && x.Estado == true
                                 select new
                                 {
                                     x.ClaveTemporal,
                                     x.Caducidad,
                                     x.Email,
                                     x.Identificacion,
                                     x.Nombre,
                                     x.Estado,
                                     x.IdRol,
                                     x.IdUsuario,
                                     y.NombreRol
                                 }).FirstOrDefault();

                    if (datos != null)
                    {
                        if (datos.ClaveTemporal.Value && datos.Caducidad < DateTime.Now)
                        {
                            return null;
                        }

                        UsuarioEnt res = new UsuarioEnt();
                        res.Email = datos.Email;
                        res.Identificacion = datos.Identificacion;
                        res.Nombre = datos.Nombre;
                        res.Estado = datos.Estado;
                        res.IdRol = datos.IdRol;
                        res.IdUsuario = datos.IdUsuario;
                        res.NombreRol = datos.NombreRol;
                        res.Token = tokGenerator.GenerateTokenJwt(datos.IdUsuario);
                        return res;
                    }
                    return null;
                }

        }

        [HttpPost]
        [AllowAnonymous]
        [Route("api/RegistrarUsuario")]
        public int RegistrarUsuario(UsuarioEnt entidad)
        {
            using (var bd = new Tienda_G6Entities1())
            {
                return bd.RegistrarUsuario(entidad.Email,
                                    entidad.Contrasenna,
                                    entidad.Identificacion,
                                    entidad.Nombre,
                                    entidad.Estado,
                                    entidad.IdRol);
            }
        }

        [HttpPost]
        [Route("api/RecuperarContrasenna")]
        public bool RecuperarContrasenna(UsuarioEnt entidad)
        {
            using (var bd = new Tienda_G6Entities1())
            {
                var datos = (from x in bd.Usuario
                             where x.Email == entidad.Email
                                && x.Estado == true
                             select x).FirstOrDefault();

                if (datos != null)
                {
                    string password = util.CreatePassword();
                    datos.Contrasenna = util.Encrypt(password);
                    datos.ClaveTemporal = true;
                    datos.Caducidad = DateTime.Now.AddMinutes(30);
                    bd.SaveChanges();

                    string mensaje = "Estimado(a) " + datos.Nombre + ". Se ha generado la siguiente contraseña temporal: " + password;
                    util.SendEmail(datos.Email, "Recuperar Contraseña", mensaje);
                    return true;
                }

                return false;
            }
        }

        [HttpPut]
        [Route("api/CambiarContrasenna")]
        public int CambiarContrasenna(UsuarioEnt entidad)
        {
            using (var bd = new Tienda_G6Entities1())
            {
                var datos = (from x in bd.Usuario
                             where x.IdUsuario == entidad.IdUsuario
                             select x).FirstOrDefault();

                if (datos != null)
                {
                    datos.Contrasenna = entidad.ContrasennaNueva;
                    datos.ClaveTemporal = false;
                    datos.Caducidad = DateTime.Now;
                    return bd.SaveChanges();
                }

                return 0;
            }
        }

        [HttpPut]
        [Route("api/CambiarEstado")]
        public int CambiarEstado(UsuarioEnt entidad)
        {
            using (var bd = new Tienda_G6Entities1())
            {
                var datos = (from x in bd.Usuario
                             where x.IdUsuario == entidad.IdUsuario
                             select x).FirstOrDefault();

                if (datos != null)
                {
                    var EstadoActual = datos.Estado;

                    datos.Estado = (EstadoActual == true ? false : true);
                    return bd.SaveChanges();
                }

                return 0;
            }
        }


        [HttpGet]
        [Route("api/ConsultarUsuarios")]
        public IHttpActionResult ConsultarUsuarios()
        {
            try
            {
                using (var bd = new Tienda_G6Entities1())
                {
                    var usuarios = bd.Usuario.ToList();

                    if (usuarios.Count > 0)
                    {
                        List<UsuarioEnt> res = new List<UsuarioEnt>();
                        foreach (var item in usuarios)
                        {
                            res.Add(new UsuarioEnt
                            {
                                IdUsuario = item.IdUsuario,
                                Identificacion = item.Identificacion,
                                Nombre = item.Nombre,
                                Email = item.Email,
                                Estado = item.Estado,
                                IdRol = item.IdRol
                            });
                        }

                        return Ok(res);
                    }

                    return BadRequest("No se encontraron usuarios en la base de datos.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al consultar usuarios: " + ex.Message);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("api/ConsultarUsuario")]
        public HttpResponseMessage ConsultarUsuario(long q)
        {
            try
            {
                using (var bd = new Tienda_G6Entities1())
                {
                    var usuario = bd.Usuario.FirstOrDefault(u => u.IdUsuario == q);

                    if (usuario != null)
                    {
                        var successResponse = new
                        {
                            mensaje = "Consulta de usuario exitosa.",
                            data = new UsuarioEnt
                            {
                                IdUsuario = usuario.IdUsuario,
                                Identificacion = usuario.Identificacion,
                                Nombre = usuario.Nombre,
                                Email = usuario.Email,
                                Estado = usuario.Estado,
                                IdRol = usuario.IdRol
                            }
                        };

                        return Request.CreateResponse(HttpStatusCode.OK, successResponse);
                    }

                    var notFoundResponse = new
                    {
                        mensaje = $"Usuario con ID '{q}' no encontrado en la base de datos."
                    };

                    return Request.CreateResponse(HttpStatusCode.BadRequest, notFoundResponse);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al consultar usuario: " + ex.Message);
                var errorResponse = new
                {
                    mensaje = "Error interno del servidor."
                };
                return Request.CreateResponse(HttpStatusCode.InternalServerError, errorResponse);
            }
        }

        [HttpPost]
        [Route("api/AgregarUsuario")]
        public HttpResponseMessage AgregarUsuario(UsuarioEnt entidad)
        {
            try
            {
                if (entidad == null)
                {
                    var errorResponse = new
                    {
                        mensaje = "El objeto usuario no puede ser nulo."
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
                        mensaje = "Error de validación en el objeto usuario.",
                        errores = errors
                    };

                    return Request.CreateResponse(HttpStatusCode.BadRequest, errorResponse);
                }

                using (var bd = new Tienda_G6Entities1())
                {
                    // Verificar si el correo ya existe en la base de datos.
                    if (bd.Usuario.Any(u => u.Email == entidad.Email))
                    {
                        var errorResponse = new
                        {
                            mensaje = "El correo ya existe en la base de datos."
                        };

                        return Request.CreateResponse(HttpStatusCode.BadRequest, errorResponse);
                    }

                    var usuario = new Usuario
                    {
                        Identificacion = entidad.Identificacion,
                        Nombre = entidad.Nombre,
                        Email = entidad.Email,
                        Contrasenna = entidad.Contrasenna,
                        Estado = entidad.Estado,
                        IdRol = entidad.IdRol
                    };

                    bd.Usuario.Add(usuario);
                    bd.SaveChanges();

                    var successResponse = new
                    {
                        mensaje = "Usuario agregado exitosamente."
                    };

                    return Request.CreateResponse(HttpStatusCode.OK, successResponse);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al registrar usuario: " + ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { mensaje = "Error interno del servidor." });
            }
        }

        [HttpPut]
        [Route("api/ActualizarUsuario")]
        public HttpResponseMessage ActualizarUsuario(UsuarioEnt entidad)
        {
            try
            {
                if (entidad == null)
                {
                    var badRequestResponse = new
                    {
                        mensaje = "El objeto usuario no puede ser nulo."
                    };
                    return Request.CreateResponse(HttpStatusCode.BadRequest, badRequestResponse);
                }

                if (!ModelState.IsValid)
                {
                    var errorResponse = new
                    {
                        mensaje = "Error de validación en el objeto usuario.",
                        errores = ModelState.Values.SelectMany(v => v.Errors)
                                                   .Select(e => e.ErrorMessage)
                                                   .ToList()
                    };

                    return Request.CreateResponse(HttpStatusCode.BadRequest, errorResponse);
                }

                using (var bd = new Tienda_G6Entities1())
                {
                    // Verificar si el correo ya existe en la base de datos (sin importar el IdUsuario).
                    if (bd.Usuario.Any(u => u.Email == entidad.Email && u.IdUsuario != entidad.IdUsuario))
                    {
                        var errorResponse = new
                        {
                            mensaje = "El correo ya existe en la base de datos."
                        };

                        return Request.CreateResponse(HttpStatusCode.BadRequest, errorResponse);
                    }

                    var usuario = bd.Usuario.FirstOrDefault(u => u.IdUsuario == entidad.IdUsuario);

                    if (usuario != null)
                    {
                        usuario.Identificacion = entidad.Identificacion;
                        usuario.Nombre = entidad.Nombre;
                        usuario.Email = entidad.Email;
                        usuario.Estado = entidad.Estado;
                        usuario.IdRol = entidad.IdRol;

                        bd.SaveChanges();

                        var successResponse = new
                        {
                            mensaje = "Usuario actualizado con éxito."
                        };

                        return Request.CreateResponse(HttpStatusCode.OK, successResponse);
                    }

                    var notFoundResponse = new
                    {
                        mensaje = $"Usuario con ID '{entidad.IdUsuario}' no encontrado en la base de datos."
                    };

                    return Request.CreateResponse(HttpStatusCode.BadRequest, notFoundResponse);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al actualizar usuario: " + ex.Message);
                var errorResponse = new
                {
                    mensaje = "Error interno del servidor."
                };
                return Request.CreateResponse(HttpStatusCode.InternalServerError, errorResponse);
            }
        }

        [HttpDelete]
        [Route("api/EliminarUsuario")]
        public HttpResponseMessage EliminarUsuario(long q)
        {
            try
            {
                using (var bd = new Tienda_G6Entities1())
                {
                    var usuario = bd.Usuario.FirstOrDefault(u => u.IdUsuario == q);

                    if (usuario != null)
                    {
                        bd.Usuario.Remove(usuario);
                        bd.SaveChanges();

                        var successResponse = new
                        {
                            mensaje = "Usuario eliminado con éxito."
                        };

                        return Request.CreateResponse(HttpStatusCode.OK, successResponse);
                    }

                    var notFoundResponse = new
                    {
                        mensaje = $"Usuario con ID '{q}' no encontrado en la base de datos."
                    };

                    return Request.CreateResponse(HttpStatusCode.BadRequest, notFoundResponse);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al eliminar usuario: " + ex.Message);
                var errorResponse = new
                {
                    mensaje = "Error interno del servidor."
                };
                return Request.CreateResponse(HttpStatusCode.InternalServerError, errorResponse);
            }
        }

    }
}