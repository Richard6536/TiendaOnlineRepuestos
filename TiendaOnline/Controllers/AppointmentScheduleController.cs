using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiendaOnline.Models;

namespace TiendaOnline.Controllers
{
    public class AppointmentScheduleController : Controller
    {
        TiendaOnlineContext db = new TiendaOnlineContext();

        // GET: AppointmentSchedule
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ProgramarCitas(int idCotizacion)
        {
            if (Session["Rol"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            if (Session["TiendaId"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            int idTienda = (int)Session["TiendaId"];
            Tienda tienda = db.Tienda.Find(idTienda);

            SolicitudCotizacion cotizacion = db.SolicitudCotizacion.Where(c => c.Id == idCotizacion).FirstOrDefault();
            //Cotizacion cotizacion = db.Cotizacions.Where(c => c.Id == idCotizacion).FirstOrDefault();

            return View(cotizacion);
        }

    }
}