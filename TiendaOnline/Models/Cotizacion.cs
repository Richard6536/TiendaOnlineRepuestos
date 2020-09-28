using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TiendaOnline.Clases;

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
        public virtual List<RegistroEnvio> RegistrosEnvios { get; set; }
        public virtual List<ServicioCotizado> ServiciosCotizados { get; set; }
        public virtual List<Vehiculo> Vehiculos { get; set; }

        public int? UsuarioTiendaMecanicoId { get; set; }
        [ForeignKey("UsuarioTiendaMecanicoId")]
        public virtual UsuarioTiendaMecanico UsuarioTiendaMecanico { get; set; }

        public virtual List<Cita> Citas { get; set; } //DEBE SER UNA!!!!!!!!!!

        public static Cotizacion CrearCotizacion(TiendaOnlineContext _db, Cotizacion _cotizacion, int _usuarioTiendaId, int _tiendaId, List<ServicioTemporal> servicios)
        {
            Usuario usuario = _db.Usuarios.Where(u => u.Id == _cotizacion.UsuarioId).FirstOrDefault();
            Usuario usuarioTienda = _db.Usuarios.Where(u => u.Id == _usuarioTiendaId).FirstOrDefault();
            Tienda tienda = _db.Tienda.Where(u => u.Id == _tiendaId).FirstOrDefault();
            _cotizacion.ServiciosCotizados = new List<ServicioCotizado>();

            float subTotalNeto = 0;
            float descuento = 0;
            float iva = 0;
            float valorTotal = 0;

            //------ ASOCIAR SERVICIOS
            SolicitudCotizacion solicitudCotizacion = _db.SolicitudCotizacion.Where(s => s.Id == _cotizacion.SolicitudCotizacionId).FirstOrDefault();
            _cotizacion.SolicitudCotizacion = solicitudCotizacion;

            foreach (ServicioTemporal servicioTemporal in servicios)
            {
                ServicioCotizado servicioCotizado = new ServicioCotizado();

                //Compueba si es un Servicio creado en la Cotización
                //Si es falso, se asocia a un Servicio en la Base de Datos
                if (!servicioTemporal.EsManual)
                {
                    Servicio servicioEncontrado = tienda.Servicios.Where(ssc => ssc.Id == servicioTemporal.Id).FirstOrDefault();
                    if (servicioEncontrado != null)
                    {
                        servicioCotizado.Servicio = servicioEncontrado;

                        if (servicioEncontrado.Categoria != null)
                            servicioCotizado.Categoria = servicioEncontrado.Categoria;
                    }

                }

                if (servicioCotizado.Categoria == null)
                {
                    Categoria categoria = _db.Categorias.Where(c => c.Id == servicioTemporal.CategoriaId).FirstOrDefault();
                    servicioCotizado.Categoria = categoria;
                }

                servicioCotizado.Nombre = servicioTemporal.Nombre;
                servicioCotizado.Valor = servicioTemporal.Valor;
                servicioCotizado.Cantidad = servicioTemporal.Cantidad;
                servicioCotizado.ValorTotal = servicioTemporal.Valor * servicioTemporal.Cantidad;
                servicioCotizado.EsManual = servicioTemporal.EsManual;

                //SUBTOTAL NETO
                subTotalNeto += servicioCotizado.ValorTotal;

                _cotizacion.ServiciosCotizados.Add(servicioCotizado);
            
            }

            valorTotal = subTotalNeto + (subTotalNeto * 19 / 100);
            iva = valorTotal - subTotalNeto;

            _cotizacion.SubTotalNeto = subTotalNeto;
            _cotizacion.Descuento = descuento;
            _cotizacion.TotalNeto = subTotalNeto - descuento;
            _cotizacion.IVA = iva;
            _cotizacion.TotalAPagar = valorTotal;

            //ASOCIAR VEHÍCULO
            _cotizacion.Vehiculos = new List<Vehiculo>();
            Vehiculo vehiculo = new Vehiculo();
            vehiculo.Marca = solicitudCotizacion.Vehiculos.First().Marca;
            vehiculo.Modelo = solicitudCotizacion.Vehiculos.First().Modelo;
            vehiculo.Year = solicitudCotizacion.Vehiculos.First().Year;
            _cotizacion.Vehiculos.Add(vehiculo);

            //ASOCIAR LOGO
            LogoRemitente logoRemitente = tienda.LogosRemitente.Where(l => l.Id == _cotizacion.LogoId).FirstOrDefault();
            _cotizacion.LogoRemitente = logoRemitente;

            _cotizacion.Usuario = usuario;
            _cotizacion.Tienda = tienda;
            _cotizacion.Fecha = DateTime.Now;
            _cotizacion.UsuarioQueAtendio = usuarioTienda.UsuarioTiendas.First();

            if (usuario.Cotizaciones == null)
                usuario.Cotizaciones = new List<Cotizacion>();

            _db.Cotizacions.Add(_cotizacion);

            try
            {
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                string e = ex.Message;
            }

            return _cotizacion;
        }

        public static Cotizacion AsociarMecanicoACotizacion(TiendaOnlineContext _db, int idTienda, int idCotizacion, int idUsuarioMecanico)
        {
            Tienda tienda = _db.Tienda.Where(ut => ut.Id == idTienda).FirstOrDefault();

            Cotizacion cotizacion = tienda.Cotizaciones.Where(c => c.Id == idCotizacion).FirstOrDefault();
            UsuarioTiendaMecanico usuarioTiendaMecanico = tienda.UsuarioTiendaMecanicos.Where(ust => ust.UsuarioTienda.Usuario.Id == idUsuarioMecanico).FirstOrDefault();

            cotizacion.UsuarioTiendaMecanico = usuarioTiendaMecanico;
            usuarioTiendaMecanico.Cotizaciones.Add(cotizacion);

            _db.SaveChanges();

            return cotizacion;
        }

        public static void EliminarCotizacion(TiendaOnlineContext _db, int _idCotizacion)
        {
            Cotizacion cotizacion = _db.Cotizacions.Where(c => c.Id == _idCotizacion).FirstOrDefault();
            cotizacion.Usuario = null;
            cotizacion.UsuarioQueAtendio = null;
            cotizacion.UsuarioTiendaMecanico = null;
            cotizacion.Tienda = null;
            cotizacion.SolicitudCotizacion = null;

            //DESVINCULAR Y ELIMINAR SERVICIOS COTIZADOS
            foreach (ServicioCotizado sc in cotizacion.ServiciosCotizados)
            {
                sc.Servicio = null;
            }
            _db.ServiciosCotizados.RemoveRange(cotizacion.ServiciosCotizados);
            cotizacion.ServiciosCotizados = new List<ServicioCotizado>();

            //DESVINCULAR Y ELIMINAR CITAS
            foreach (Cita c in cotizacion.Citas)
            {
                c.UsuarioTiendaMecanico = null;
            }
            
            _db.Cita.RemoveRange(cotizacion.Citas);
            cotizacion.Citas = new List<Cita>();

            //DESVINCULAR VEHÍCULOS (NO SE DEBEN ELIMINAR YA QUE ESTÁN ASOCIADOS A UNA SOLICITUD)
            foreach (Vehiculo vehiculo in cotizacion.Vehiculos)
            {
                vehiculo.Cotizacion = null;
            }
            cotizacion.Vehiculos = new List<Vehiculo>();

            _db.Cotizacions.Remove(cotizacion);
            _db.SaveChanges();
        }
    }
}