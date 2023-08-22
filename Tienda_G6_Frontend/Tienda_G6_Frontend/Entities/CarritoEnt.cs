using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tienda_G6_Frontend.Entities
{
    public class CarritoEnt
    {
        public long IdCarrito { get; set; }
        public long IdCliente { get; set; }
        public long IdArticulo { get; set; }

        [Required(ErrorMessage = "La fecha es obligatorio.")]
        public DateTime Fecha { get; set; }

        public float Precio { get; set; }
        public string Descripcion { get; set; }
        public string Detalle { get; set; }

    }
}