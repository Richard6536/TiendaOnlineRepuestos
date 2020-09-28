using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiendaOnline.Models;
using TiendaOnline.Clases;
using Newtonsoft.Json;
using System.Globalization;

namespace TiendaOnline.Controllers
{
    public class HomeController : Controller
    {
        TiendaOnlineContext db = new TiendaOnlineContext();

        //asi como giko que muestra todos los productos y permite filtrar por categoria
        //aqui se muestran todas las casas independiente de quien las subio

        public ActionResult Portal()
        {
            return View();
        }

        public ActionResult Index(int page = 1, int categoriaId = -1)
        {

            List<Producto> productos = db.Productos.Where(t => t.Tienda.EstadoTienda == Models.Tienda.Estado.Publicada).ToList();

            int productsPerPage = 12;
            int start = (page - 1) * productsPerPage;

            ViewBag.Tiendas = db.Tienda.ToList();

            //Categorias
            IEnumerable<Categoria> categorias = db.Categorias.Where(c => c.TipoCategoria == Categoria.CategoriaTipo.Producto).ToList();
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

        public ActionResult _CrearTienda()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _CrearTienda(Tienda model)
        {

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

            if (Session["Id"] == null)
                return RedirectToAction("Registrate", "Login", new { nombreTienda = model.Nombre });

            else
            {
                int userId = (int)Session["Id"];
                Tienda tienda = Models.Tienda.CrearNuevaTienda(db, model);
                UsuarioTienda usTienda = UsuarioTienda.CrearUsuarioTienda(db, userId, tienda);

                if (usTienda != null)
                {
                    Session["TiendaId"] = usTienda.Tienda.Id;
                    Session["TiendaNombre"] = usTienda.Tienda.Nombre;
                }

                return RedirectToAction("Index", "TiendaUser", new { id = tienda.Id });
            }

        }

        [HttpPost]
        public ActionResult OrdenarProductos(Producto.OrderByType orderType)
        {
            Session["OrderByProduct"] = orderType;
            return Json(new
            {
                exito = true,
                productos = "Ok"
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ListarProductosGridList(TiendaHome.ViewProductType productView)
        {
            Session["ViewProductType"] = productView;
            return Json(new
            {
                exito = true,
                productos = "Ok"
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Tienda(int? id, int page = 1, int categoriaId = -1, string seccion = "producto", string search = "")
        {
            Tienda tienda = db.Tienda.Find(id);
            IEnumerable<Producto> productosTienda = null;
            IEnumerable<Servicio> serviciosTienda = null;

            TiendaHome tiendaHome = new TiendaHome();
            tiendaHome.OrderByProductName = "";
            tiendaHome.ViewProductEnumType = TiendaHome.ViewProductType.Grid;
            tiendaHome.ViewTypeName = "Grid";

            if (tienda == null)
                return RedirectToAction("Index", "Home");

            int productsPerPage = 12;
            int start = (page - 1) * productsPerPage;

            if (seccion.Equals("producto"))
            {

                productosTienda = tienda.Productos;

                //Verificar si productos deben estar ordenados
                if (Session["OrderByProduct"] != null)
                {
                    Tuple<IEnumerable<Producto>, string> tupleSort = Producto.SortProduct(productosTienda, (Producto.OrderByType)Session["OrderByProduct"]);
                    productosTienda = tupleSort.Item1;
                    tiendaHome.OrderByProductName = tupleSort.Item2;
                }

                //Verificar si productos están en List o Grid
                if (Session["ViewProductType"] != null)
                {
                    tiendaHome.ViewProductEnumType = (TiendaHome.ViewProductType)Session["ViewProductType"];
                    if ((TiendaHome.ViewProductType)Session["ViewProductType"] == TiendaHome.ViewProductType.Grid)
                        tiendaHome.ViewTypeName = "Grid";
                    else if((TiendaHome.ViewProductType)Session["ViewProductType"] == TiendaHome.ViewProductType.List)
                        tiendaHome.ViewTypeName = "List";
                }

                //Realiza una busqueda
                if (!search.Equals(""))
                {
                    Tuple<List<Producto>, List<Servicio>, List<Tienda>, Boolean, Busqueda.BusquedaTipo, string> productosFiltrados = Busqueda.BuscarProductosPorCategoria(db, -1, search, seccion, true, tienda);
                    productosTienda = productosFiltrados.Item1;
                }

                //Categorias
                ViewBag.Categorias = Busqueda.buscarCategoriasPorTienda(db, tienda.Productos, null, true);

                if (categoriaId != -1)
                {
                    productosTienda = productosTienda.Where(p => p.CategoriaId == categoriaId).ToList();
                    ViewBag.CategoriaFiltrada = db.Categorias.Find(categoriaId).NombreCategoria;
                }

                ViewBag.PageCount = Math.Ceiling(productosTienda.Count() / (double)productsPerPage);
                productosTienda = productosTienda.Skip(start).Take(productsPerPage);
                ViewBag.PaginatedProducts = productosTienda;
                ViewBag.Seccion = seccion;

                tiendaHome.Tienda = tienda;
                tiendaHome.Productos = productosTienda;

            }
            else if (seccion.Equals("servicio"))
            {
                serviciosTienda = tienda.Servicios;

                //Realiza una busqueda
                if (!search.Equals(""))
                {
                    Tuple<List<Producto>, List<Servicio>, List<Tienda>, Boolean, Busqueda.BusquedaTipo, string> serviciosFiltrados = Busqueda.BuscarProductosPorCategoria(db, -1, search, seccion, true, tienda);
                    serviciosTienda = serviciosFiltrados.Item2;
                }

                //Categorias
                ViewBag.Categorias = Busqueda.buscarCategoriasPorTienda(db, tienda.Productos, null, true);

                if (categoriaId != -1)
                {

                    serviciosTienda = serviciosTienda.Where(p => p.CategoriaId == categoriaId).ToList();
                    ViewBag.CategoriaFiltrada = db.Categorias.Find(categoriaId).NombreCategoria;
                }

                ViewBag.PageCount = Math.Ceiling(serviciosTienda.Count() / (double)productsPerPage);
                serviciosTienda = serviciosTienda.OrderBy(p => p.Id).Skip(start).Take(productsPerPage);
                ViewBag.PaginatedProducts = serviciosTienda;
                ViewBag.Seccion = seccion;
            }

            //SI LA TIENDA ESTÁ OCULTA, SOLO LA PUEDE VER EL ADMINISTRADOR.
            if (tiendaHome.Tienda.EstadoTienda == Models.Tienda.Estado.Oculta)
            {
                if (Session["Id"] == null)
                    return RedirectToAction("IniciarSesion", "Login");

                int idUsuario = (int)Session["Id"];
                UsuarioTienda existeUsuarioEnTienda = tiendaHome.Tienda.UsuariosTienda.Where(us => us.Usuario.Id == idUsuario).FirstOrDefault();

                if (existeUsuarioEnTienda == null)
                    return RedirectToAction("Index", "Home");
                else
                    return View(tiendaHome);
            }

            return View(tiendaHome);
        }

        public ActionResult BuscarProductos(int tiendaId, string productoBusqueda, string seccion)
        {

            //Tuple<List<Producto>, Boolean> productosFiltrados = Categoria.BuscarProductosPorCategoria(db, -1, productoBusqueda);

            return View();
        }

        public ActionResult CalendarExample()
        {
            return View();
        }

        public ActionResult CalendarExampleDXDraggableEX()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Search(string seccion, int categoria_field, string prod_field, string marca_field, string modelo_field, int? year_field)
        {
            Tuple<List<Producto>, List<Servicio>, List<Tienda>, Boolean, Busqueda.BusquedaTipo, string> repuestosServiciosTiendasFiltrados = Busqueda.BuscarProductosPorCategoriaConModeloMarcaYear(db, categoria_field, prod_field, modelo_field, marca_field, year_field, seccion, false, null);

            Busqueda busquedaResult = new Busqueda();
            busquedaResult.BusquedaText = repuestosServiciosTiendasFiltrados.Item6;
            busquedaResult.Productos = repuestosServiciosTiendasFiltrados.Item1;
            busquedaResult.Servicios = repuestosServiciosTiendasFiltrados.Item2;
            busquedaResult.Tiendas = repuestosServiciosTiendasFiltrados.Item3;

            return View(busquedaResult);
        }
    }
}