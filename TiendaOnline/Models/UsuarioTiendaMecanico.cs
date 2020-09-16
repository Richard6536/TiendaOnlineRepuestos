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
        public int? UsuarioTiendaId { get; set; }
        [ForeignKey("UsuarioTiendaId")]
        public virtual UsuarioTienda UsuarioTienda { get; set; }
        public virtual List<Agenda> Agendas { get; set; } //DEBE SER UNA
        //public int? AgendaId { get; set; }
        //[ForeignKey("AgendaId")]
        //public virtual Agenda Agenda { get; set; }
    }
}