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
    [Authorize]
    public class UtilitariosController : ApiController
    {

        [HttpPost]
        [Route("api/RegistrarBitacora")]
        public void RegistrarBitacora(BitacoraEnt entidad)
        {
            using (var bd = new Tienda_G6Entities1())
            {
                bd.RegistrarBitacora(entidad.MensajeError,
                                    entidad.Origen,
                                    entidad.IdUsuario,
                                    entidad.DireccionIP);
            }
        }
    }
}
