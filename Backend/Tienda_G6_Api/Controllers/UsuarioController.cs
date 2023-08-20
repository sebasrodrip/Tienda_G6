using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Http;
using Tienda_G6_Api.App_Start;
using Tienda_G6_Api.Entities;
using Tienda_G6_Api.Models;

namespace Tienda_G6_Api.Controllers
{
    [Authorize]
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
        //public IHttpActionResult RegistrarUsuario(UsuarioEnt usuario)
        //{
        //    try
        //    {
        //        if (usuario == null)
        //        {
        //            return BadRequest("El objeto usuario no puede ser nulo.");
        //        }

        //        if (!ModelState.IsValid)
        //        {
        //            return BadRequest(ModelState);
        //        }

        //        using (var bd = new Tienda_G6Entities1())
        //        {
        //            // Verificar si la Identificación ya existe en la base de datos
        //            if (bd.Usuario.Any(u => u.Identificacion == usuario.Identificacion))
        //            {
        //                ModelState.AddModelError("Identificacion", "La Identificación ya existe en la base de datos.");
        //                return BadRequest(ModelState);
        //            }

        //            // Verificar si el Email ya existe en la base de datos
        //            if (bd.Usuario.Any(u => u.Email == usuario.Email))
        //            {
        //                ModelState.AddModelError("Email", "El Email ya existe en la base de datos.");
        //                return BadRequest(ModelState);
        //            }

        //            var nuevoUsuario = new Usuario
        //            {
        //                Identificacion = usuario.Identificacion,
        //                Nombre = usuario.Nombre,
        //                Email = usuario.Email,
        //                Estado = true, // Valor predeterminado para Estado
        //                Contrasenna = usuario.Contrasenna,
        //                IdRol = 2 // Valor predeterminado para IdRol
        //            };

        //            bd.Usuario.Add(nuevoUsuario);
        //            bd.SaveChanges();

        //            return Ok("Usuario agregado exitosamente.");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Error al agregar usuario: " + ex.Message);
        //        return InternalServerError();
        //    }
        //}

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
                    var datos = (from x in bd.Usuario
                                 select x).ToList();

                    if (datos.Count > 0)
                    {
                        List<UsuarioEnt> res = new List<UsuarioEnt>();
                        foreach (var item in datos)
                        {
                            res.Add(new UsuarioEnt
                            {
                                IdUsuario = item.IdUsuario,
                                Identificacion = item.Identificacion,
                                Email = item.Email,
                                Nombre = item.Nombre,
                                Estado = item.Estado,
                                IdRol = item.IdRol
                            });
                        }

                        return Ok(res);
                    }

                    return BadRequest("No hay registros en la base de datos.");
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
        public IHttpActionResult ConsultarUsuario(long q)
        {
            try
            {
                using (var bd = new Tienda_G6Entities1())
                {
                    var datos = (from x in bd.Usuario
                                 where x.IdUsuario == q
                                 select x).FirstOrDefault();

                    if (datos != null)
                    {
                        UsuarioEnt res = new UsuarioEnt();
                        res.IdUsuario = datos.IdUsuario;
                        res.Identificacion = datos.Identificacion;
                        res.Nombre = datos.Nombre;
                        res.Email = datos.Email;
                        res.Estado = datos.Estado;
                        res.IdRol = datos.IdRol;

                        return Ok(res);
                    }

                    return BadRequest($"Usuario con ID '{q}' no encontrado en la base de datos.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al consultar usuario: " + ex.Message);
                return InternalServerError();
            }
        }

        [HttpPost]
        [Route("api/AgregarUsuario")]
        public IHttpActionResult AgregarUsuario(UsuarioEnt usuario)
        {
            try
            {
                if (usuario == null)
                {
                    return BadRequest("El objeto usuario no puede ser nulo.");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                using (var bd = new Tienda_G6Entities1())
                {
                    // Verificar si la Identificación ya existe en la base de datos
                    if (bd.Usuario.Any(u => u.Identificacion == usuario.Identificacion))
                    {
                        ModelState.AddModelError("Identificacion", "La Identificación ya existe en la base de datos.");
                        return BadRequest(ModelState);
                    }

                    // Verificar si el Email ya existe en la base de datos
                    if (bd.Usuario.Any(u => u.Email == usuario.Email))
                    {
                        ModelState.AddModelError("Email", "El Email ya existe en la base de datos.");
                        return BadRequest(ModelState);
                    }

                    var nuevoUsuario = new Usuario
                    {
                        Identificacion = usuario.Identificacion,
                        Nombre = usuario.Nombre,
                        Email = usuario.Email,
                        Estado = usuario.Estado,
                        Contrasenna = usuario.Contrasenna,
                        IdRol = usuario.IdRol
                    };

                    bd.Usuario.Add(nuevoUsuario);
                    bd.SaveChanges();

                    return Ok("Usuario agregado exitosamente.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al agregar usuario: " + ex.Message);
                return InternalServerError();
            }
        }

        [HttpPut]
        [Route("api/ActualizarUsuario")]
        public IHttpActionResult ActualizarUsuario(UsuarioEnt usuario)
        {
            try
            {
                if (usuario == null || usuario.IdUsuario <= 0)
                {
                    return BadRequest("Datos de usuario inválidos.");
                }

                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                using (var bd = new Tienda_G6Entities1())
                {
                    var usuarioExistente = bd.Usuario.FirstOrDefault(x => x.IdUsuario == usuario.IdUsuario);

                    if (usuarioExistente == null)
                    {
                        return NotFound();
                    }

                    usuarioExistente.Identificacion = usuario.Identificacion;
                    usuarioExistente.Nombre = usuario.Nombre;
                    usuarioExistente.Email = usuario.Email;
                    usuarioExistente.Estado = usuario.Estado;
                    usuarioExistente.IdRol = usuario.IdRol;

                    bd.SaveChanges();

                    return Ok("Usuario actualizado exitosamente.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al actualizar usuario: " + ex.Message);
                return InternalServerError();
            }
        }

        [HttpDelete]
        [Route("api/EliminarUsuario")]
        public IHttpActionResult EliminarUsuario(long q)
        {
            try
            {
                if (q <= 0)
                {
                    return BadRequest("Id de usuario inválido.");
                }

                using (var bd = new Tienda_G6Entities1())
                {
                    var usuario = bd.Usuario.FirstOrDefault(x => x.IdUsuario == q);

                    if (usuario == null)
                    {
                        return BadRequest($"Usuario con ID '{q}' no encontrado en la base de datos.");
                    }

                    bd.Usuario.Remove(usuario);
                    bd.SaveChanges();

                    return Ok("Usuario eliminado exitosamente.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al eliminar usuario: " + ex.Message);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("api/ConsultarRoles")]
        public List<RolEnt> ConsultarRoles()
        {
            using (var bd = new Tienda_G6Entities1())
            {
                var datos = (from x in bd.Rol
                             select x).ToList();

                if (datos.Count > 0)
                {
                    List<RolEnt> res = new List<RolEnt>();
                    foreach (var item in datos)
                    {
                        res.Add(new RolEnt
                        {
                            IdRol = item.IdRol,
                            NombreRol = item.NombreRol
                        });
                    }

                    return res;
                }

                return new List<RolEnt>();
            }
        }
    
    }
}