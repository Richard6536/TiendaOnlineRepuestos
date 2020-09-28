using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiendaOnline.Models;

namespace TiendaOnline.Controllers
{
    public class SolicitudCotizacionController : Controller
    {
        TiendaOnlineContext db = new TiendaOnlineContext();

        // GET: SolicitudCotizacion
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListaSolicitudesCotizaciones()
        {
            if (Session["Rol"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            if (Session["TiendaId"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            int idTienda = (int)Session["TiendaId"];
            Tienda tienda = db.Tienda.Find(idTienda);

            List<SolicitudCotizacion> solicitudCotizacionesTienda = tienda.SolicitudCotizaciones.ToList();
            solicitudCotizacionesTienda.Reverse();

            return View(solicitudCotizacionesTienda);
        }

        public ActionResult DetallesSolicitudCotizacion(int id)
        {
            if (Session["Rol"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            if (Session["TiendaId"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            SolicitudCotizacion solicitudCotizacion = db.SolicitudCotizacion.Where(s => s.Id == id).FirstOrDefault();
            
            return View(solicitudCotizacion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarSolicitudCotizacion(int id)
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

            SolicitudCotizacion.EliminarSolicitud(db, id);

            return Json(new
            {
                exito = true,
                mensaje = "Eliminación exitosa."
            }, JsonRequestBehavior.AllowGet);
        }
    }
}