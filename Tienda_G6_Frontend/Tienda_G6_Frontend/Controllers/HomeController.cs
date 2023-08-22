using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tienda_G6_Frontend.Entities;
using Tienda_G6_Frontend.Models;

namespace Tienda_G6_Frontend.Controllers
{
    public class HomeController : Controller
    {
        UsuarioModel model = new UsuarioModel();

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public ActionResult SobreNosotros()
        {
            return View();

        }

        [HttpGet]
        public ActionResult Recuperar()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Admin()
        {
            return View();
        }



        [HttpPost]
        public ActionResult IniciarSesion(UsuarioEnt entidad)
        {
            //PROGRAMACION
            try
            {
                entidad.Contrasenna = model.Encrypt(entidad.Contrasenna);
                var resp = model.IniciarSesion(entidad);

                if (resp != null)
                {
                    Session["IdUsuario"] = resp.IdUsuario.ToString();
                    Session["CorreoUsuario"] = resp.Email;
                    Session["NombreUsuario"] = resp.Nombre;
                    Session["RolUsuario"] = resp.NombreRol;
                    Session["IdRolUsuario"] = resp.IdRol;
                    Session["TokenUsuario"] = resp.Token;

                    return RedirectToAction("", "Inicio");
                }
                else
                {
                    ViewBag.MsjPantalla = "No se ha podido validar su información";
                    return View("Login");
                }
            }
            catch (Exception ex)
            {
                return View("Error"+ex);
            }
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegistrarUsuario(UsuarioEnt entidad)
        {

            try
            {
                entidad.Contrasenna = model.Encrypt(entidad.Contrasenna);
                entidad.IdRol = 2;
                entidad.Estado = true;

                var resp = model.RegistrarUsuario(entidad);

                if (resp > 0)
                    return RedirectToAction("Login", "Home");
                else
                {
                    ViewBag.MsjPantalla = "No se ha podido registrar su información";
                    return View("Registro");
                }
            }
            catch (Exception ex)
            {
                return View("Error"+ex);
            }
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
                return View("Error"+ex);
            }
        }

    }
}