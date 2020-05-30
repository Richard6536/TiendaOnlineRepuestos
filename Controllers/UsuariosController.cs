using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiendaOnline.Models;

namespace TiendaOnline.Controllers
{
    public class UsuariosController : Controller
    {
        TiendaOnlineContext db = new TiendaOnlineContext();
        //si no tiene tienda no puede usar nada de esto
        //controlador para los user comunes y que creen usuarios dentro de su propia tienda con diferentes roles

        //crear/editar/eliminar


        #region -----USUARIOS-----

        public ActionResult CrearUsuario()
        {
            if (Session["Rol"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            if (Session["TiendaId"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CrearUsuario(Usuario model)
        {
            if (Session["Rol"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            if (Session["TiendaId"] == null)
                return RedirectToAction("IniciarSesion", "Login");


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
                return View(model);
            }

            int tiendaId = (int)Session["TiendaId"];
            model.RolUsuario = Usuario.TipoUsuario.Cliente;
            Usuario.CrearNuevoUsuario(db, model, tiendaId, model.RolEnTienda);

            return RedirectToAction("ListaUsuarios");
        }

        public ActionResult ListaUsuarios()
        {
            if (Session["Rol"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            if (Session["TiendaId"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            int tiendaId = (int)Session["TiendaId"];
            Tienda tienda = db.Tienda.Find(tiendaId);
            List<UsuarioTienda> usuariosTienda = tienda.UsuariosTienda.ToList();

            return View(usuariosTienda);
        }

        public ActionResult EditarUsuario(int id)
        {
            if (Session["Rol"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            if (Session["TiendaId"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            int tiendaId = (int)Session["TiendaId"];
            Tienda tienda = db.Tienda.Find(tiendaId);
            List<UsuarioTienda> usuariosTienda = tienda.UsuariosTienda.ToList();
            UsuarioTienda ustienda = usuariosTienda.Where(ut => ut.UsuarioId == id).FirstOrDefault();

            if (ustienda == null)
                return HttpNotFound();
            if(ustienda.Usuario == null)
                return HttpNotFound();

            ustienda.Usuario.RolEnTienda = ustienda.RolTienda;
            return View(ustienda.Usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarUsuario(Usuario model)
        {
            if (Session["Rol"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            if (Session["TiendaId"] == null)
                return RedirectToAction("IniciarSesion", "Login");


            if (model.NombreUsuario != null)
                model.NombreUsuario = model.NombreUsuario.ToUpper();
            if (model.Email != null)
                model.Email = model.Email.ToUpper();

            int tiendaId = (int)Session["TiendaId"];
            Tienda tienda = db.Tienda.Find(tiendaId);
            List<UsuarioTienda> usuariosTienda = tienda.UsuariosTienda.ToList();
            UsuarioTienda ustienda = usuariosTienda.Where(ut => ut.UsuarioId == model.Id).FirstOrDefault();
            if(ustienda == null)
                return RedirectToAction("IniciarSesion", "Login");

            Usuario repetido = db.Usuarios.Where(u => u.NombreUsuario == model.NombreUsuario && u.Id != model.Id).FirstOrDefault();
            Usuario mailRepetido = db.Usuarios.Where(u => u.Email == model.Email && u.Id != model.Id).FirstOrDefault();

            ModelState.Clear();
            model.Password = "asdf";//para evitar que el ModelState falle por "ser obligatorio"
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
                return View(model);
            }

            if (model.RolUsuario == Usuario.TipoUsuario.SuperAdmin)
                model.RolUsuario = Usuario.TipoUsuario.Cliente;

            Usuario.EditarUsuario(db, model, tiendaId, model.RolEnTienda);

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

            if (Session["Rol"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            if (Session["TiendaId"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            Usuario.EliminarUsuario(db, id);

            return Json(new
            {
                exito = true,
                mensaje = "Eliminación exitosa."
            }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}