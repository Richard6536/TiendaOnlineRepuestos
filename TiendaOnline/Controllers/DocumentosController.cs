using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiendaOnline.Models;

namespace TiendaOnline.Controllers
{
    public class DocumentosController : Controller
    {
        TiendaOnlineContext db = new TiendaOnlineContext();
        // GET: Documentos
        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public PartialViewResult Cotizacion(int? id, int? tiendaId, bool? pdf)
        {
            if (id == null)
            {
                TempData["msgPartialView"] = "Not found";
                return PartialView("Mensaje");
            }

            int? idTienda = tiendaId;
            if (tiendaId == null)
            {
                if (Session["TiendaId"] == null)
                {
                    TempData["msgPartialView"] = "Not found";
                    return PartialView("Mensaje");
                }
                idTienda = (int)Session["TiendaId"];
            }

            Tienda tienda = db.Tienda.Where(t => t.Id == idTienda).FirstOrDefault();
            Cotizacion cotizacion = tienda.Cotizaciones.Where(c => c.Id == id).FirstOrDefault();
            if (cotizacion == null)
            {
                TempData["msgPartialView"] = "Not found";
                return PartialView("Mensaje");
            }


            bool espdf = false;
            if (pdf != null)
                espdf = pdf.Value;
            ViewBag.EsPDF = espdf;


            return PartialView("Cotizacion", cotizacion);
        }
    }
}