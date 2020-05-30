using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiendaOnline.Models;

namespace TiendaOnline.Controllers
{
    public class TiendaUserController : Controller
    {
        TiendaOnlineContext db = new TiendaOnlineContext();
        //controlador con las vistas al visitar la tienda de cierto user

        public ActionResult Index(int id)
        {
            if (Session["Rol"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            if (Session["TiendaId"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            int idTienda = (int)Session["TiendaId"];
            Tienda tienda = db.Tienda.Find(id);

            if (tienda.Id != idTienda)
                return RedirectToAction("Index", "Home");

            return View(tienda);
        }
    }
}