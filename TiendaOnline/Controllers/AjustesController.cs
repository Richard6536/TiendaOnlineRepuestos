using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiendaOnline.Models;

namespace TiendaOnline.Controllers
{

    public class AjustesController : Controller
    {
        TiendaOnlineContext db = new TiendaOnlineContext();

        // GET: Ajustes
        public ActionResult Index()
        {
            if (Session["Rol"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            if (Session["TiendaId"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            int id = (int)Session["TiendaId"];
            Tienda tienda = db.Tienda.Find(id);

            return View(tienda);
        }

        public ActionResult InformacionContacto(int id)
        {
            if (Session["Id"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            Tienda tienda = db.Tienda.Find(id);
            return View(tienda);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult InformacionContacto(Tienda model)
        {
            if (Session["Rol"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            if (Session["TiendaId"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            Tienda tienda = Tienda.EditarTienda(db, model);

            return RedirectToAction("InformacionContacto", "Ajustes", new { id = tienda.Id });
        }

        public ActionResult Logos()
        {
            if (Session["Rol"] == null)
                return RedirectToAction("IniciarSesion", "Login");
      
            if (Session["TiendaId"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            int id = (int)Session["TiendaId"];
            Tienda tienda = db.Tienda.Find(id);

            return View(tienda);
        }

        [HttpPost]
        public ActionResult Logos(string fileImagePerfil)
        {
            if (Session["Rol"] == null)
            {
                return Json(new
                {
                    exito = false,
                    mensaje = "La sesión ha terminado."
                }, JsonRequestBehavior.AllowGet);
            }

            if (Session["TiendaId"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            int idTienda = (int)Session["TiendaId"];
            Tienda tienda = db.Tienda.Find(idTienda);

            try
            {
                Imagen.GuardarLogoEnTienda(db, tienda, "Logos", "imagen_perfil.png", fileImagePerfil, Imagen.ImagenTipo.ProfileTienda);
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

        public ActionResult LogoRemitente()
        {
            return View();
        }
    }
}