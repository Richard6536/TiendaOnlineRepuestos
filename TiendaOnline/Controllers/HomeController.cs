using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiendaOnline.Models;

namespace TiendaOnline.Controllers
{
    public class HomeController : Controller
    {
        TiendaOnlineContext db = new TiendaOnlineContext();

        //asi como giko que muestra todos los productos y permite filtrar por categoria
        //aqui se muestran todas las casas independiente de quien las subio


        public ActionResult Index()
        {
            List<Producto> productos = db.Productos.ToList();
            return View(productos);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult CrearTienda()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CrearTienda(Tienda model)
        {
            if (Session["Id"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            ModelState.Clear();
            if (model.Nombre == null || model.Nombre == "")
                ModelState.AddModelError("Nombre", "El campo nombre es obligatorio");
            else 
            {
                Tienda repetido = db.Tienda.Where(u => u.Nombre.ToUpper() == model.Nombre.ToUpper()).FirstOrDefault();

                if (repetido != null)
                    ModelState.AddModelError("Nombre", "El nombre de Tienda ya se encuentra en uso");        
            }
            TryValidateModel(model);


            if (ModelState.IsValid == false)
            {
                return View(model);
            }

            Tienda tienda = Models.Tienda.CrearNuevaTienda(db, model);

            int userId = (int)Session["Id"];
            Usuario usuario = db.Usuarios.Find(userId);
            Usuario.EditarUsuario(db, usuario, tienda.Id, UsuarioTienda.RolEnTienda.Admin);

            UsuarioTienda usTienda = db.UsuariosTienda.Where(ut => ut.Usuario.Id == userId).FirstOrDefault();

            if (usTienda != null)
            {
                Session["TiendaId"] = usTienda.Tienda.Id;
                Session["TiendaNombre"] = usTienda.Tienda.Nombre;
            }

            return RedirectToAction("Index", "TiendaUser", new { id = tienda.Id});
        }

        public ActionResult Tienda(int? id)
        {
            Tienda tienda = db.Tienda.Find(id);

            if (tienda == null)
                return RedirectToAction("Index", "Home");

            return View(tienda);
        }

    }
}