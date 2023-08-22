using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tienda_G6_Frontend.Entities
{
    public class ArticuloEnt
    {
        public long IdArticulo { get; set; }

        [Required(ErrorMessage = "La Categoría es obligatoria.")]
        public long IdCategoria { get; set; }

        [Required(ErrorMessage = "La Descripción es obligatoria.")]
        [StringLength(20, ErrorMessage = "La Descripción debe tener como máximo 20 caracteres.")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El Detalle es obligatorio.")]
        [StringLength(20, ErrorMessage = "El Detalle debe tener como máximo 20 caracteres.")]
        public string Detalle { get; set; }

        [Required(ErrorMessage = "El Precio es obligatorio.")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "La Existencia es obligatoria.")]
        public int Existencia { get; set; }

        [Required(ErrorMessage = "El Estado es obligatorio.")]
        public bool Estado { get; set; }
    }
}