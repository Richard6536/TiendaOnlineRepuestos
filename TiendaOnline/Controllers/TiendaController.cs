using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiendaOnline.Clases;
using TiendaOnline.Models;

namespace TiendaOnline.Controllers
{
    public class TiendaController : Controller
    {

        //controlador con las vistas al visitar la tienda de cierto user
        TiendaOnlineContext db = new TiendaOnlineContext();

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


        public ActionResult Tienda(int? id, int page = 1)
        {
            Tienda tienda = db.Tienda.Find(id);

            if (tienda == null)
                return RedirectToAction("Index", "Home");

            List<Producto> products = tienda.Productos;
            int productsPerPage = 12;
            int start = (page - 1) * productsPerPage;

            ViewBag.PageCount = Math.Ceiling(products.Count() / (double)productsPerPage);
            IEnumerable<Producto> prods = tienda.Productos.OrderBy(p => p.Id).Skip(start).Take(productsPerPage);
            ViewBag.PaginatedProducts = prods;

            return View(tienda);
        }


        public ActionResult _PartialProducts(TiendaHome.ViewProductType viewProductType, IEnumerable<Producto> productos)
        {
            ViewBag.ViewProductType = viewProductType;
            return PartialView(productos);
        }

        public ActionResult _PartialServices(TiendaHome.ViewProductType viewProductType, IEnumerable<Servicio> servicios)
        {
            ViewBag.ViewProductType = viewProductType;
            return PartialView(servicios);
        }

        public ActionResult _PartialTiendas(TiendaHome.ViewProductType viewProductType, IEnumerable<Tienda> tiendas)
        {
            ViewBag.ViewProductType = viewProductType;
            return PartialView(tiendas);
        }

    }
}