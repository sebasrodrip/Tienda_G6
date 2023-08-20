using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tienda_G6_Api.Entities
{
    public class RolEnt
    {

        public long IdRol { get; set; }

        [MinLength(6, ErrorMessage = "El nombre debe tener al menos 4 caracteres.")]
        [StringLength(20, ErrorMessage = "El nombre debe tener como máximo 20 caracteres.")]
        public string NombreRol { get; set; }
    }
}