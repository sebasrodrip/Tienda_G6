using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tienda_G6_Frontend.Entities;
using Tienda_G6_Frontend.Models;

namespace Tienda_G6_Frontend.Controllers
{
    public class ArticuloController : Controller
    {
        ArticuloModel modelArticulos = new ArticuloModel();

        [HttpGet]
        public ActionResult Index()
        {
            var articulos = modelArticulos.ConsultarArticulos();
            return View(articulos);
        }

        [HttpPost]
        public ActionResult Agregar(ArticuloEnt entidad)
        {
            if (ModelState.IsValid)
            {
                var respuestaApi = modelArticulos.RegistrarArticulo(entidad);

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

            var articulos = modelArticulos.ConsultarArticulos();
            return View(articulos);
        }

        [HttpPost]
        public ActionResult Actualizar(ArticuloEnt entidad)
        {
            if (ModelState.IsValid)
            {
                var respuestaApi = modelArticulos.ActualizarArticulo(entidad);

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
            var respuestaApi = modelArticulos.EliminarArticulo(q);

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