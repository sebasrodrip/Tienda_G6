using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tienda_G6_Frontend.Entities;
using Tienda_G6_Frontend.Models;

namespace Tienda_G6_Frontend.Controllers
{
    public class VentaController : Controller
    {
        VentaModel modelVentas = new VentaModel();

        [HttpGet]
        public ActionResult Index()
        {
            var ventas = modelVentas.ConsultarVentas();
            return View(ventas);
        }

        [HttpPost]
        public ActionResult Agregar(VentaEnt entidad)
        {
            if (ModelState.IsValid)
            {
                var respuestaApi = modelVentas.RegistrarVenta(entidad);

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

            var ventas = modelVentas.ConsultarVentas();
            return View(ventas);
        }

        [HttpPost]
        public ActionResult Actualizar(VentaEnt entidad)
        {
            if (ModelState.IsValid)
            {
                var respuestaApi = modelVentas.ActualizarVenta(entidad);

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
            var respuestaApi = modelVentas.EliminarVenta(q);

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