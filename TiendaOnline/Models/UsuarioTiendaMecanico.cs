using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;
using System.ComponentModel;
using TiendaOnline.Clases;

namespace TiendaOnline.Models
{
    [Table("UsuarioTiendaMecanico")]
    public class UsuarioTiendaMecanico
    {
        public enum DisponibilidadMecanico { Disponible, Ausente }

        [Key]
        public int Id { get; set; }
        public DisponibilidadMecanico Disponibilidad { get; set; }

        public virtual UsuarioTienda UsuarioTienda { get; set; }
        public virtual List<HorarioTrabajador> Horario { get; set; }
        public virtual List<Cotizacion> Cotizaciones { get; set; }
        public virtual List<Agenda> Agendas { get; set; } //DEBE SER UNA
        public int? TiendaId { get; set; }
        [ForeignKey("TiendaId")]
        public virtual Tienda Tienda { get; set; }

        public static UsuarioTienda CrearMecanico(TiendaOnlineContext _db, UsuarioTienda _usuarioTienda)
        {

            if (_usuarioTienda.UsuariosTiendaMecanicos == null || _usuarioTienda.UsuariosTiendaMecanicos.Count == 0)
            {
                UsuarioTiendaMecanico usuarioTiendaMecanico = new UsuarioTiendaMecanico();
                usuarioTiendaMecanico.Disponibilidad = DisponibilidadMecanico.Disponible;
                //usuarioTiendaMecanico.UsuarioTienda = usuarioTienda;
                usuarioTiendaMecanico.Agendas = new List<Agenda>();
                usuarioTiendaMecanico.Horario = new List<HorarioTrabajador>();

                _usuarioTienda.UsuariosTiendaMecanicos = new List<UsuarioTiendaMecanico>();
                _usuarioTienda.UsuariosTiendaMecanicos.Add(usuarioTiendaMecanico); //DEBE SER UNA RELACIÓN

                //_db.UsuarioTiendaMecanicos.Add(usuarioTiendaMecanico);
                _db.SaveChanges();
            }


            return _usuarioTienda;
        }
    }
}