using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiendaOnline.Models;

namespace TiendaOnline.Controllers
{
    public class ScheduleController : Controller
    {
        TiendaOnlineContext db = new TiendaOnlineContext();

        // GET: Schedule
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CalendarioTienda()
        {
            if (Session["Rol"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            if (Session["TiendaId"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            int idTienda = (int)Session["TiendaId"];
            Calendario calendario = Calendario.ObtenerAgendasTienda(db, idTienda);

            return View(calendario);
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

        public ActionResult SeleccionarHora(int? id)
        {
            if (Session["Id"] == null) //SI NO ESTÁ LOGEADO, DEBERÁ INICIAR SESIÓN Y REDIRECCIONAR HACÍA ACÁ
                return RedirectToAction("IniciarSesion", "Login");

            if (Session["Rol"] == null) //SI NO ESTÁ LOGEADO, DEBERÁ INICIAR SESIÓN Y REDIRECCIONAR HACÍA ACÁ
                return RedirectToAction("IniciarSesion", "Login");

            Cotizacion cotizacion = db.Cotizacions.Where(c => c.Id == id).FirstOrDefault();

            return View(cotizacion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegistrarCita(Cita model)
        {
            if (Session["Rol"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            DateTime newDateTermino = model.FechaTermino.AddHours(1);
            model.FechaTermino = newDateTermino;

            Cita cita = Cita.CrearCita(db, model);

            return View();
        }
    }
}