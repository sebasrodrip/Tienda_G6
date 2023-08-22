using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tienda_G6_Frontend.Entities;
using Tienda_G6_Frontend.Models;

namespace Tienda_G6_Frontend.Controllers
{
    public class CreditoController : Controller
    {
        CreditoModel modelCreditos = new CreditoModel();

        [HttpGet]
        public ActionResult Index()
        {
            var creditos = modelCreditos.ConsultarCreditos();
            return View(creditos);
        }

        [HttpPost]
        public ActionResult Agregar(CreditoEnt entidad)
        {
            if (ModelState.IsValid)
            {
                var respuestaApi = modelCreditos.RegistrarCredito(entidad);

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

            var creditos = modelCreditos.ConsultarCreditos();
            return View(creditos);
        }

        [HttpPost]
        public ActionResult Actualizar(CreditoEnt entidad)
        {
            if (ModelState.IsValid)
            {
                var respuestaApi = modelCreditos.ActualizarCredito(entidad);

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
            var respuestaApi = modelCreditos.EliminarCredito(q);

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