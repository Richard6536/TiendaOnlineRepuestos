using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TiendaOnline.Models
{
    [Table("ServicioSolicitadoCotizacion")]
    public class ServicioSolicitadoCotizacion
    {
        [Key]
        public int Id { get; set; }

        public int Cantidad { get; set; }

        public float Valor { get; set; }
        public float ValorTotal { get; set; }

        public string Comentario { get; set; }

        public int ServicioId { get; set; }
        [ForeignKey("ServicioId")]
        public virtual Servicio Servicio { get; set; }
    }
}