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
    public class BitacoraController : ApiController
    {
        [HttpGet]
        [Route("api/ConsultarBitacoras")]
        public IHttpActionResult ConsultarBitacoras()
        {
            try
            {
                using (var bd = new Tienda_G6Entities1())
                {
                    var bitacoras = (from x in bd.Bitacora
                                     select x).ToList();

                    if (bitacoras.Count > 0)
                    {
                        List<BitacoraEnt> res = new List<BitacoraEnt>();
                        foreach (var item in bitacoras)
                        {
                            res.Add(new BitacoraEnt
                            {
                                IdBitacora = item.IdBitacora,
                                FechaHora = item.FechaHora,
                                MensajeError = item.MensajeError,
                                Origen = item.Origen,
                                IdUsuario = item.IdUsuario,
                                DireccionIP = item.DireccionIP
                            });
                        }

                        return Ok(res);
                    }

                    return BadRequest("No se encontraron bitácoras en la base de datos.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al Consultar bitácoras: " + ex.Message);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("api/ConsultarBitacora")]
        public HttpResponseMessage ConsultarBitacora(long q)
        {
            try
            {
                using (var bd = new Tienda_G6Entities1())
                {
                    var bitacora = (from x in bd.Bitacora
                                    where x.IdBitacora == q
                                    select x).FirstOrDefault();

                    if (bitacora != null)
                    {
                        var successResponse = new
                        {
                            mensaje = "Consulta de bitácora exitosa.",
                            data = new BitacoraEnt
                            {
                                IdBitacora = bitacora.IdBitacora,
                                FechaHora = bitacora.FechaHora,
                                MensajeError = bitacora.MensajeError,
                                Origen = bitacora.Origen,
                                IdUsuario = bitacora.IdUsuario,
                                DireccionIP = bitacora.DireccionIP
                            }
                        };

                        return Request.CreateResponse(HttpStatusCode.OK, successResponse);
                    }

                    var notFoundResponse = new
                    {
                        mensaje = $"Bitácora con ID '{q}' no encontrada en la base de datos."
                    };

                    return Request.CreateResponse(HttpStatusCode.BadRequest, notFoundResponse);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al consultar bitácora: " + ex.Message);
                var errorResponse = new
                {
                    mensaje = "Error interno del servidor."
                };
                return Request.CreateResponse(HttpStatusCode.InternalServerError, errorResponse);
            }
        }

        [HttpPost]
        [Route("api/AgregarBitacora")]
        public HttpResponseMessage AgregarBitacora(BitacoraEnt entidad)
        {
            try
            {
                if (entidad == null)
                {
                    var errorResponse = new
                    {
                        mensaje = "El objeto bitácora no puede ser nulo."
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
                        mensaje = "Error de validación en el objeto bitácora.",
                        errores = errors
                    };

                    return Request.CreateResponse(HttpStatusCode.BadRequest, errorResponse);
                }

                using (var bd = new Tienda_G6Entities1())
                {
                    var bitacora = new Bitacora
                    {
                        FechaHora = entidad.FechaHora,
                        MensajeError = entidad.MensajeError,
                        Origen = entidad.Origen,
                        IdUsuario = entidad.IdUsuario,
                        DireccionIP = entidad.DireccionIP
                    };

                    bd.Bitacora.Add(bitacora);
                    bd.SaveChanges();

                    var successResponse = new
                    {
                        mensaje = "Bitácora agregada exitosamente."
                    };

                    return Request.CreateResponse(HttpStatusCode.OK, successResponse);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al registrar bitácora: " + ex.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { mensaje = "Error interno del servidor." });
            }
        }

        [HttpPut]
        [Route("api/ActualizarBitacora")]
        public HttpResponseMessage ActualizarBitacora(BitacoraEnt entidad)
        {
            try
            {
                if (entidad == null)
                {
                    var badRequestResponse = new
                    {
                        mensaje = "El objeto bitácora no puede ser nulo."
                    };
                    return Request.CreateResponse(HttpStatusCode.BadRequest, badRequestResponse);
                }

                if (!ModelState.IsValid)
                {
                    var errorResponse = new
                    {
                        mensaje = "Error de validación en el objeto bitácora.",
                        errores = ModelState.Values.SelectMany(v => v.Errors)
                                                   .Select(e => e.ErrorMessage)
                                                   .ToList()
                    };

                    return Request.CreateResponse(HttpStatusCode.BadRequest, errorResponse);
                }

                using (var bd = new Tienda_G6Entities1())
                {
                    var bitacora = bd.Bitacora.FirstOrDefault(b => b.IdBitacora == entidad.IdBitacora);

                    if (bitacora != null)
                    {
                        bitacora.FechaHora = entidad.FechaHora;
                        bitacora.MensajeError = entidad.MensajeError;
                        bitacora.Origen = entidad.Origen;
                        bitacora.IdUsuario = entidad.IdUsuario;
                        bitacora.DireccionIP = entidad.DireccionIP;
                        bd.SaveChanges();

                        var successResponse = new
                        {
                            mensaje = "Bitácora actualizada con éxito."
                        };

                        return Request.CreateResponse(HttpStatusCode.OK, successResponse);
                    }

                    var notFoundResponse = new
                    {
                        mensaje = $"Bitácora con ID '{entidad.IdBitacora}' no encontrada en la base de datos."
                    };

                    return Request.CreateResponse(HttpStatusCode.BadRequest, notFoundResponse);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al actualizar bitácora: " + ex.Message);
                var errorResponse = new
                {
                    mensaje = "Error interno del servidor."
                };
                return Request.CreateResponse(HttpStatusCode.InternalServerError, errorResponse);
            }
        }

        [HttpDelete]
        [Route("api/EliminarBitacora")]
        public HttpResponseMessage EliminarBitacora(long q)
        {
            try
            {
                using (var bd = new Tienda_G6Entities1())
                {
                    var bitacora = bd.Bitacora.FirstOrDefault(b => b.IdBitacora == q);

                    if (bitacora != null)
                    {
                        bd.Bitacora.Remove(bitacora);
                        bd.SaveChanges();

                        var successResponse = new
                        {
                            mensaje = "Bitácora eliminada con éxito."
                        };

                        return Request.CreateResponse(HttpStatusCode.OK, successResponse);
                    }

                    var notFoundResponse = new
                    {
                        mensaje = $"Bitácora con ID '{q}' no encontrada en la base de datos."
                    };

                    return Request.CreateResponse(HttpStatusCode.BadRequest, notFoundResponse);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al eliminar bitácora: " + ex.Message);
                var errorResponse = new
                {
                    mensaje = "Error interno del servidor."
                };
                return Request.CreateResponse(HttpStatusCode.InternalServerError, errorResponse);
            }
        }
    }

}
