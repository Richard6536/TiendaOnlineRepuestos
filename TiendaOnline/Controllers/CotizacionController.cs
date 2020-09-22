using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiendaOnline.Models;

namespace TiendaOnline.Controllers
{
    public class CotizacionController : Controller
    {
        TiendaOnlineContext db = new TiendaOnlineContext();

        // GET: Cotizacion
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult ListarCotizaciones()
        {
            if (Session["Rol"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            if (Session["TiendaId"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            int tiendaId = (int)Session["TiendaId"];
            Tienda tienda = db.Tienda.Where(c => c.Id == tiendaId).FirstOrDefault();
            List<Cotizacion> cotizaciones = tienda.Cotizaciones.ToList(); 

            return View(cotizaciones);
        }

        public ActionResult VerCotizacion(int id)
        {
            Cotizacion cotizacion = db.Cotizacions.Where(c => c.Id == id).FirstOrDefault();

            return View(cotizacion);
        }

        public ActionResult CrearCotizacion(bool manual, int solicitudId = 0)
        {
            if (Session["Rol"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            if (Session["TiendaId"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            Cotizacion cotizacion = new Cotizacion();
            cotizacion.Manual = manual;

            if (!manual)
                cotizacion = CompletarCotizacionConSolicitud(solicitudId, cotizacion);

            return View(cotizacion);
        }

        public Cotizacion CompletarCotizacionConSolicitud(int solicitudId, Cotizacion cotizacion)
        {
            SolicitudCotizacion solicitudCotizacion = db.SolicitudCotizacion.Where(s => s.Id == solicitudId).FirstOrDefault();
            cotizacion.ServiciosCotizados = new List<Servicio>();
            cotizacion.ServiciosCotizados = solicitudCotizacion.Servicios;
            cotizacion.Usuario = solicitudCotizacion.Usuario;
            cotizacion.Codigo = "COT_EJEMPLO_1";

            foreach (Servicio servicio in solicitudCotizacion.Servicios)
            {
                cotizacion.TotalNeto += servicio.Precio;
            }

            cotizacion.IVA = cotizacion.TotalNeto * 19 / 100;
            cotizacion.TotalAPagar = cotizacion.TotalNeto + cotizacion.IVA;

            cotizacion.SolicitudCotizacion = solicitudCotizacion;

            return cotizacion;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CrearCotizacionJS(Cotizacion model)
        {
            if (Session["Rol"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            if (Session["TiendaId"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            int idUsuarioTienda = (int)Session["Id"];
            int idTienda = (int)Session["TiendaId"];

            Cotizacion cotizacionCreada = Cotizacion.CrearCotizacion(db, model, idUsuarioTienda, idTienda);

            return Json(new { exito = true, id = cotizacionCreada.Id });
        }

        public ActionResult AsociarMecanico(int? id)
        {
            if (id == null)
                return RedirectToAction("IniciarSesion", "Login");

            if (Session["TiendaId"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            int idTienda = (int)Session["TiendaId"];
            Tienda tienda = db.Tienda.Where(ut => ut.Id == idTienda).FirstOrDefault();

            Cotizacion cotizacion = tienda.Cotizaciones.Where(c => c.Id == id).FirstOrDefault();
            List<UsuarioTienda> usuariosTienda = tienda.UsuariosTienda.Where(ust => ust.UsuariosTiendaMecanicos.Count > 0).ToList();

            List<Usuario> usuariosMecanicos = new List<Usuario>();
            List<UsuarioTiendaMecanico> usuarioTiendaMecanicos = new List<UsuarioTiendaMecanico>();

            foreach (UsuarioTienda ust in usuariosTienda)
            {
                usuariosMecanicos.Add(ust.Usuario);
                usuarioTiendaMecanicos.Add(ust.UsuariosTiendaMecanicos.First());
            }

            ViewBag.Cotizacion = cotizacion;
            ViewBag.MecanicoId = new SelectList(usuariosMecanicos, "Id", "NombreCompleto");

            return View(usuarioTiendaMecanicos.AsEnumerable());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AsociarMecanicoCotizacion(int idCotizacion, int idUsuarioMecanico)
        {
            
            if (Session["TiendaId"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            int idTienda = (int)Session["TiendaId"];
            Cotizacion cotizacion = Cotizacion.AsociarMecanicoACotizacion(db, idTienda, idCotizacion, idUsuarioMecanico);

            return Json(new { exito = true, id = cotizacion.Id });
        }

        public ActionResult EditarCotizacion()
        {
            return View();
        }
    }
}