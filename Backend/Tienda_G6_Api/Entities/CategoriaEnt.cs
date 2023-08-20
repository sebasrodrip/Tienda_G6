using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tienda_G6_Api.Entities
{
    public class CategoriaEnt
    {
        public long IdCategoria { get; set; }

        [Required(ErrorMessage = "La Descripción es obligatoria.")]
        [StringLength(30, ErrorMessage = "La Descripción debe tener como máximo 30 caracteres.")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El Estado es obligatorio.")]
        public bool Estado { get; set; }
    }
}