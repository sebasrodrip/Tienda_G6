using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KN_WEB.Entities
{
    public class ProductoEnt
    {
        public long IdProducto { get; set; }

        public long NombreProducto { get; set; }

        public long Descripcion { get; set; }

        public long Precio { get; set; }

        public long Imagen { get; set; }
    }
}