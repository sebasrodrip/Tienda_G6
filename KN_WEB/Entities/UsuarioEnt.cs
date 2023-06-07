using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KN_WEB.Entities
{
    public class UsuarioEnt
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfPass { get; set; }
        public string ID { get; set; }
        public string Nombre { get; set; }
        public bool Estado { get; set; }
    }
}