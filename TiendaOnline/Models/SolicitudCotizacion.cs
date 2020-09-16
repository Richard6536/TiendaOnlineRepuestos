using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TiendaOnline.Models
{
    public class SolicitudCotizacion
    {
        [Key]
        public int Id { get; set; }
        public string Codigo { get; set; }
        public DateTime Fecha { get; set; }

        public int? TiendaId { get; set; }
        [ForeignKey("TiendaId")]
        public virtual Tienda Tienda { get; set; }

        public int? UsuarioId { get; set; }
        [ForeignKey("UsuarioId")]
        public virtual Usuario Usuario { get; set; }
        public virtual List<Cotizacion> Cotizacion { get; set; }
        public virtual List<Servicio> Servicios { get; set; }

        public static bool CrearSolicitud(TiendaOnlineContext _db, List<Servicio> _servicios, Usuario _usuario, int idTienda)
        {
            Tienda tienda = _db.Tienda.Where(u => u.Id == idTienda).FirstOrDefault();

            SolicitudCotizacion solicitudCotizacion = new SolicitudCotizacion();
            //solicitudCotizacion.Servicios = new List<Servicio>();
            solicitudCotizacion.Servicios = new List<Servicio>();

            foreach (Servicio servicio in _servicios)
            {
                solicitudCotizacion.Servicios.Add(servicio);
            }

            solicitudCotizacion.Fecha = DateTime.Now;
            solicitudCotizacion.Tienda = tienda;
            solicitudCotizacion.TiendaId = tienda.Id;
            solicitudCotizacion.Usuario = _usuario;
            solicitudCotizacion.UsuarioId = _usuario.Id;

            tienda.SolicitudCotizaciones.Add(solicitudCotizacion);
            _db.SaveChanges();

            return true;
        }

        public static bool EliminarSolicitud(TiendaOnlineContext _db, int _id)
        {
            SolicitudCotizacion solicitudCotizacion = _db.SolicitudCotizacion.Where(s => s.Id == _id).FirstOrDefault();
            if (solicitudCotizacion == null)
                return false;

            solicitudCotizacion.Usuario = null;
            solicitudCotizacion.Tienda = null;

            _db.SolicitudCotizacion.Remove(solicitudCotizacion);
            _db.SaveChanges();

            return true;
        }
    }
}