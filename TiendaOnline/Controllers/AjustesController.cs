using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiendaOnline.Clases;
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

        public ActionResult HorarioTienda(int? idTienda)
        {
            if (idTienda == null)
                return RedirectToAction("IniciarSesion", "Login");

            Tienda tienda = db.Tienda.Where(ut => ut.Id == idTienda).FirstOrDefault();

            return View(tienda);
        }

        public class HorarioTemporalTiendaList
        {
            public int TiendaId { get; set; }
            public List<HorarioTemporalTrabajador> LunesList { get; set; }
            public List<HorarioTemporalTrabajador> MartesList { get; set; }
            public List<HorarioTemporalTrabajador> MiercolesList { get; set; }
            public List<HorarioTemporalTrabajador> JuevesList { get; set; }
            public List<HorarioTemporalTrabajador> ViernesList { get; set; }
            public List<HorarioTemporalTrabajador> SabadoList { get; set; }
            public List<HorarioTemporalTrabajador> DomingoList { get; set; }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CrearHorarioTienda(HorarioTemporalTiendaList model)
        {
            if (Session["Rol"] == null)
            {
                return Json(new
                {
                    exito = false,
                    mensaje = "La sesión ha terminado."
                }, JsonRequestBehavior.AllowGet);
            }

            Tienda tienda = db.Tienda.Where(t => t.Id == model.TiendaId).FirstOrDefault();
            tienda.Horario = new List<HorarioTienda>();

            Models.HorarioTienda.CrearHorarioPorDia(db, model.TiendaId, Models.HorarioTienda.DiaHorarioTienda.Lunes,
                model.LunesList[0].Hora, model.LunesList[1].Hora, model.LunesList[2].Hora, model.LunesList[3].Hora, true);

            Models.HorarioTienda.CrearHorarioPorDia(db, model.TiendaId, Models.HorarioTienda.DiaHorarioTienda.Martes,
                model.MartesList[0].Hora, model.MartesList[1].Hora, model.MartesList[2].Hora, model.MartesList[3].Hora, true);

            Models.HorarioTienda.CrearHorarioPorDia(db, model.TiendaId, Models.HorarioTienda.DiaHorarioTienda.Miercoles,
                model.MiercolesList[0].Hora, model.MiercolesList[1].Hora, model.MiercolesList[2].Hora, model.MiercolesList[3].Hora, true);

            Models.HorarioTienda.CrearHorarioPorDia(db, model.TiendaId, Models.HorarioTienda.DiaHorarioTienda.Jueves,
                model.JuevesList[0].Hora, model.JuevesList[1].Hora, model.JuevesList[2].Hora, model.JuevesList[3].Hora, true);

            Models.HorarioTienda.CrearHorarioPorDia(db, model.TiendaId, Models.HorarioTienda.DiaHorarioTienda.Viernes,
                model.ViernesList[0].Hora, model.ViernesList[1].Hora, model.ViernesList[2].Hora, model.ViernesList[3].Hora, true);

            Models.HorarioTienda.CrearHorarioPorDia(db, model.TiendaId, Models.HorarioTienda.DiaHorarioTienda.Sabado,
                model.SabadoList[0].Hora, model.SabadoList[1].Hora, model.SabadoList[2].Hora, model.SabadoList[3].Hora, true);

            Models.HorarioTienda.CrearHorarioPorDia(db, model.TiendaId, Models.HorarioTienda.DiaHorarioTienda.Domingo,
                model.DomingoList[0].Hora, model.DomingoList[1].Hora, model.DomingoList[2].Hora, model.DomingoList[3].Hora, true);

            return Json(new { exito = true, id = model.TiendaId });
        }
    }
}