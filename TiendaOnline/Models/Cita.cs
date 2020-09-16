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
    [Table("Cita")]
    public class Cita
    {
        [Key]
        public int Id { get; set; }
        public string Codigo { get; set; }
        public DateTime Fecha { get; set; }

        public int? UsuarioTiendaMecanicoId { get; set; }
        [ForeignKey("UsuarioTiendaMecanicoId")]
        public virtual UsuarioTiendaMecanico UsuarioTiendaMecanico { get; set; }

        public int? ServicioId { get; set; }
        [ForeignKey("ServicioId")]
        public virtual Servicio Servicio { get; set; }

        public int? UsuarioId { get; set; }
        [ForeignKey("UsuarioId")]
        public virtual Usuario Usuario { get; set; }
    }
}