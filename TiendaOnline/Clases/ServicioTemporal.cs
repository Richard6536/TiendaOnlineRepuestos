using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TiendaOnline.Models;

namespace TiendaOnline.Clases
{
    public class ServicioTemporal
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public virtual int CategoriaId { get; set; }
        public virtual Categoria Categoria { get; set; }

        public float Valor { get; set; }
        public int Cantidad { get; set; }
        public bool EsManual { get; set; }
    }
}