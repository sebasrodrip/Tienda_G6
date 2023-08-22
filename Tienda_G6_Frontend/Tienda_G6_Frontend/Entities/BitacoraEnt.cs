using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tienda_G6_Frontend.Entities
{
    public class BitacoraEnt
    {
        public long IdBitacora { get; set; }

        [Required(ErrorMessage = "La Fecha y Hora son obligatorias.")]
        public DateTime FechaHora { get; set; }

        [Required(ErrorMessage = "El Mensaje de Error es obligatorio.")]
        [StringLength(5000, ErrorMessage = "El Mensaje de Error debe tener como máximo 5000 caracteres.")]
        public string MensajeError { get; set; }

        [Required(ErrorMessage = "El Origen es obligatorio.")]
        [StringLength(5000, ErrorMessage = "El Origen debe tener como máximo 5000 caracteres.")]
        public string Origen { get; set; }

        [Required(ErrorMessage = "El ID del Usuario es obligatorio.")]
        public long IdUsuario { get; set; }

        [Required(ErrorMessage = "La Dirección IP es obligatoria.")]
        [StringLength(50, ErrorMessage = "La Dirección IP debe tener como máximo 50 caracteres.")]
        public string DireccionIP { get; set; }
    }
}