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
    [Table("Agenda")]
    public class Agenda
    {
        [Key]
        public int Id { get; set; }

        public int? UsuarioTiendaMecanicoId { get; set; }
        [ForeignKey("UsuarioTiendaMecanicoId")]
        public virtual UsuarioTiendaMecanico UsuarioTiendaMecanico { get; set; }
        public int? CalendarioId { get; set; }
        [ForeignKey("CalendarioId")]
        public virtual Calendario Calendario { get; set; }

        public virtual List<Cita> Citas { get; set; }
    }
}