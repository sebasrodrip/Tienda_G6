using KN_WEB.Entities;
using KN_WEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KN_WEB.Controllers
{
    public class HomeController : Controller
    {
        UsuarioModel model = new UsuarioModel();

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public ActionResult SobreNosotros() {
            return View();

        }


        [HttpGet]
        public ActionResult Recuperar() 
        {
            return View();
        }

        [HttpPost]
        public ActionResult IniciarSesion(UsuarioEnt entidad)
        {
            //PROGRAMACION
            return RedirectToAction("Index","Home");
        }

        [HttpGet]
        public ActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegistrarUsuario(UsuarioEnt entidad)
        {
            //PROGRAMACION
            return RedirectToAction("Login", "Home");
        }

        [HttpPost]
        public ActionResult RecuperarContrasenna(UsuarioEnt entidad)
        {
            try
            {
                var resp = model.RecuperarContrasenna(entidad);

                if (resp)
                    return RedirectToAction("Login", "Home");
                else
                {
                    ViewBag.MsjPantalla = "No se ha podido recuperar su infomración";
                    return View("Recuperar");
                }
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

    }
}