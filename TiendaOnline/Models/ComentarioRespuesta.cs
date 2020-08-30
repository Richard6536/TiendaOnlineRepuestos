using System;
using System.Collections.Generic;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TiendaOnline.Models
{
    [Table("ComentarioRespuesta")]
    public class ComentarioRespuesta
    {
        [Key]
        public int Id { get; set; }
        public string Mensaje { get; set; }
        public DateTime Fecha { get; set; }

        public int? ComentarioId { get; set; }
        [ForeignKey("ComentarioId")]
        public virtual Comentario Comentario { get; set; }

        public int? UsuarioId { get; set; }
        [ForeignKey("UsuarioId")]
        public virtual Usuario Usuario { get; set; }
    }
}