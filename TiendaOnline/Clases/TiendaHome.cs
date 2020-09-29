using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TiendaOnline.Models;

namespace TiendaOnline.Clases
{
    public class TiendaHome
    {

        public enum OrderByType { PrecioAscendente, PrecioDescendente, Relevante }
        public enum ViewProductType { List, Grid }

        public string OrderByProductName { get; set; }
        public string ViewTypeName { get; set; }
        public virtual Tienda Tienda { get; set; }
        public virtual IEnumerable<Producto> Productos { get; set; }
        public virtual IEnumerable<Servicio> Servicios { get; set; }
        public ViewProductType ViewProductEnumType { get; set; }

        public static Tuple<IEnumerable<Producto>, IEnumerable<Servicio>, string> SortProduct(IEnumerable<Producto> repuestos, IEnumerable<Servicio> servicios, OrderByType orderByType)
        {
            string orderByProductName = "";
            IEnumerable<Producto> repuestosOrdenados = null;
            IEnumerable<Servicio> serviciosOrdenados = null;

            switch (orderByType)
            {
                case OrderByType.PrecioAscendente:
                    repuestosOrdenados = repuestos.OrderBy(p => p.Precio).ToList();
                    serviciosOrdenados = servicios.OrderBy(p => p.Precio).ToList();
                    orderByProductName = "Precio Menor";

                    break;

                case OrderByType.PrecioDescendente:
                    repuestosOrdenados = repuestos.OrderByDescending(p => p.Precio).ToList();
                    serviciosOrdenados = servicios.OrderByDescending(p => p.Precio).ToList();
                    orderByProductName = "Precio Mayor";

                    break;
            }

            return new Tuple<IEnumerable<Producto>, IEnumerable<Servicio>, string>(repuestosOrdenados, serviciosOrdenados, orderByProductName);
        }
    }
}