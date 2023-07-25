using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KN_WEB.Entities
{
    public class UsuarioEnt
    {
        public long IdUsuario { get; set; }

        [Required(ErrorMessage = "La Identificación es obligatoria.")]
        [StringLength(20, ErrorMessage = "La Identificación debe tener como máximo 20 caracteres.")]
        public string Identificacion { get; set; }

        [Required(ErrorMessage = "El Nombre es obligatorio.")]
        [StringLength(20, ErrorMessage = "El Nombre debe tener como máximo 20 caracteres.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El Email es obligatorio.")]
        [EmailAddress(ErrorMessage = "El Email no es válido.")]
        [StringLength(20, ErrorMessage = "El Email debe tener como máximo 20 caracteres.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La Contraseña es obligatoria.")]
        [MinLength(6, ErrorMessage = "La Contraseña debe tener al menos 6 caracteres.")]
        public string Contrasenna { get; set; }

        [Compare("Contrasenna", ErrorMessage = "Las contraseñas no coinciden.")]
        public string ConfirmarContrasenna { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio.")]
        public bool Estado { get; set; }

        [Required(ErrorMessage = "El rol es obligatorio.")]
        public int IdRol { get; set; }

        public string NombreRol { get; set; }

        public string ContrasennaNueva { get; set; }
    }
}