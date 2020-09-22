using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.IO;

namespace TiendaOnline.Models
{
    [Table("RegistroEnvio")]
    public class RegistroEnvio
    {

        [Key]
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string MedioDeContacto { get; set; }
        public string Remitente { get; set; }
        public string Destinatario { get; set; }
        public string Notas { get; set; }
        public bool GeneradoAutomaticamente { get; set; }

        public int? CotizacionId { get; set; }
        [ForeignKey("CotizacionId")]
        public virtual Cotizacion Cotizacion { get; set; }

    }
}