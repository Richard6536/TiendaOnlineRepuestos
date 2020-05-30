using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiendaOnline.Clases;
using TiendaOnline.Models;

namespace TiendaOnline.Controllers
{
    public class LoginController : Controller
    {
        TiendaOnlineContext db = new TiendaOnlineContext();

        //quizas no aqui pero
        //el usuario luego de registrarse por su cuenta
        //tiene un boton a mano como "Cree su tienda"
        //le pide nombre y cualquier dato extra necesario y la tienda creada se asocia a ese user


        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult IniciarSesion()
        {
            if (Session["Rol"] != null)
            {
                Usuario.TipoUsuario rol = (Usuario.TipoUsuario)Session["Rol"];

                if (rol == Usuario.TipoUsuario.SuperAdmin)
                    return RedirectToAction("Index", "SuperAdmin");
                else
                    return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public ActionResult IniciarSesion(string nombreUsuario, string password)
        {
            if (nombreUsuario.Equals("") || password.Equals(""))
            {
                ViewBag.Mensaje = "Debe rellenar los campos";
                return View();
            }

            int numeroAdmins = db.Usuarios.Where(u => u.RolUsuario == Usuario.TipoUsuario.SuperAdmin).ToList().Count;
            if (numeroAdmins == 0)
                return RedirectToAction("PrimerUsuario");

            bool usuarioExiste = ValidarUsuario(nombreUsuario, password);

            if (usuarioExiste == false)
            {
                ViewBag.Mensaje = "El Nombre de Usuario & Contraseña no son válidos";
                return View();
            }

            Usuario user = db.Usuarios.Where(u => u.NombreUsuario.ToUpper() == nombreUsuario.ToUpper()).FirstOrDefault();


            if (user.RolUsuario == Usuario.TipoUsuario.Bloqueado)
            {
                ViewBag.Mensaje = "Denegado el acceso. La cuenta se encuentra bloqueada.";
                return View();
            }

            Session["Id"] = user.Id;
            Session["Nombre"] = user.NombreUsuario;
            Session["Rol"] = user.RolUsuario;

            UsuarioTienda usTienda = db.UsuariosTienda.Where(ut => ut.Usuario.Id == user.Id).FirstOrDefault();
            if (usTienda != null)
            {
                Session["TiendaId"] = usTienda.Tienda.Id;
                Session["TiendaNombre"] = usTienda.Tienda.Nombre;
            }


            Carrocompra carroCompra = Carrocompra.CrearCarroCompra(user, db);
            Carrocompra.EliminarTodosLosProductosEnCarro(user, db);

            if (user.RolUsuario == Usuario.TipoUsuario.SuperAdmin)
                return RedirectToAction("Index", "SuperAdmin");
            else
                return RedirectToAction("Index", "Home");

        }

        public bool ValidarUsuario(string _nombreUsuario, string _password)
        {
            bool valido = false;

            Usuario usuario = db.Usuarios.Where(u => u.NombreUsuario.ToUpper() == _nombreUsuario.ToUpper()).FirstOrDefault();
            if (usuario != null)
            {
                string pass = usuario.Password;
                valido = PasswordHash.ValidatePassword(_password, pass);
            }

            return valido;
        }

        public ActionResult Registrate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registrate(Usuario model)
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
                return View(model);
            }
            model.RolUsuario = Usuario.TipoUsuario.Cliente;
            Usuario.CrearNuevoUsuario(db, model);

            return RedirectToAction("IniciarSesion");
        }

        public ActionResult PrimerUsuario()
        {
            List<Usuario> admins = db.Usuarios.Where(u => u.RolUsuario == Usuario.TipoUsuario.SuperAdmin).ToList();

            if (admins.Count > 0)
                return RedirectToAction("IniciarSesion");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PrimerUsuario(Usuario model)
        {
            if (ModelState.IsValid == false)
            {
                return View(model);
            }


            model.RolUsuario = Usuario.TipoUsuario.SuperAdmin;

            Usuario user = Usuario.CrearNuevoUsuario(db, model, 0, UsuarioTienda.RolEnTienda.Bloqueado);
            if (user == null)
            {
                ModelState.Clear();
                ModelState.AddModelError("NombreUsuario", "Este nombre de usuario ya existe.");
                return View();
            }

            if (ModelState.IsValid == false)
            {
                return View(model);
            }

            return RedirectToAction("IniciarSesion");
        }

        public ActionResult CerrarSesion()
        {
            Session["Id"] = null;
            Session["Nombre"] = null;
            Session["Rol"] = null;

            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }
    }
}