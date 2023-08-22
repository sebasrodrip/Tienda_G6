using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tienda_G6_Frontend.Entities;
using Tienda_G6_Frontend.Models;

namespace Tienda_G6_Frontend.Controllers
{
    public class FacturaController : Controller
    {
        FacturaModel modelFacturas = new FacturaModel();

        [HttpGet]
        public ActionResult Index()
        {
            var facturas = modelFacturas.ConsultarFacturas();
            return View(facturas);
        }

        [HttpPost]
        public ActionResult Agregar(FacturaEnt entidad)
        {
            if (ModelState.IsValid)
            {
                var respuestaApi = modelFacturas.RegistrarFactura(entidad);

                if (!respuestaApi.Success)
                {
                    ModelState.AddModelError("", respuestaApi.Mensaje);
                    if (respuestaApi.Errores != null)
                    {
                        foreach (var error in respuestaApi.Errores)
                        {
                            ModelState.AddModelError("", error);
                        }
                    }
                }

                return Json(respuestaApi);
            }

            var facturas = modelFacturas.ConsultarFacturas();
            return View(facturas);
        }

        [HttpPost]
        public ActionResult Actualizar(FacturaEnt entidad)
        {
            if (ModelState.IsValid)
            {
                var respuestaApi = modelFacturas.ActualizarFactura(entidad);

                if (!respuestaApi.Success)
                {
                    ModelState.AddModelError("", respuestaApi.Mensaje);
                    if (respuestaApi.Errores != null)
                    {
                        foreach (var error in respuestaApi.Errores)
                        {
                            ModelState.AddModelError("", error);
                        }
                    }
                }

                return Json(respuestaApi);
            }
            return Json(new { Success = false, Mensaje = "Error en la validación del formulario." });
        }

        [HttpPost]
        public ActionResult Eliminar(long q)
        {
            var respuestaApi = modelFacturas.EliminarFactura(q);

            if (!respuestaApi.Success)
            {
                ModelState.AddModelError("", respuestaApi.Mensaje);
                if (respuestaApi.Errores != null)
                {
                    foreach (var error in respuestaApi.Errores)
                    {
                        ModelState.AddModelError("", error);
                    }
                }
            }
            return Json(respuestaApi);
        }
    }

}