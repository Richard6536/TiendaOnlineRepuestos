using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiendaOnline.Models;

namespace TiendaOnline.Controllers
{
    public class CarroCompraController : Controller
    {
        TiendaOnlineContext db = new TiendaOnlineContext();

        // GET: CarroCompra
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AgregarAlCarro(int idServicio)
        {
            if (Session["Rol"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            int idUser = (int)Session["Id"];

            //Carrocompra.CrearCarroCompra(idUser, db);
            ProductoCarro productoCarro = ProductoCarro.CrearProductoCarro(idServicio, idUser, 1, db, true);
            Carrocompra.AgregarProductoAlCarroCompra(idUser, productoCarro, db);

            return RedirectToAction("RealizarCompra", "CarroCompra");
            //return RedirectToAction("VerServicio", "Servicio", new { id = idServicio });
        }

        public ActionResult CarroCompra()
        {
            if (Session["Rol"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            int idUser = (int)Session["Id"];
            Carrocompra carroCompra = db.CarrosCompra.Where(c => c.Usuario.Id == idUser).FirstOrDefault();

            return View(carroCompra);
        }

        public ActionResult RealizarCompra()
        {

            if (Session["Rol"] == null)
                return RedirectToAction("IniciarSesion", "Login");

            int idUsuario = (int)Session["Id"];

            Usuario usuario = db.Usuarios.Where(u => u.Id == idUsuario).FirstOrDefault();
            Carrocompra carroCompra = usuario.CarroCompra.First();

            foreach (ProductoCarro productoCarroTienda in carroCompra.ProductosCarro)
            {
                List<Servicio> serviciosTienda = new List<Servicio>();
                int idTienda = productoCarroTienda.Servicio.Tienda.Id;

                foreach (ProductoCarro productoCarro in carroCompra.ProductosCarro)
                {
                    if (idTienda == productoCarro.Servicio.Tienda.Id)
                    {
                        serviciosTienda.Add(productoCarro.Servicio);
                    }

                }

                SolicitarCotizacion(serviciosTienda, idTienda);
            }

            return View(carroCompra);
        }

        public bool SolicitarCotizacion(List<Servicio> servicios, int idTienda)
        {
            if (Session["Rol"] == null)
                RedirectToAction("IniciarSesion", "Login");
            if (Session["Id"] == null)
                RedirectToAction("IniciarSesion", "Login");

            int usuarioId = (int)Session["Id"];
            Usuario usuario = db.Usuarios.Where(u => u.Id == usuarioId).FirstOrDefault();
            //Servicio servicio = db.Servicios.Where(u => u.Id == servicioId).FirstOrDefault();

            SolicitudCotizacion.CrearSolicitud(db, servicios, usuario, idTienda);

            return true;
        }
    }
}