using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tienda_G6_Api.Entities
{
    public class VentaEnt
    {
        public long IdVenta { get; set; }

        [Required(ErrorMessage = "El ID de la Factura es obligatorio.")]
        public long IdFactura { get; set; }

        [Required(ErrorMessage = "El ID del Artículo es obligatorio.")]
        public long IdArticulo { get; set; }

        [Required(ErrorMessage = "El Precio es obligatorio.")]
        public double Precio { get; set; }

        [Required(ErrorMessage = "La Cantidad es obligatoria.")]
        public int Cantidad { get; set; }
    }
}