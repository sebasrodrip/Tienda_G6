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
    public class ClienteController : ApiController
    {
        [HttpGet]
        [Route("api/ConsultarClientes")]
        public IHttpActionResult ConsultarClientes()
        {
            try
            {
                using (var bd = new Tienda_G6Entities1())
                {
                    var clientes = (from x in bd.Cliente
                                    select x).ToList();

                    if (clientes.Count > 0)
                    {
                        List<ClienteEnt> res = new List<ClienteEnt>();
                        foreach (var item in clientes)
                        {
                            res.Add(new ClienteEnt
                            {
                                IdCliente = item.IdCliente,
                                IdUsuario = item.IdUsuario,
                                IdCredito = item.IdCredito,
                                Nombre = item.Nombre,
                                Apellidos = item.Apellidos,
                                Email = item.Email,
                                Telefono = item.Telefono
                            });
                        }

                        return Ok(res);
                    }

                    return BadRequest("No se encontraron clientes en la base de datos.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al Consultar clientes: " + ex.Message);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("api/ConsultarCliente")]
        public HttpResponseMessage ConsultarCliente(long q)
        {
            try
            {
                using (var bd = new Tienda_G6Entities1())
                {
                    var cliente = (from x in bd.Cliente
                                   where x.IdCliente == q
                                   select x).FirstOrDefault();

                    if (cliente != null)
                    {
                        var successResponse = new
                        {
                            mensaje = "Consulta de cliente exitosa.",
                            data = new ClienteEnt
                            {
                                IdCliente = cliente.IdCliente,
                                IdUsuario = cliente.IdUsuario,
                                IdCredito = cliente.IdCredito,
                                Nombre = cliente.Nombre,
                                Apellidos = cliente.Apellidos,
                                Email = cliente.Email,
                                Telefono = cliente.Telefono
                            }
                        };

                        return Request.CreateResponse(HttpStatusCode.OK, successResponse);
                    }

                    var notFoundResponse = new
                    {
                        mensaje = $"Cliente con ID '{q}' no encontrado en la base de datos."
                    };

                    return Request.CreateResponse(HttpStatusCode.BadRequest, notFoundResponse);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al consultar cliente: " + ex.Message);
                var errorResponse = new
                {
                    mensaje = "Error interno del servidor."
                };
                return Request.CreateResponse(HttpStatusCode.InternalServerError, errorResponse);
            }
        }

        [HttpPost]
        [Route("api/AgregarCliente")]
        public HttpResponseMessage AgregarCliente(ClienteEnt entidad)
        {
            try
            {
                if (entidad == null)
                {
                    var errorResponse = new
                    {
                        mensaje = "El objeto cliente no puede ser nulo."
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
                        mensaje = "Error de validación en el objeto cliente.",
                        errores = errors
                    };

                    return Request.CreateResponse(HttpStatusCode.BadRequest, errorResponse);
                }

                using (var bd = new Tienda_G6Entities1())
                {
                    var cliente = new Cliente
                    {
                        IdUsuario = entidad.IdUsuario,
                        IdCredito = entidad.IdCredito,
                        Nombre = entidad.Nombre,
                        Apellidos = entidad.Apellidos,
                        Email = entidad.Email,
                        Telefono = entidad.Telefono
                    };

                    bd.Cliente.Add(cliente);
                    bd.SaveChanges();

                    var successResponse = new
                    {
                        mensaje = "Cliente agregado exitosamente."
                    };

                    return Request.CreateResponse(HttpStatusCode.OK, successResponse);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al registrar cliente: " + ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { mensaje = "Error interno del servidor." });
            }
        }

        [HttpPut]
        [Route("api/ActualizarCliente")]
        public HttpResponseMessage ActualizarCliente(ClienteEnt entidad)
        {
            try
            {
                if (entidad == null)
                {
                    var badRequestResponse = new
                    {
                        mensaje = "El objeto cliente no puede ser nulo."
                    };
                    return Request.CreateResponse(HttpStatusCode.BadRequest, badRequestResponse);
                }

                if (!ModelState.IsValid)
                {
                    var errorResponse = new
                    {
                        mensaje = "Error de validación en el objeto cliente.",
                        errores = ModelState.Values.SelectMany(v => v.Errors)
                                                   .Select(e => e.ErrorMessage)
                                                   .ToList()
                    };

                    return Request.CreateResponse(HttpStatusCode.BadRequest, errorResponse);
                }

                using (var bd = new Tienda_G6Entities1())
                {
                    var cliente = bd.Cliente.FirstOrDefault(c => c.IdCliente == entidad.IdCliente);

                    if (cliente != null)
                    {
                        cliente.IdUsuario = entidad.IdUsuario;
                        cliente.IdCredito = entidad.IdCredito;
                        cliente.Nombre = entidad.Nombre;
                        cliente.Apellidos = entidad.Apellidos;
                        cliente.Email = entidad.Email;
                        cliente.Telefono = entidad.Telefono;
                        bd.SaveChanges();

                        var successResponse = new
                        {
                            mensaje = "Cliente actualizado con éxito."
                        };

                        return Request.CreateResponse(HttpStatusCode.OK, successResponse);
                    }

                    var notFoundResponse = new
                    {
                        mensaje = $"Cliente con ID '{entidad.IdCliente}' no encontrado en la base de datos."
                    };

                    return Request.CreateResponse(HttpStatusCode.BadRequest, notFoundResponse);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al actualizar cliente: " + ex.Message);
                var errorResponse = new
                {
                    mensaje = "Error interno del servidor."
                };
                return Request.CreateResponse(HttpStatusCode.InternalServerError, errorResponse);
            }
        }

        [HttpDelete]
        [Route("api/EliminarCliente")]
        public HttpResponseMessage EliminarCliente(long q)
        {
            try
            {
                using (var bd = new Tienda_G6Entities1())
                {
                    var cliente = bd.Cliente.FirstOrDefault(c => c.IdCliente == q);

                    if (cliente != null)
                    {
                        bd.Cliente.Remove(cliente);
                        bd.SaveChanges();

                        var successResponse = new
                        {
                            mensaje = "Cliente eliminado con éxito."
                        };

                        return Request.CreateResponse(HttpStatusCode.OK, successResponse);
                    }

                    var notFoundResponse = new
                    {
                        mensaje = $"Cliente con ID '{q}' no encontrado en la base de datos."
                    };

                    return Request.CreateResponse(HttpStatusCode.BadRequest, notFoundResponse);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al eliminar cliente: " + ex.Message);
                var errorResponse = new
                {
                    mensaje = "Error interno del servidor."
                };
                return Request.CreateResponse(HttpStatusCode.InternalServerError, errorResponse);
            }
        }
    }

}
