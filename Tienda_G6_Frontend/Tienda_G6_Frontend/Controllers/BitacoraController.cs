using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tienda_G6_Frontend.Models;

namespace Tienda_G6_Frontend.Controllers
{
    public class BitacoraController : Controller
    {
        BitacoraModel modelBitacoras = new BitacoraModel();

        [HttpGet]
        public ActionResult Index()
        {
            var bitacoras = modelBitacoras.ConsultarBitacoras();
            return View(bitacoras);
        }
    }

}