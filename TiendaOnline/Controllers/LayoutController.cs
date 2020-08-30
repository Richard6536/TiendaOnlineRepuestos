using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiendaOnline.Models;

namespace TiendaOnline.Controllers
{
    public class LayoutController : Controller
    {
        TiendaOnlineContext db = new TiendaOnlineContext();

        // GET: Layout
        public ActionResult GetCategories()
        {
            return Content("Some data");
        }

        public ActionResult _CategoriesPartial()
        {
            //Categorias
            IEnumerable<Categoria> categoriasProducto = db.Categorias.Where(c => c.TipoCategoria == Categoria.CategoriaTipo.Producto).ToList();
            IEnumerable<Categoria> categoriasServicio = db.Categorias.Where(c => c.TipoCategoria == Categoria.CategoriaTipo.Servicio).ToList();
            ViewBag.categoriasProducto = categoriasProducto;
            ViewBag.categoriasServicio = categoriasServicio;

            //---------
            return PartialView();
        }

    }
}