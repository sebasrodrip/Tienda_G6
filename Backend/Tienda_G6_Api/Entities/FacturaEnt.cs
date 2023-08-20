using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tienda_G6_Api.Entities
{
    public class FacturaEnt
    {
        public long IdFactura { get; set; }

        [Required(ErrorMessage = "El ID del Cliente es obligatorio.")]
        public long IdCliente { get; set; }

        [Required(ErrorMessage = "La Fecha es obligatoria.")]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "El Total es obligatorio.")]
        public double Total { get; set; }

        [Required(ErrorMessage = "El Estado es obligatorio.")]
        public int Estado { get; set; }
    }
}