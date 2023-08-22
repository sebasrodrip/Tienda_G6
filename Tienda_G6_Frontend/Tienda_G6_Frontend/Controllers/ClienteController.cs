using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tienda_G6_Frontend.Entities;
using Tienda_G6_Frontend.Models;

namespace Tienda_G6_Frontend.Controllers
{
    public class ClienteController : Controller
    {
        ClienteModel modelClientes = new ClienteModel();

        [HttpGet]
        public ActionResult Index()
        {
            var clientes = modelClientes.ConsultarClientes();
            return View(clientes);
        }

        [HttpPost]
        public ActionResult Agregar(ClienteEnt entidad)
        {
            if (ModelState.IsValid)
            {
                var respuestaApi = modelClientes.RegistrarCliente(entidad);

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

            var clientes = modelClientes.ConsultarClientes();
            return View(clientes);
        }

        [HttpPost]
        public ActionResult Actualizar(ClienteEnt entidad)
        {
            if (ModelState.IsValid)
            {
                var respuestaApi = modelClientes.ActualizarCliente(entidad);

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
            var respuestaApi = modelClientes.EliminarCliente(q);

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