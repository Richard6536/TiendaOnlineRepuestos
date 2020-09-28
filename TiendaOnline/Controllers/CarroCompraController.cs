using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiendaOnline.Models;

namespace TiendaOnline.Controllers
{
    public class CarroCompraController : Controller
    {
        TiendaOnlineContext db = new TiendaOnlineContext();

        // GET: CarroCompra
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult CarroCompra()
        {
            if (Session["Rol"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            int idUser = (int)Session["Id"];
            Carrocompra carroCompra = db.CarrosCompra.Where(c => c.Usuario.Id == idUser).FirstOrDefault();

            return View(carroCompra);
        }

    }
}