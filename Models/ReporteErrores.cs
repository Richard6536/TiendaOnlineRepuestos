using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;
using System.ComponentModel;

namespace TiendaOnline.Models
{
    [Table("ReporteErrores")]
    public class ReporteErrores
    {
        [Key]
        public int Id { get; set; }

        public DateTime Fecha { get; set; }

        public string Mensaje { get; set; }
    }
}