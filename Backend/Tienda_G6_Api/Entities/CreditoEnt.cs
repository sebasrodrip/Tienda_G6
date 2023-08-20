using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Tienda_G6_Api.Entities
{
    public class CreditoEnt
    {
        public int IdCredito { get; set; }

        [Required(ErrorMessage = "El Límite es obligatorio.")]
        public double Limite { get; set; }
    }
}