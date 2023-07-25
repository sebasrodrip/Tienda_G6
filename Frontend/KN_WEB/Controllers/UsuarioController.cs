using KN_WEB.Entities;
using KN_WEB.Models;
using OpenAI_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace KN_WEB.Controllers
{
    public class UsuarioController : Controller
    {
        UsuarioModel model = new UsuarioModel();

        public class UsuarioViewModel
        {
            public List<UsuarioEnt> Usuarios { get; set; }
            public List<RolEnt> Roles { get; set; }
        }

        public ActionResult Index()
        {
            var datos = model.ConsultarUsuarios();
            var roles = model.ConsultarRoles();

            var viewModel = new UsuarioViewModel
            {
                Usuarios = datos,
                Roles = roles,
            };

            return View(viewModel);
        }


    }
}
