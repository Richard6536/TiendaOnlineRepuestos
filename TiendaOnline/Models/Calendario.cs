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
    [Table("Calendario")]
    public class Calendario
    {
        [Key]
        public int Id { get; set; }
        public virtual List<Agenda> Agendas { get; set; }
    }
}