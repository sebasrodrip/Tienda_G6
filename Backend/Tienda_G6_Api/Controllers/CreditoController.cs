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
    public class CreditoController : ApiController
    {
        [HttpGet]
        [Route("api/ConsultarCreditos")]
        public IHttpActionResult ConsultarCreditos()
        {
            try
            {
                using (var bd = new Tienda_G6Entities1())
                {
                    var creditos = (from x in bd.Credito
                                    select x).ToList();

                    if (creditos.Count > 0)
                    {
                        List<CreditoEnt> res = new List<CreditoEnt>();
                        foreach (var item in creditos)
                        {
                            res.Add(new CreditoEnt
                            {
                                IdCredito = item.IdCredito,
                                Limite = item.Limite
                            });
                        }

                        return Ok(res);
                    }

                    return BadRequest("No se encontraron créditos en la base de datos.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al Consultar créditos: " + ex.Message);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("api/ConsultarCredito")]
        public IHttpActionResult ConsultarCredito(int q)
        {
            try
            {
                using (var bd = new Tienda_G6Entities1())
                {
                    var credito = (from x in bd.Credito
                                   where x.IdCredito == q
                                   select x).FirstOrDefault();

                    if (credito != null)
                    {
                        return Ok(new CreditoEnt
                        {
                            IdCredito = credito.IdCredito,
                            Limite = credito.Limite
                        });
                    }

                    return BadRequest($"Crédito con ID '{q}' no encontrado en la base de datos.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al consultar crédito: " + ex.Message);
                return InternalServerError();
            }
        }

        [HttpPost]
        [Route("api/AgregarCredito")]
        public IHttpActionResult AgregarCredito(CreditoEnt entidad)
        {
            try
            {
                if (entidad == null)
                {
                    return BadRequest("El objeto crédito no puede ser nulo.");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                using (var bd = new Tienda_G6Entities1())
                {
                    var credito = new Credito
                    {
                        Limite = entidad.Limite
                    };

                    bd.Credito.Add(credito);
                    bd.SaveChanges();

                    return Ok("Crédito agregado exitosamente.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al registrar crédito: " + ex.Message);
                return InternalServerError();
            }
        }

        [HttpPut]
        [Route("api/ActualizarCredito")]
        public IHttpActionResult ActualizarCredito(CreditoEnt entidad)
        {
            try
            {
                if (entidad == null)
                {
                    return BadRequest("El objeto crédito no puede ser nulo.");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                using (var bd = new Tienda_G6Entities1())
                {
                    var credito = (from x in bd.Credito
                                   where x.IdCredito == entidad.IdCredito
                                   select x).FirstOrDefault();

                    if (credito != null)
                    {
                        credito.Limite = entidad.Limite;
                        bd.SaveChanges();
                        return Ok("Crédito actualizado con éxito.");
                    }

                    return BadRequest($"Crédito con ID '{entidad.IdCredito}' no encontrado en la base de datos.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al actualizar crédito: " + ex.Message);
                return InternalServerError();
            }
        }

        [HttpDelete]
        [Route("api/EliminarCredito")]
        public IHttpActionResult EliminarCredito(int q)
        {
            try
            {
                using (var bd = new Tienda_G6Entities1())
                {
                    var credito = (from x in bd.Credito
                                   where x.IdCredito == q
                                   select x).FirstOrDefault();

                    if (credito != null)
                    {
                        bd.Credito.Remove(credito);
                        bd.SaveChanges();
                        return Ok("Crédito eliminado con éxito.");
                    }

                    return BadRequest($"Crédito con ID '{q}' no encontrado en la base de datos.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al eliminar crédito: " + ex.Message);
                return InternalServerError();
            }
        }
    }

}
