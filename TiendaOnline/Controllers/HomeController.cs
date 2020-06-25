using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiendaOnline.Models;

namespace TiendaOnline.Controllers
{
    public class HomeController : Controller
    {
        TiendaOnlineContext db = new TiendaOnlineContext();

        //asi como giko que muestra todos los productos y permite filtrar por categoria
        //aqui se muestran todas las casas independiente de quien las subio


        public ActionResult Index(int page = 1, int categoriaId = -1)
        {
            List<Producto> productos = db.Productos.ToList();

            int productsPerPage = 12;
            int start = (page - 1) * productsPerPage;

            //Categorias
            IEnumerable<Categoria> categorias = db.Categorias.ToList();
            ViewBag.Categorias = categorias;
            if(categoriaId != -1)
            {
                productos = productos.Where(p => p.CategoriaId == categoriaId).ToList();
                ViewBag.CategoriaFiltrada = db.Categorias.Find(categoriaId).NombreCategoria;
            }

            //---------

            //Productos
            ViewBag.PageCount = Math.Ceiling(productos.Count() / (double)productsPerPage);
            IEnumerable<Producto> prods = productos.OrderBy(p => p.Id).Skip(start).Take(productsPerPage);
            ViewBag.PaginatedProducts = prods;

            return View(productos);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(String value, int categoriaId)
        {
            return RedirectToAction("Index", "Home", new { id = categoriaId });
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult CrearTienda()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CrearTienda(Tienda model)
        {
            if (Session["Id"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            ModelState.Clear();
            if (model.Nombre == null || model.Nombre == "")
                ModelState.AddModelError("Nombre", "El campo nombre es obligatorio");
            else 
            {
                Tienda repetido = db.Tienda.Where(u => u.Nombre.ToUpper() == model.Nombre.ToUpper()).FirstOrDefault();

                if (repetido != null)
                    ModelState.AddModelError("Nombre", "El nombre de Tienda ya se encuentra en uso");        
            }
            TryValidateModel(model);


            if (ModelState.IsValid == false)
            {
                return View(model);
            }

            Tienda tienda = Models.Tienda.CrearNuevaTienda(db, model);

            int userId = (int)Session["Id"];
            Usuario usuario = db.Usuarios.Find(userId);
            Usuario.EditarUsuario(db, usuario, tienda.Id, UsuarioTienda.RolEnTienda.Admin);

            UsuarioTienda usTienda = db.UsuariosTienda.Where(ut => ut.Usuario.Id == userId).FirstOrDefault();

            if (usTienda != null)
            {
                Session["TiendaId"] = usTienda.Tienda.Id;
                Session["TiendaNombre"] = usTienda.Tienda.Nombre;
            }

            return RedirectToAction("Index", "TiendaUser", new { id = tienda.Id});
        }


        public ActionResult Tienda(int? id, int page = 1, int categoriaId = -1)
        {
            Tienda tienda = db.Tienda.Find(id);

            if (tienda == null)
                return RedirectToAction("Index", "Home");

            IEnumerable<Producto> productosTienda = tienda.Productos;
            int productsPerPage = 12;
            int start = (page - 1) * productsPerPage;

            //Categorias
            ViewBag.Categorias = Categoria.buscarCategoriasPorTienda(db, productosTienda);
            if (categoriaId != -1) {
                
                productosTienda = productosTienda.Where(p => p.CategoriaId == categoriaId).ToList();
                ViewBag.CategoriaFiltrada = db.Categorias.Find(categoriaId).NombreCategoria;
            }

            ViewBag.PageCount = Math.Ceiling(productosTienda.Count() / (double)productsPerPage);
            productosTienda = productosTienda.OrderBy(p => p.Id).Skip(start).Take(productsPerPage);
            ViewBag.PaginatedProducts = productosTienda;

            return View(tienda);
        }

    }
}