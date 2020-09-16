using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TiendaOnline.Models
{
    [Table("ClienteTienda")]
    public class ClienteTienda
    {
        [Key]
        public int Id { get; set; }

        public int? TiendaId { get; set; }
        [ForeignKey("TiendaId")]
        public virtual Tienda Tienda { get; set; }

        public int? UsuarioId { get; set; }
        [ForeignKey("UsuarioId")]
        public virtual Usuario Usuario { get; set; }
    }
}