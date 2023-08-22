using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tienda_G6_Frontend.Entities
{
    public class ClienteEnt
    {
        public long IdCliente { get; set; }

        [Required(ErrorMessage = "El Usuario es obligatorio.")]
        public long IdUsuario { get; set; }

        [Required(ErrorMessage = "El Crédito es obligatorio.")]
        public int IdCredito { get; set; }

        [Required(ErrorMessage = "El Nombre es obligatorio.")]
        [StringLength(20, ErrorMessage = "El Nombre debe tener como máximo 20 caracteres.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Los Apellidos son obligatorios.")]
        [StringLength(20, ErrorMessage = "Los Apellidos deben tener como máximo 20 caracteres.")]
        public string Apellidos { get; set; }

        [Required(ErrorMessage = "El Email es obligatorio.")]
        [EmailAddress(ErrorMessage = "El Email no es válido.")]
        [StringLength(20, ErrorMessage = "El Email debe tener como máximo 20 caracteres.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El Teléfono es obligatorio.")]
        [StringLength(15, ErrorMessage = "El Teléfono debe tener como máximo 15 caracteres.")]
        public string Telefono { get; set; }
    }
}