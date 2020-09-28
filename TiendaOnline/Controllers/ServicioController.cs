using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiendaOnline.Models;

namespace TiendaOnline.Controllers
{
    public class ServicioController : Controller
    {
        TiendaOnlineContext db = new TiendaOnlineContext();
        public ActionResult Index(int page = 1, int categoriaId = -1)
        {
            List<Servicio> servicios = db.Servicios.ToList();

            int productsPerPage = 12;
            int start = (page - 1) * productsPerPage;

            //Categorias
            IEnumerable<Categoria> categorias = db.Categorias.Where(c => c.TipoCategoria == Categoria.CategoriaTipo.Servicio).ToList();
            ViewBag.Categorias = categorias;
            if (categoriaId != -1)
            {
                servicios = servicios.Where(p => p.CategoriaId == categoriaId).ToList();
                ViewBag.CategoriaFiltrada = db.Categorias.Find(categoriaId).NombreCategoria;
            }

            //---------

            //Productos
            ViewBag.PageCount = Math.Ceiling(servicios.Count() / (double)productsPerPage);
            IEnumerable<Servicio> prods = servicios.OrderBy(p => p.Id).Skip(start).Take(productsPerPage);
            ViewBag.PaginatedProducts = prods;

            return View(servicios);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(String value, int categoriaId)
        {
            return RedirectToAction("Index", "Servicio", new { id = categoriaId });
        }

        public ActionResult VerServicio(int id)
        {
            Servicio servicio = db.Servicios.Find(id);
            servicio.Comentarios.Reverse();
            return View(servicio);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VerServicio(int idServicio, string marca, string modelo, int year, string comentario)
        {
            if (Session["Rol"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            int idUser = (int)Session["Id"];

            //Carrocompra.CrearCarroCompra(idUser, db);
            ProductoCarro productoCarro = ProductoCarro.CrearProductoCarro(idServicio, idUser, 1, db, true);
            Carrocompra.AgregarProductoAlCarroCompra(idUser, productoCarro, db);

            RealizarCompra(idUser, marca, modelo, year, comentario);
            //return RedirectToAction("RealizarCompra", "CarroCompra");
            //return RedirectToAction("VerServicio", "Servicio", new { id = idServicio });

            System.Web.Script.Serialization.JavaScriptSerializer oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            string sJSON = oSerializer.Serialize(
                new
                {
                    NombreServicio = productoCarro.Servicio.Nombre
                }
            );

            return Json(new { exito = true, respuesta = sJSON });
        }

        public void RealizarCompra(int idUsuario, string marca, string modelo, int year, string comentario)
        {

            Usuario usuario = db.Usuarios.Where(u => u.Id == idUsuario).FirstOrDefault();
            Carrocompra carroCompra = usuario.CarroCompra.First();

            foreach (ProductoCarro productoCarroTienda in carroCompra.ProductosCarro)
            {
                List<Servicio> serviciosTienda = new List<Servicio>();
                int idTienda = productoCarroTienda.Servicio.Tienda.Id;

                foreach (ProductoCarro productoCarro in carroCompra.ProductosCarro)
                {
                    if (idTienda == productoCarro.Servicio.Tienda.Id)
                    {
                        serviciosTienda.Add(productoCarro.Servicio);
                    }

                }

                SolicitarCotizacion(serviciosTienda, idTienda, marca, modelo, year, comentario);
            }
        }

        public bool SolicitarCotizacion(List<Servicio> servicios, int idTienda, string marca, string modelo, int year, string comentario)
        {
            if (Session["Rol"] == null)
                RedirectToAction("IniciarSesion", "Login");
            if (Session["Id"] == null)
                RedirectToAction("IniciarSesion", "Login");

            int usuarioId = (int)Session["Id"];
            Usuario usuario = db.Usuarios.Where(u => u.Id == usuarioId).FirstOrDefault();
            //Servicio servicio = db.Servicios.Where(u => u.Id == servicioId).FirstOrDefault();

            Vehiculo vehiculo = Vehiculo.CrearVehiculo(db, marca, modelo, year);
            SolicitudCotizacion.CrearSolicitud(db, servicios, usuario, idTienda, vehiculo, comentario);

            return true;
        }

        public ActionResult ListaServicios()
        {
            if (Session["Rol"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            if (Session["TiendaId"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            int tiendaId = (int)Session["TiendaId"];
            Tienda tienda = db.Tienda.Find(tiendaId);
            List<Servicio> serviciosTienda = tienda.Servicios.ToList();

            return View(serviciosTienda);
        }

        public ActionResult CrearServicio()
        {
            if (Session["Rol"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            if (Session["TiendaId"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            List<Categoria> categorias = db.Categorias.Where(s => s.TipoCategoria == Categoria.CategoriaTipo.Servicio).ToList();
            categorias.Reverse();

            ViewBag.CategoriaId = new SelectList(categorias, "Id", "NombreCategoria");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CrearServicio(Servicio model)
        {
            if (Session["Rol"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            if (Session["TiendaId"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            int tiendaId = (int)Session["TiendaId"];
            Tienda tienda = db.Tienda.Find(tiendaId);
            Servicio.CrearServicio(model, tienda, db);

            return RedirectToAction("ListaServicios");
        }

        public ActionResult EditarServicio(int id)
        {
            if (Session["Rol"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            if (Session["TiendaId"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            Servicio servicio = db.Servicios.Find(id);

            List<Categoria> categorias = db.Categorias.Where(c => c.TipoCategoria == Categoria.CategoriaTipo.Servicio).ToList();
            categorias.Reverse();

            ViewBag.CategoriaId = new SelectList(categorias, "Id", "NombreCategoria", servicio.Id);

            return View(servicio);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarServicio(Servicio model)
        {
            if (Session["Rol"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            if (Session["TiendaId"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            int tiendaId = (int)Session["TiendaId"];
            Tienda tienda = db.Tienda.Find(tiendaId);

            Servicio.EditarServicio(model, tienda, db);

            return RedirectToAction("ListaServicios");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarServicio(int id)
        {

            if (Session["Rol"] == null)
            {
                return Json(new
                {
                    exito = false,
                    mensaje = "La sesión ha terminado."
                }, JsonRequestBehavior.AllowGet);
            }

            if (Session["Rol"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            if (Session["TiendaId"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            int tiendaId = (int)Session["TiendaId"];
            Tienda tienda = db.Tienda.Find(tiendaId);

            Servicio.EliminarServicio(db, tienda, id);

            return Json(new
            {
                exito = true,
                mensaje = "Eliminación exitosa."
            }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult VerImagenesServicio(int id)
        {
            if (Session["Rol"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            if (Session["TiendaId"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            int tiendaId = (int)Session["TiendaId"];
            Tienda tienda = db.Tienda.Find(tiendaId);
            Servicio servicio = tienda.Servicios.Where(p => p.Id == id).FirstOrDefault();

            return View(servicio);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VerImagenesServicio(List<HttpPostedFileBase> files, int id)
        {
            if (files.Count == 1 && files.Contains(null))
                return RedirectToAction("VerImagenesServicio", new { id = id });

            if (files.Contains(null))
            {
                //me habia dado error al subir 2 imagens una vez y no pude repetir el problema
                TempData["msgError"] = "Hubo un error al subir imagenes. Se le recomienda subir de una en una";
                return RedirectToAction("VerImagenesServicio", new { id = id });
            }

            foreach (HttpPostedFileBase file in files)
            {
                if (file.ContentLength > Imagen.MAX_SIZE)
                {
                    TempData["msgError"] = "Una de las imagenes subidas supera el peso máximo de " + Imagen.MAX_SIZE_IN_MB + " MB";
                    return RedirectToAction("VerImagenesServicio", new { id = id });
                }

            }


            int tiendaId = (int)Session["TiendaId"];
            Tienda tienda = db.Tienda.Find(tiendaId);
            Servicio servicio = tienda.Servicios.Where(p => p.Id == id).FirstOrDefault();

            Imagen.SubirImagenesServicio(files, db, servicio);

            return RedirectToAction("VerImagenesServicio", new { id = id });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Comentar(int idServicio, string comentario)
        {
            if (Session["Rol"] == null)
            {
                return Json(new
                {
                    exito = false,
                    respuesta = "Necesita iniciar sesión para comentar."
                }, JsonRequestBehavior.AllowGet);
            }

            int idUsuario = (int)Session["Id"];

            Comentario cp = Servicio.EnviarComentario(idServicio, idUsuario, comentario, db);
            String Fecha = cp.Fecha.ToString();
            String NombreUsuario = cp.Usuario.Nombre;
            String Mensaje = cp.Mensaje;

            System.Web.Script.Serialization.JavaScriptSerializer oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            string sJSON = oSerializer.Serialize(
                new
                {
                    NombreUsuario,
                    Mensaje,
                    Fecha
                }
            );

            return Json(new { exito = true, respuesta = sJSON });
        }

        public ActionResult EliminarComentario(int id, int servicio_id)
        {
            Servicio serv = db.Servicios.Find(servicio_id);
            Servicio.EliminarComentario(db, serv, id);

            return RedirectToAction("VerProducto", new { id = servicio_id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarComentarioAction(int id, int producto_id, string mensajeEdit)
        {
            Servicio serv = db.Servicios.Find(producto_id);
            Tuple<Comentario, Boolean> data = Servicio.EditarComentario(db, serv, id, mensajeEdit);

            if (data.Item2 == false)
            {
                //Comentario no encontrado. Hacer algo.
            }

            String NombreUsuario = data.Item1.Usuario.NombreUsuario;
            String Mensaje = data.Item1.Mensaje;
            String Fecha = data.Item1.Fecha.ToString();

            System.Web.Script.Serialization.JavaScriptSerializer oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            string sJSON = oSerializer.Serialize(
                new
                {
                    id,
                    NombreUsuario,
                    Mensaje,
                    Fecha
                }
            );

            return Json(new { exito = true, respuesta = sJSON });
        }
    }
}