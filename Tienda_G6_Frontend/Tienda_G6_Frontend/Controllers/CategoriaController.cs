using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tienda_G6_Frontend.Entities;
using Tienda_G6_Frontend.Models;

namespace Tienda_G6_Frontend.Controllers
{
    public class CategoriaController : Controller
    {
        CategoriaModel modelCategorias = new CategoriaModel();

        [HttpGet]
        public ActionResult Index()
        {
            var categorias = modelCategorias.ConsultarCategorias();
            return View(categorias);
        }

        [HttpPost]
        public ActionResult Agregar(CategoriaEnt entidad)
        {
            if (ModelState.IsValid)
            {
                var respuestaApi = modelCategorias.RegistrarCategoria(entidad);

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
            var categorias = modelCategorias.ConsultarCategorias();
            return View(categorias);
        }

        [HttpPost]
        public ActionResult Actualizar(CategoriaEnt entidad)
        {

            if (ModelState.IsValid)
            {
                var respuestaApi = modelCategorias.ActualizarCategoria(entidad);

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
            var respuestaApi = modelCategorias.EliminarCategoria(q);

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