using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tienda_G6_Frontend.Entities;
using Tienda_G6_Frontend.Models;

namespace Tienda_G6_Frontend.Controllers
{
    public class UsuarioController : Controller
    {
        UsuarioModel model = new UsuarioModel();
        RolModel modelR = new RolModel();

        public class UsuarioViewModel
        {
            public List<UsuarioEnt> Usuarios { get; set; }
            public List<RolEnt> Roles { get; set; }
        }

        [HttpGet]
        public ActionResult Index()
        {
            var datos = model.ConsultarUsuarios();
            var roles = modelR.ConsultarRoles();

            var viewModel = new UsuarioViewModel
            {
                Usuarios = datos,
                Roles = roles,
            };

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Agregar(UsuarioEnt entidad)
        {
            if (ModelState.IsValid)
            {
                var respuestaApi = model.AgregarUsuario(entidad);

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

            var creditos = model.ConsultarUsuarios();
            return View(creditos);
        }

        [HttpPost]
        public ActionResult Actualizar(UsuarioEnt entidad)
        {
            if (ModelState.IsValid)
            {
                var respuestaApi = model.ActualizarUsuario(entidad);

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
            var respuestaApi = model.EliminarUsuario(q);

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
