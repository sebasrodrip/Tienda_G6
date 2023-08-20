using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tienda_G6_Api.Entities
{
    public class BitacoraEnt
    {
        public string MensajeError { get; set; }
        public string Origen { get; set; }
        public long IdUsuario { get; set; }
        public string DireccionIP { get; set; }
    }
}