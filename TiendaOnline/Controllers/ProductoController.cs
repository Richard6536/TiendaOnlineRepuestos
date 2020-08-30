using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiendaOnline.Models;

namespace TiendaOnline.Controllers
{
    public class ProductoController : Controller
    {
        TiendaOnlineContext db = new TiendaOnlineContext();
        //si no tiene tienda no puede usar nada de esto
        //hay excepciones como "ver producto"

        //controller para los usuarios comunes que crean productos en sus propias tiendas

        //crear/editar/eliminar
        //las categorias vienen por default y son creadas por admins... 

        // GET: Productos
        public ActionResult Index()
        {
            if (Session["Rol"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            if (Session["TiendaId"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            return View();
        }

        public ActionResult VerProducto(int id)
        {
            Producto producto = db.Productos.Find(id);
            producto.Comentarios.Reverse();
            return View(producto);
        }

        public ActionResult ListaProductos()
        {
            if (Session["Rol"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            if (Session["TiendaId"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            int tiendaId = (int)Session["TiendaId"];
            Tienda tienda = db.Tienda.Find(tiendaId);
            List<Producto> productosTienda = tienda.Productos.ToList();

            return View(productosTienda);
        }

        public ActionResult CrearProducto()
        {
            if (Session["Rol"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            if (Session["TiendaId"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            List<Categoria> categorias = db.Categorias.Where(p => p.TipoCategoria == Categoria.CategoriaTipo.Producto).ToList();
            categorias.Reverse();

            ViewBag.CategoriaId = new SelectList(categorias, "Id", "NombreCategoria");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CrearProducto(Producto model)
        {
            if (Session["Rol"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            if (Session["TiendaId"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            int tiendaId = (int)Session["TiendaId"];
            Tienda tienda = db.Tienda.Find(tiendaId);
            Producto.CrearProducto(model, tienda, db);

            return RedirectToAction("ListaProductos");
        }

        public ActionResult EditarProducto(int id)
        {
            if (Session["Rol"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            if (Session["TiendaId"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            Producto producto = db.Productos.Find(id);

            List<Categoria> categorias = db.Categorias.Where(c => c.TipoCategoria == Categoria.CategoriaTipo.Producto).ToList();
            categorias.Reverse();

            ViewBag.CategoriaId = new SelectList(categorias, "Id", "NombreCategoria", producto.Id);

            return View(producto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarProducto(Producto model)
        {
            if (Session["Rol"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            if (Session["TiendaId"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            int tiendaId = (int)Session["TiendaId"];
            Tienda tienda = db.Tienda.Find(tiendaId);

            Producto.EditarProducto(model, tienda, db);

            return RedirectToAction("ListaProductos");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarProducto(int id)
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

            Producto.EliminarProducto(db, tienda, id);

            return Json(new
            {
                exito = true,
                mensaje = "Eliminación exitosa."
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult VerImagenesProducto(int id)
        {
            if (Session["Rol"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            if (Session["TiendaId"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            int tiendaId = (int)Session["TiendaId"];
            Tienda tienda = db.Tienda.Find(tiendaId);
            Producto producto = tienda.Productos.Where(p => p.Id == id).FirstOrDefault();

            return View(producto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VerImagenesProducto(List<HttpPostedFileBase> files, int id)
        {
            if(files.Count == 1 && files.Contains(null))
                return RedirectToAction("VerImagenesProducto", new { id = id });

            if (files.Contains(null))
            {
                //me habia dado error al subir 2 imagens una vez y no pude repetir el problema
                TempData["msgError"] = "Hubo un error al subir imagenes. Se le recomienda subir de una en una";
                return RedirectToAction("VerImagenesProducto", new { id = id });
            }

            foreach(HttpPostedFileBase file in files)
            {
                if(file.ContentLength > Imagen.MAX_SIZE)
                {
                    TempData["msgError"] = "Una de las imagenes subidas supera el peso máximo de " + Imagen.MAX_SIZE_IN_MB + " MB";
                    return RedirectToAction("VerImagenesProducto", new { id = id });
                }

            }


            int tiendaId = (int)Session["TiendaId"];
            Tienda tienda = db.Tienda.Find(tiendaId);
            Producto producto = tienda.Productos.Where(p => p.Id == id).FirstOrDefault();

            Imagen.SubirImagenesProducto(files, db, producto);

            return RedirectToAction("VerImagenesProducto", new { id = id});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarImagenProducto(int id, int prodid)
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

            try
            {
                Producto prod = tienda.Productos.Where(p => p.Id == prodid).FirstOrDefault();
                Imagen img = prod.Imagenes.Where(i => i.Id == id).FirstOrDefault();

                if (img == null)
                    throw new Exception();

                Imagen.EliminarImagen(db, id);
            }
            catch
            {
                if (Session["Rol"] == null)
                {
                    return Json(new
                    {
                        exito = false,
                        mensaje = "Hubo un problema encontrando el registro a eliminar."
                    }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new
            {
                exito = true,
                mensaje = "Eliminación exitosa."
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Comentar(int idProducto, string comentario)
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

            Comentario cp = Producto.EnviarComentario(idProducto, idUsuario, comentario, db);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResponderComentarioTienda(int idComentario, string respuesta)
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
            ComentarioRespuesta cr = Comentario.ResponderComentario(db, idComentario, idUsuario, respuesta);

            String Fecha = cr.Fecha.ToString();
            String NombreUsuario = cr.Usuario.Nombre;
            String Mensaje = cr.Mensaje;
            int Id = idComentario;

            System.Web.Script.Serialization.JavaScriptSerializer oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            string sJSON = oSerializer.Serialize(
                new
                {
                    Id,
                    NombreUsuario,
                    Mensaje,
                    Fecha
                }
            );

            return Json(new { exito = true, respuesta = sJSON });
        }

        public ActionResult EliminarComentario(int id, int producto_id)
        {
            Producto prod = db.Productos.Find(producto_id);
            Producto.EliminarComentario(db, prod, id);

            return RedirectToAction("VerProducto", new { id = producto_id });
        }

        public ActionResult EliminarRespuesta(int id, int producto_id, int comentario_id)
        {

            if (Session["Rol"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            if (Session["TiendaId"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            Comentario comentario = db.Comentarios.Find(comentario_id);
            Comentario.EliminarComentarioRespuesta(db, comentario, id);

            return RedirectToAction("VerProducto", new { id = producto_id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarComentarioAction(int id, int producto_id, string mensajeEdit)
        {
            if (Session["Rol"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            if (Session["TiendaId"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            Producto prod = db.Productos.Find(producto_id);
            Tuple<Comentario, Boolean> data = Producto.EditarComentario(db, prod, id, mensajeEdit);

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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarRespuestaAction(int comentarioRespuesta_id, int comentario_id, string mensajeEdit)
        {
            Comentario comentario = db.Comentarios.Find(comentario_id);
            Tuple<ComentarioRespuesta, Boolean> data = Comentario.EditarComentarioRespuesta(db, comentario, comentarioRespuesta_id, mensajeEdit);

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
                    comentarioRespuesta_id,
                    NombreUsuario,
                    Mensaje,
                    Fecha
                }
            );

            return Json(new { exito = true, respuesta = sJSON });
        }
    }
}