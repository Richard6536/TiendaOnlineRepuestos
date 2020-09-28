using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TiendaOnline.Models
{
    [Table("ServicioCotizado")]
    public class ServicioCotizado
    {
        [Key]
        public int Id { get; set; }

        //Nombre y Catengoria para Servicios creados en Cotización
        //Ya que no son asociados a a un Servicio en la Base de Datos
        public string Nombre { get; set; }
        public virtual Categoria Categoria { get; set; }

        public int Cantidad { get; set; }

        public float Valor { get; set; }
        public float ValorTotal { get; set; }

        public string Comentario { get; set; }
        public bool EsManual { get; set; } //Es true si el Servicio se crea al crear una Cotizacion.
        public virtual Servicio Servicio { get; set; }
    }
}