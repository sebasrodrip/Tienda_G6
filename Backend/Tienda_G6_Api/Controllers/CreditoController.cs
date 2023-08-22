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
        public HttpResponseMessage ConsultarCredito(int q)
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
                        var successResponse = new
                        {
                            mensaje = "Consulta de crédito exitosa.",
                            data = new CreditoEnt
                            {
                                IdCredito = credito.IdCredito,
                                Limite = credito.Limite
                            }
                        };

                        return Request.CreateResponse(HttpStatusCode.OK, successResponse);
                    }

                    var notFoundResponse = new
                    {
                        mensaje = $"Crédito con ID '{q}' no encontrado en la base de datos."
                    };

                    return Request.CreateResponse(HttpStatusCode.BadRequest, notFoundResponse);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al consultar crédito: " + ex.Message);
                var errorResponse = new
                {
                    mensaje = "Error interno del servidor."
                };
                return Request.CreateResponse(HttpStatusCode.InternalServerError, errorResponse);
            }
        }

        [HttpPost]
        [Route("api/AgregarCredito")]
        public HttpResponseMessage AgregarCredito(CreditoEnt entidad)
        {
            try
            {
                if (entidad == null)
                {
                    var errorResponse = new
                    {
                        mensaje = "El objeto crédito no puede ser nulo."
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
                        mensaje = "Error de validación en el objeto crédito.",
                        errores = errors
                    };

                    return Request.CreateResponse(HttpStatusCode.BadRequest, errorResponse);
                }

                using (var bd = new Tienda_G6Entities1())
                {
                    var credito = new Credito
                    {
                        Limite = entidad.Limite
                    };

                    bd.Credito.Add(credito);
                    bd.SaveChanges();

                    var successResponse = new
                    {
                        mensaje = "Crédito agregado exitosamente."
                    };

                    return Request.CreateResponse(HttpStatusCode.OK, successResponse);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al registrar crédito: " + ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { mensaje = "Error interno del servidor." });
            }
        }

        [HttpPut]
        [Route("api/ActualizarCredito")]
        public HttpResponseMessage ActualizarCredito(CreditoEnt entidad)
        {
            try
            {
                if (entidad == null)
                {
                    var badRequestResponse = new
                    {
                        mensaje = "El objeto crédito no puede ser nulo."
                    };
                    return Request.CreateResponse(HttpStatusCode.BadRequest, badRequestResponse);
                }

                if (!ModelState.IsValid)
                {
                    var errorResponse = new
                    {
                        mensaje = "Error de validación en el objeto crédito.",
                        errores = ModelState.Values.SelectMany(v => v.Errors)
                                                   .Select(e => e.ErrorMessage)
                                                   .ToList()
                    };

                    return Request.CreateResponse(HttpStatusCode.BadRequest, errorResponse);
                }

                using (var bd = new Tienda_G6Entities1())
                {
                    var credito = bd.Credito.FirstOrDefault(c => c.IdCredito == entidad.IdCredito);

                    if (credito != null)
                    {
                        credito.Limite = entidad.Limite;
                        bd.SaveChanges();

                        var successResponse = new
                        {
                            mensaje = "Crédito actualizado con éxito."
                        };

                        return Request.CreateResponse(HttpStatusCode.OK, successResponse);
                    }

                    var notFoundResponse = new
                    {
                        mensaje = $"Crédito con ID '{entidad.IdCredito}' no encontrado en la base de datos."
                    };

                    return Request.CreateResponse(HttpStatusCode.BadRequest, notFoundResponse);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al actualizar crédito: " + ex.Message);
                var errorResponse = new
                {
                    mensaje = "Error interno del servidor."
                };
                return Request.CreateResponse(HttpStatusCode.InternalServerError, errorResponse);
            }
        }

        [HttpDelete]
        [Route("api/EliminarCredito")]
        public HttpResponseMessage EliminarCredito(int q)
        {
            try
            {
                using (var bd = new Tienda_G6Entities1())
                {
                    var credito = bd.Credito.FirstOrDefault(c => c.IdCredito == q);

                    if (credito != null)
                    {
                        bd.Credito.Remove(credito);
                        bd.SaveChanges();

                        var successResponse = new
                        {
                            mensaje = "Crédito eliminado con éxito."
                        };

                        return Request.CreateResponse(HttpStatusCode.OK, successResponse);
                    }

                    var notFoundResponse = new
                    {
                        mensaje = $"Crédito con ID '{q}' no encontrado en la base de datos."
                    };

                    return Request.CreateResponse(HttpStatusCode.BadRequest, notFoundResponse);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al eliminar crédito: " + ex.Message);
                var errorResponse = new
                {
                    mensaje = "Error interno del servidor."
                };
                return Request.CreateResponse(HttpStatusCode.InternalServerError, errorResponse);
            }
        }
    }

}
