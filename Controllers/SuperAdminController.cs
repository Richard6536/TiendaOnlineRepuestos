using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiendaOnline.Models;

namespace TiendaOnline.Controllers
{   
    public class SuperAdminController : Controller
    {
        TiendaOnlineContext db = new TiendaOnlineContext();

        //CREAR/EDITAR/ELIMIAR
        //USUARIOS/TIENDAS/CATEGORIAS DE PRODUCTOS

        //usuarios se pueden asociar a tiendas

        // GET: SuperAdmin
        public ActionResult Index()
        {
            if (Session["Rol"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            int rolSession = (int)Session["Rol"];
            if (rolSession != (int)Usuario.TipoUsuario.SuperAdmin)
                return RedirectToAction("IniciarSesion", "Login");

            return View();
        }

        #region -----USUARIOS-----

        public ActionResult CrearUsuario()
        {
            if (Session["Rol"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            int rolSession = (int)Session["Rol"];
            if (rolSession != (int)Usuario.TipoUsuario.SuperAdmin)
                return RedirectToAction("IniciarSesion", "Login");

            List<Tienda> tiendas = db.Tienda.ToList();
            tiendas.Add(new Tienda() { Id = 0, Nombre = "No asignar" });
            tiendas.Reverse();

            ViewBag.TiendaId = new SelectList(tiendas, "Id", "Nombre");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CrearUsuario(Usuario model, int TiendaId)
        {
            if (model.NombreUsuario != null)
                model.NombreUsuario = model.NombreUsuario.ToUpper();
            if (model.Email != null)
                model.Email = model.Email.ToUpper();

            Usuario repetido = db.Usuarios.Where(u => u.NombreUsuario == model.NombreUsuario).FirstOrDefault();
            Usuario mailRepetido = db.Usuarios.Where(u => u.Email == model.Email).FirstOrDefault();

            ModelState.Clear();
            if (repetido != null || mailRepetido != null)
            {
                if (repetido != null)
                    ModelState.AddModelError("NombreUsuario", "El nombre de usuario ya se encuentra en uso");
                if (mailRepetido != null)
                    ModelState.AddModelError("Email", "El email ya se encuentra en uso");
            }
            TryValidateModel(model);
            if (ModelState.IsValid == false)
            {
                List<Tienda> tiendas = db.Tienda.ToList();

                tiendas.Add(new Tienda() { Id = 0, Nombre = "No asignar" });
                tiendas.Reverse();

                ViewBag.TiendaId = new SelectList(tiendas, "Id", "Nombre");

                return View(model);
            }

            //Se supone que deberia mandar el RolEnTienda que escogí en la vista
            Usuario.CrearNuevoUsuario(db, model, TiendaId, model.RolEnTienda);

            return RedirectToAction("ListaUsuarios");
        }

        public ActionResult EditarUsuario(int id)
        {
            if (Session["Rol"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            int rolSession = (int)Session["Rol"];
            if (rolSession != (int)Usuario.TipoUsuario.SuperAdmin)
                return RedirectToAction("IniciarSesion", "Login");

            List<Tienda> tiendas = db.Tienda.ToList();
            tiendas.Add(new Tienda() { Id = 0, Nombre = "No asignar" });
            tiendas.Reverse();


            Usuario user = db.Usuarios.Find(id);
            UsuarioTienda ust = null;
            if(user.UsuarioTiendas.Count > 0)
            {
                ust = user.UsuarioTiendas.First();
                user.RolEnTienda = ust.RolTienda;

                ViewBag.TiendaId = new SelectList(tiendas, "Id", "Nombre", ust.Tienda.Id);
            }
            else
            {
                ViewBag.TiendaId = new SelectList(tiendas, "Id", "Nombre");
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarUsuario(Usuario model, int TiendaId)
        {
            if (model.NombreUsuario != null)
                model.NombreUsuario = model.NombreUsuario.ToUpper();
            if (model.Email != null)
                model.Email = model.Email.ToUpper();

            Usuario repetido = db.Usuarios.Where(u => u.NombreUsuario == model.NombreUsuario && u.Id != model.Id).FirstOrDefault();
            Usuario mailRepetido = db.Usuarios.Where(u => u.Email == model.Email && u.Id != model.Id).FirstOrDefault();

            ModelState.Clear();
            model.Password = "asdf"; //para evitar que el ModelState falle por "ser obligatorio"
            if (repetido != null || mailRepetido != null)
            {
                if (repetido != null)
                    ModelState.AddModelError("NombreUsuario", "El nombre de usuario ya se encuentra en uso");
                if (mailRepetido != null)
                    ModelState.AddModelError("Email", "El email ya se encuentra en uso");
            }

            TryValidateModel(model);

            if (ModelState.IsValid == false)
            {
                List<Tienda> tiendas = db.Tienda.ToList();
                UsuarioTienda ust = db.UsuariosTienda.Where(ut => ut.Usuario.Id == model.Id).FirstOrDefault();

                tiendas.Add(new Tienda() { Id = 0, Nombre = "No asignar" });
                tiendas.Reverse();

                if (ust != null)
                {
                    ViewBag.TiendaId = new SelectList(tiendas, "Id", "Nombre", ust.Tienda.Id);
                }
                else
                {
                    ViewBag.TiendaId = new SelectList(tiendas, "Id", "Nombre");
                }

                return View(model);
            }

            //Se supone que deberia mandar el RolEnTienda que escogí en la vista
            Usuario.EditarUsuario(db, model, TiendaId, model.RolEnTienda);

            return RedirectToAction("ListaUsuarios");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarUsuario(int id)
        {

            if (Session["Rol"] == null)
            {
                return Json(new
                {
                    exito = false,
                    mensaje = "La sesión ha terminado."
                }, JsonRequestBehavior.AllowGet);
            }

            int rolSession = (int)Session["Rol"];
            if (rolSession != (int)Usuario.TipoUsuario.SuperAdmin)
            {
                return Json(new
                {
                    exito = false,
                    mensaje = "Rol no autorizado."
                }, JsonRequestBehavior.AllowGet);
            }

            Usuario.EliminarUsuario(db, id);

            return Json(new
            {
                exito = true,
                mensaje = "Eliminación exitosa."
            }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ListaUsuarios()
        {
            if (Session["Rol"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            int rolSession = (int)Session["Rol"];
            if (rolSession != (int)Usuario.TipoUsuario.SuperAdmin)
                return RedirectToAction("IniciarSesion", "Login");


            List<Usuario> usuarios = db.Usuarios.ToList();
            return View(usuarios);
        }

        #endregion

        #region -----TIENDAS-----
        public ActionResult CrearTienda()
        {
            if (Session["Rol"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            int rolSession = (int)Session["Rol"];
            if (rolSession != (int)Usuario.TipoUsuario.SuperAdmin)
                return RedirectToAction("IniciarSesion", "Login");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CrearTienda(Tienda model)
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

            Tienda.CrearNuevaTienda(db, model);

            return RedirectToAction("ListaTiendas");
        }

        public ActionResult EditarTienda(int id)
        {
            if (Session["Rol"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            int rolSession = (int)Session["Rol"];
            if (rolSession != (int)Usuario.TipoUsuario.SuperAdmin)
                return RedirectToAction("IniciarSesion", "Login");

            Tienda tienda = db.Tienda.Find(id);
            return View(tienda);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarTienda(Tienda model)
        {
            ModelState.Clear();
            if (model.Nombre == null || model.Nombre == "")
                ModelState.AddModelError("Nombre", "El campo nombre es obligatorio");
            else
            {
                Usuario repetido = db.Usuarios.Where(u => u.Nombre == model.Nombre && u.Id != model.Id).FirstOrDefault();

                if (repetido != null)
                    ModelState.AddModelError("Nombre", "El nombre de Tienda ya se encuentra en uso");
            }
            TryValidateModel(model);

            if (ModelState.IsValid == false)
            {
                return View(model);
            }

            Tienda.EditarTienda(db, model);

            return RedirectToAction("ListaTiendas");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarTienda(int id)
        {

            if (Session["Rol"] == null)
            {
                return Json(new
                {
                    exito = false,
                    mensaje = "La sesión ha terminado."
                }, JsonRequestBehavior.AllowGet);
            }


            int rolSession = (int)Session["Rol"];
            if (rolSession != (int)Usuario.TipoUsuario.SuperAdmin)
            {
                return Json(new
                {
                    exito = false,
                    mensaje = "Rol no autorizado."
                }, JsonRequestBehavior.AllowGet);
            }

            Tienda.EliminarTienda(db, id);

            return Json(new
            {
                exito = true,
                mensaje = "Eliminación exitosa."
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListaTiendas()
        {
            if (Session["Rol"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            int rolSession = (int)Session["Rol"];
            if (rolSession != (int)Usuario.TipoUsuario.SuperAdmin)
                return RedirectToAction("IniciarSesion", "Login");

            List<Tienda> tiendas = db.Tienda.ToList();
            return View(tiendas);
        }

        #endregion

        #region -----CATEGORIAS-----

        public ActionResult CrearCategoria()
        {
            if (Session["Rol"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            int rolSession = (int)Session["Rol"];
            if (rolSession != (int)Usuario.TipoUsuario.SuperAdmin)
                return RedirectToAction("IniciarSesion", "Login");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CrearCategoria(Categoria model)
        {
            if (Session["Rol"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            int rolSession = (int)Session["Rol"];
            if (rolSession != (int)Usuario.TipoUsuario.SuperAdmin)
                return RedirectToAction("IniciarSesion", "Login");

            ModelState.Clear();
            Usuario repetido = db.Usuarios.Where(u => u.Nombre == model.NombreCategoria).FirstOrDefault();
            if (repetido != null)
            {
                ModelState.AddModelError("NombreCategoria", "El nombre de esta Categoria ya se encuentra en uso");
            }
            TryValidateModel(model);

            if (ModelState.IsValid == false)
            {
                return View(model);
            }

            Categoria.CrearCategoria(db, model);

            return RedirectToAction("ListaCategorias");
        }

        public ActionResult EditarCategoria(int id)
        {
            if (Session["Rol"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            int rolSession = (int)Session["Rol"];
            if (rolSession != (int)Usuario.TipoUsuario.SuperAdmin)
                return RedirectToAction("IniciarSesion", "Login");

            Categoria categoria = db.Categorias.Find(id);

            return View(categoria);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarCategoria(Categoria model)
        {
            if (Session["Rol"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            int rolSession = (int)Session["Rol"];
            if (rolSession != (int)Usuario.TipoUsuario.SuperAdmin)
                return RedirectToAction("IniciarSesion", "Login");

            ModelState.Clear();
            Usuario repetido = db.Usuarios.Where(u => u.Nombre == model.NombreCategoria && u.Id != model.Id).FirstOrDefault();
            if (repetido != null)
            {

                if (repetido != null)
                    ModelState.AddModelError("NombreCategoria", "El nombre de esta Categoria ya se encuentra en uso");
            }
            TryValidateModel(model);

            if (ModelState.IsValid == false)
            {
                return View(model);
            }

            Categoria.EditarCategoria(db, model);

            return RedirectToAction("ListaCategorias");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarCategoria(int id)
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

            int rolSession = (int)Session["Rol"];
            if (rolSession != (int)Usuario.TipoUsuario.SuperAdmin)
            {
                return Json(new
                {
                    exito = false,
                    mensaje = "Rol no autorizado."
                }, JsonRequestBehavior.AllowGet);
            }

            Categoria.EliminarCategoria(db, id);

            return Json(new
            {
                exito = true,
                mensaje = "Eliminación exitosa."
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListaCategorias()
        {
            if (Session["Rol"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            int rolSession = (int)Session["Rol"];
            if (rolSession != (int)Usuario.TipoUsuario.SuperAdmin)
                return RedirectToAction("IniciarSesion", "Login");

            List<Categoria> categorias = db.Categorias.ToList();

            return View(categorias);
        }

        #endregion
    }
}