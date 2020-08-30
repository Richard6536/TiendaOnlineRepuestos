using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TiendaOnline.Models;

namespace TiendaOnline.Clases
{
    public class TiendaHome
    {
        public enum ViewProductType { List, Grid }

        public string OrderByProductName { get; set; }
        public string ViewTypeName { get; set; }
        public virtual Tienda Tienda { get; set; }
        public virtual IEnumerable<Producto> Productos { get; set; }
        public ViewProductType ViewProductEnumType { get; set; }
    }
}