using Rotativa;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiendaOnline.Models;

namespace TiendaOnline.Controllers
{
    public class PDFController : Controller
    {
        TiendaOnlineContext db = new TiendaOnlineContext();

        // GET: PDF
        public ActionResult Index()
        {
            return View();
        }
    
        public ActionResult ExportPDF(int? id, bool exportacionDirecta)
        {
            if (Session["TiendaId"] == null)
            {
                return RedirectToAction("IniciarSesion", "Login");
            }

            int tiendaId = (int)Session["TiendaId"];
            Tienda tienda = db.Tienda.Where(t => t.Id == tiendaId).FirstOrDefault();

            Cotizacion cotizacion = tienda.Cotizaciones.Where(c => c.Id == id).FirstOrDefault();

            string codigo = cotizacion.Codigo;
            string nombre = cotizacion.Usuario.NombreCompleto.ToUpper();


            string NombreCarpeta = "PDFTiendas";
            string NombreVistaPDF = "PreviewExportPDFCotizacion";

            string nombreDocumentoPDF = codigo + "_" + nombre + ".pdf";

            nombreDocumentoPDF = nombreDocumentoPDF.Replace('|', ' ');

            ActionAsPdf action = new ActionAsPdf(NombreVistaPDF + "/", new { id = id, tiendaId = tiendaId })
            {
                //FileName = cotizacion.Codigo + ".pdf"
                FileName = nombreDocumentoPDF,
                CustomSwitches =
                "--footer-center \"  Página: " + "[page]/[toPage]\"" +
                " --footer-line --footer-font-size \"10\" --footer-spacing 1 --footer-font-name \"Segoe UI\""

            };

            string fullPath = "";

            string basePath = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/img/" + NombreCarpeta + "/"), tiendaId + "");
            if (!System.IO.File.Exists(basePath))
                Directory.CreateDirectory(basePath); //crear si no existe


            int nArchivos = -1;
            int cFile = 0;
            string nFile = "";
            try
            {
                //Se eliminan los archivos en la carpeta si hay muchos
                string[] fileNames = Directory.GetFiles(basePath);
                nArchivos = fileNames.Count();
                if (fileNames.Count() >= 5)
                {
                    foreach (string file in fileNames)
                    {
                        string[] separado = file.Split('\\');
                        nFile = separado.Last();
                        string path = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/img/" + NombreCarpeta + "/" + tiendaId), nFile);
                        System.IO.File.Delete(path);
                        cFile++;
                    }
                }
            }
            catch (Exception e)
            {
                //ReporteErrores.CrearReporteError(db, "No se pudo borrar PDFs N°Arhivos=" + nArchivos + " numFail=" + cFile + " nombreFail=" + nFile + " MensajeException= " + e.Message + " ExeptionFULL= " + e.ToString());
            }

            if (exportacionDirecta == false)
            {
                fullPath = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/img/" + NombreCarpeta + "/" + tiendaId), nombreDocumentoPDF);

                try
                {
                    if (System.IO.File.Exists(fullPath))
                    {
                        System.IO.File.Delete(fullPath);
                    }

                    var byteArray = action.BuildFile(ControllerContext);
                    var fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
                    fileStream.Write(byteArray, 0, byteArray.Length);
                    fileStream.Close();

                }
                catch (Exception e)
                {
                    //ReporteErrores.CrearReporteError(db, "ERROR REEMPLAZANDO PDF MensajeException= " + e.Message + " ExeptionFULL= " + e.ToString());
                }

            }

            if (exportacionDirecta == true)
                return action; //retorna el ActionAsPdf para descargar
            else
            {
                //pdf del documento queda almacenado temporalmente
                //se genera el catalogo para luego enviar ambos por email
                return RedirectToAction("RedirectCentral", "Email",
                new
                {
                    id = id,
                    tiendaId = tiendaId,
                    nombreDocumento = nombreDocumentoPDF,
                    usuarioId = (int)Session["Id"]
                });
            }
        }

        public ActionResult PreviewExportPDFCotizacion(int? id, int? tiendaId)
        {
            if (!Request.IsLocal)
                return HttpNotFound();

            if (id == null)
                return HttpNotFound();

            Tienda tienda = db.Tienda.Where(e => e.Id == tiendaId).FirstOrDefault();
            Cotizacion cotizacion = tienda.Cotizaciones.Where(c => c.Id == id).FirstOrDefault();

            return View(cotizacion);
        }
    }
}