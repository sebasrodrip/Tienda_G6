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
//    [Authorize]
    public class RolController : ApiController
    {

        [HttpGet]
        [Route("api/ObtenerRoles")]
        public IHttpActionResult ObtenerRoles()
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
                Console.WriteLine("Error al obtener roles: " + ex.Message);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("api/ConsultarRol")]
        public IHttpActionResult ConsultarRol(long q)
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
                        return Ok(new RolEnt
                        {
                            IdRol = rol.IdRol,
                            NombreRol = rol.NombreRol
                        });
                    }

                    return BadRequest($"Rol con ID '{q}' no encontrado en la base de datos.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al consultar rol: " + ex.Message);
                return InternalServerError();
            }
        }

        [HttpPost]
        [Route("api/AgregarRol")]
        public IHttpActionResult AgregarRol(RolEnt entidad)
        {
            try
            {
                if (entidad == null)
                {
                    return BadRequest("El objeto rol no puede ser nulo.");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                using (var bd = new Tienda_G6Entities1())
                {

                    // Verificar si el nombre del rol ya existe en la base de datos
                    if (bd.Rol.Any(r => r.NombreRol == entidad.NombreRol))
                    {
                        ModelState.AddModelError("NombreRol", "El nombre de rol ya existe en la base de datos.");
                        return BadRequest(ModelState);
                    }

                    var rol = new Rol
                    {
                        NombreRol = entidad.NombreRol
                    };

                    bd.Rol.Add(rol);
                    bd.SaveChanges();

                    return Ok("Rol agregado exitosamente.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al registrar rol: " + ex.Message);
                return InternalServerError();
            }
        }

        [HttpPut]
        [Route("api/ActualizarRol")]
        public IHttpActionResult ActualizarRol(RolEnt entidad)
        {
            try
            {
                if (entidad == null)
                {
                    return BadRequest("El objeto rol no puede ser nulo.");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                using (var bd = new Tienda_G6Entities1())
                {
                    // Verificar si el nombre del rol ya existe en la base de datos (excepto el rol actual)
                    if (bd.Rol.Any(r => r.NombreRol == entidad.NombreRol && r.IdRol != entidad.IdRol))
                    {
                        ModelState.AddModelError("NombreRol", "El nombre de rol ya existe en la base de datos.");
                        return BadRequest(ModelState);
                    }

                    var rol = (from x in bd.Rol
                               where x.IdRol == entidad.IdRol
                               select x).FirstOrDefault();

                    if (rol != null)
                    {
                        rol.NombreRol = entidad.NombreRol;
                        bd.SaveChanges();
                        return Ok("Rol actualizado con éxito.");
                    }

                    return BadRequest($"Rol con ID '{entidad.IdRol}' no encontrado en la base de datos.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al actualizar rol: " + ex.Message);
                return InternalServerError();
            }
        }

        [HttpDelete]
        [Route("api/EliminarRol")]
        public IHttpActionResult EliminarRol(long q)
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
                        bd.Rol.Remove(rol);
                        bd.SaveChanges();
                        return Ok("Rol eliminado con éxito.");
                    }

                    return BadRequest($"Rol con ID '{q}' no encontrado en la base de datos.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al eliminar rol: " + ex.Message);
                return InternalServerError();
            }
        }


    }
}