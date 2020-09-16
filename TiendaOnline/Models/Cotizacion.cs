using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TiendaOnline.Models
{
    [Table("Cotizacion")]
    public class Cotizacion
    {
        [Key]
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Referencia { get; set; }
        public DateTime Fecha { get; set; }
        public bool Manual { get; set; }
        public float SubTotalNeto { get; set; }
        public float Descuento { get; set; }
        public float TotalNeto { get; set; }
        public float IVA { get; set; }
        public float TotalAPagar { get; set; } //IVA INCLUIDO
        public string Comentario { get; set; }

        public int? UsuarioTiendaId { get; set; }
        [ForeignKey("UsuarioTiendaId")]
        public virtual UsuarioTienda UsuarioQueAtendio { get; set; }

        public int? TiendaId { get; set; }
        [ForeignKey("TiendaId")]
        public virtual Tienda Tienda { get; set; }

        public int? UsuarioId { get; set; }
        [ForeignKey("UsuarioId")]
        public virtual Usuario Usuario { get; set; }

        public int? LogoId { get; set; }
        [ForeignKey("LogoId")]
        public virtual LogoRemitente LogoRemitente { get; set; }

        public int? SolicitudCotizacionId { get; set; }
        [ForeignKey("SolicitudCotizacionId")]
        public virtual SolicitudCotizacion SolicitudCotizacion { get; set; }

        public virtual List<Servicio> ServiciosCotizados { get; set; }

        public static Cotizacion CrearCotizacion(TiendaOnlineContext _db, Cotizacion _cotizacion, int _usuarioTiendaId, int _tiendaId)
        {
            Usuario usuario = _db.Usuarios.Where(u => u.Id == _cotizacion.UsuarioId).FirstOrDefault();
            Usuario usuarioTienda = _db.Usuarios.Where(u => u.Id == _usuarioTiendaId).FirstOrDefault();
            Tienda tienda = _db.Tienda.Where(u => u.Id == _tiendaId).FirstOrDefault();
            _cotizacion.ServiciosCotizados = new List<Servicio>();

            //------ Automatica
            SolicitudCotizacion solicitudCotizacion = _db.SolicitudCotizacion.Where(s => s.Id == _cotizacion.SolicitudCotizacionId).FirstOrDefault();
            _cotizacion.SolicitudCotizacion = solicitudCotizacion;
            foreach (Servicio servicio in solicitudCotizacion.Servicios)
            {
                _cotizacion.ServiciosCotizados.Add(servicio);
            }
            //-----------------

            _cotizacion.Usuario = usuario;
            _cotizacion.Tienda = tienda;
            _cotizacion.Fecha = DateTime.Now;
            _cotizacion.UsuarioQueAtendio = usuarioTienda.UsuarioTiendas.First();

            if (usuario.Cotizaciones == null)
                usuario.Cotizaciones = new List<Cotizacion>();

            _db.Cotizacions.Add(_cotizacion);

            _db.SaveChanges();

            return _cotizacion;
        }

    }
}