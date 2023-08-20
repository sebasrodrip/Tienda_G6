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
        public IHttpActionResult ConsultarCliente(long q)
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
                        return Ok(new ClienteEnt
                        {
                            IdCliente = cliente.IdCliente,
                            IdUsuario = cliente.IdUsuario,
                            IdCredito = cliente.IdCredito,
                            Nombre = cliente.Nombre,
                            Apellidos = cliente.Apellidos,
                            Email = cliente.Email,
                            Telefono = cliente.Telefono
                        });
                    }

                    return BadRequest($"Cliente con ID '{q}' no encontrado en la base de datos.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al consultar cliente: " + ex.Message);
                return InternalServerError();
            }
        }

        [HttpPost]
        [Route("api/AgregarCliente")]
        public IHttpActionResult AgregarCliente(ClienteEnt entidad)
        {
            try
            {
                if (entidad == null)
                {
                    return BadRequest("El objeto cliente no puede ser nulo.");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
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

                    return Ok("Cliente agregado exitosamente.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al registrar cliente: " + ex.Message);
                return InternalServerError();
            }
        }

        [HttpPut]
        [Route("api/ActualizarCliente")]
        public IHttpActionResult ActualizarCliente(ClienteEnt entidad)
        {
            try
            {
                if (entidad == null)
                {
                    return BadRequest("El objeto cliente no puede ser nulo.");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                using (var bd = new Tienda_G6Entities1())
                {
                    var cliente = (from x in bd.Cliente
                                   where x.IdCliente == entidad.IdCliente
                                   select x).FirstOrDefault();

                    if (cliente != null)
                    {
                        cliente.IdUsuario = entidad.IdUsuario;
                        cliente.IdCredito = entidad.IdCredito;
                        cliente.Nombre = entidad.Nombre;
                        cliente.Apellidos = entidad.Apellidos;
                        cliente.Email = entidad.Email;
                        cliente.Telefono = entidad.Telefono;
                        bd.SaveChanges();
                        return Ok("Cliente actualizado con éxito.");
                    }

                    return BadRequest($"Cliente con ID '{entidad.IdCliente}' no encontrado en la base de datos.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al actualizar cliente: " + ex.Message);
                return InternalServerError();
            }
        }

        [HttpDelete]
        [Route("api/EliminarCliente")]
        public IHttpActionResult EliminarCliente(long q)
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
                        bd.Cliente.Remove(cliente);
                        bd.SaveChanges();
                        return Ok("Cliente eliminado con éxito.");
                    }

                    return BadRequest($"Cliente con ID '{q}' no encontrado en la base de datos.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al eliminar cliente: " + ex.Message);
                return InternalServerError();
            }
        }
    }

}
