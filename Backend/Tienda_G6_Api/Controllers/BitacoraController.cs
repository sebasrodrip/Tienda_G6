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

                    return BadRequest("No se encontraron registros en la bitácora.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al Consultar registros de la bitácora: " + ex.Message);
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("api/ConsultarBitacora")]
        public IHttpActionResult ConsultarBitacora(long q)
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
                        return Ok(new BitacoraEnt
                        {
                            IdBitacora = bitacora.IdBitacora,
                            FechaHora = bitacora.FechaHora,
                            MensajeError = bitacora.MensajeError,
                            Origen = bitacora.Origen,
                            IdUsuario = bitacora.IdUsuario,
                            DireccionIP = bitacora.DireccionIP
                        });
                    }

                    return BadRequest($"Registro de bitácora con ID '{q}' no encontrado.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al consultar registro de bitácora: " + ex.Message);
                return InternalServerError();
            }
        }
    }

}
