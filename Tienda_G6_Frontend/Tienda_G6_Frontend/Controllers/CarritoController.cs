using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tienda_G6_Frontend.Entities;
using Tienda_G6_Frontend.Models;

namespace Tienda_G6_Frontend.Controllers
{
    public class CarritoController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        CarritoModel carrito = new CarritoModel();
        ArticuloModel articulo = new ArticuloModel();

        [HttpGet]
        public ActionResult AgregarArticuloCarrito(long q)
        {
            CarritoEnt entidad = new CarritoEnt();
            entidad.Fecha = DateTime.Now;
            entidad.IdArticulo = q;
            entidad.IdCliente = long.Parse(Session["IdUsuario"].ToString());

            var respuesta = carrito.AgregarArticuloCarrito(entidad);
            ActualizarDatosSesion();

            if (respuesta > 0)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var articulos = articulo.ConsultarArticulos();
                ViewBag.MsjPantalla = "El Articulo ya fue comprado o agregado a su carrito";
                return View("../Home/Index", articulos);
            }
        }


        [HttpGet]
        public ActionResult RemoverArticuloCarrito(long q)
        {
            var respuesta = carrito.RemoverArticuloCarrito(q);
            ActualizarDatosSesion();

            if (respuesta > 0)
            {
                return RedirectToAction("VerCarrito", "Carrito");
            }
            else
            {
                ViewBag.MsjPantalla = "No se pudo remover el Articulo de su carrito";
                return View("VerCarrito");
            }
        }


        public void ActualizarDatosSesion()
        {
            var datos = carrito.ConsultarArticulosCliente(long.Parse(Session["IdUsuario"].ToString()));
            Session["CantidadArticulo"] = datos.Count();
            Session["SubTotalArticulos"] = datos.Sum(x => x.Precio);
            Session["TotalArticulos"] = datos.Sum(x => x.Precio) + (datos.Sum(x => x.Precio) * 0.13M);
        }


        [HttpGet]
        public ActionResult VerCarrito()
        {
            var datos = carrito.ConsultarArticulosCliente(long.Parse(Session["IdUsuario"].ToString()));
            return View(datos);
        }


        [HttpGet]
        public ActionResult VerMisArticuloss()
        {
            var datos = carrito.ConsultarArticulosCliente(long.Parse(Session["IdUsuario"].ToString()));
            return View(datos);
        }


        [HttpPost]
        public ActionResult ConfirmarPago()
        {
            CarritoEnt entidad = new CarritoEnt();
            entidad.IdCliente = long.Parse(Session["IdUsuario"].ToString());

            carrito.PagarArticuloCarrito(entidad);
            return RedirectToAction("Index", "Home");
        }

    }
}