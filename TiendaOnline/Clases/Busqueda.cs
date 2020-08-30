using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using TiendaOnline.Models;

namespace TiendaOnline.Clases
{
    public class Busqueda
    {
        public enum BusquedaTipo { Producto, Servicio, Tienda }

        public string BusquedaText { get; set; }
        public virtual IEnumerable<Producto> Productos { get; set; }
        public virtual IEnumerable<Servicio> Servicios { get; set; }
        public virtual IEnumerable<Tienda> Tiendas { get; set; }

        public static Tuple<List<Producto>, List<Servicio>, List<Tienda>, Boolean, BusquedaTipo, string> BuscarProductosPorCategoriaConModeloMarcaYear(TiendaOnlineContext _db, int id, string busqueda, string modelo, string marca, int? year, string seccion, bool desdeTienda, Tienda tienda)
        {
            //Inicializacion de variables
            bool agregado = false;
            bool agregadoExacto = false;
            bool esTienda = false; //Será true solo en el caso de que no se encuentren productos o servicios.

            Producto productoExacto = null;
            Servicio servicioExacto = null;
            Tienda tiendaExacto = null;

            List<Producto> productosFiltrados = new List<Producto>();
            List<Servicio> serviciosFiltrados = new List<Servicio>();
            List<Tienda> TiendasFiltrados = new List<Tienda>();

            List<Producto> productos = new List<Producto>();
            List<Servicio> servicios = new List<Servicio>();
            List<Tienda> tiendas = new List<Tienda>();
            //------------------------------


            if (desdeTienda)
            {
                //Realiza busqueda de productos/servicios solamente desde una tienda.
                productos = tienda.Productos.ToList();
                servicios = tienda.Servicios.ToList();
            }
            else
            {
                //Realiza busqueda de todos los productos/servicios.
                productos = _db.Productos.ToList();
                servicios = _db.Servicios.ToList();
                tiendas = _db.Tienda.ToList();
            }

            //Si recibo una categoría, solo filtro productos/servicios pertenecientes a esa categoría.
            Categoria categoria = null;
            if (id != -1)
            {
                categoria = _db.Categorias.Find(id);
                if (categoria.TipoCategoria == Categoria.CategoriaTipo.Producto)
                {
                    productos = categoria.Productos;
                }
                else if (categoria.TipoCategoria == Categoria.CategoriaTipo.Servicio)
                {
                    servicios = categoria.Servicios;
                }

            }

            if (busqueda == null || busqueda.Equals(""))
            {
                return Tuple.Create(productos, servicios, tiendas, false, BusquedaTipo.Producto, busqueda);
            }

            //Busqueda de producto o servicio seleccionado categoria
            if (categoria != null && categoria.TipoCategoria == Categoria.CategoriaTipo.Producto)
            {
                Tuple<List<Producto>, List<Servicio>, List<Tienda>, Boolean, BusquedaTipo, string> productosTuple = BuscarProductosConModeloMarcaYear(productos, productoExacto, busqueda, marca, modelo, year, agregadoExacto, productosFiltrados, agregado);
                if (VerificacionTienda(productosTuple.Item1, productosTuple.Item2) == true)
                    esTienda = true;

                return productosTuple;
            }
            else if (categoria != null && categoria.TipoCategoria == Categoria.CategoriaTipo.Servicio)
            {
                Tuple<List<Producto>, List<Servicio>, List<Tienda>, Boolean, BusquedaTipo, string> serviciosTuple = BuscarServicios(servicios, servicioExacto, busqueda, agregadoExacto, serviciosFiltrados, agregado);
                if (VerificacionTienda(serviciosTuple.Item1, serviciosTuple.Item2) == true)
                    esTienda = true;

                return serviciosTuple;
            }
            else if (categoria == null)
            {
                Tuple<List<Producto>, List<Servicio>, List<Tienda>, Boolean, BusquedaTipo, string> productosTuple = BuscarProductosConModeloMarcaYear(productos, productoExacto, busqueda, marca, modelo, year, agregadoExacto, productosFiltrados, agregado);
                Tuple<List<Producto>, List<Servicio>, List<Tienda>, Boolean, BusquedaTipo, string> serviciosTuple = BuscarServicios(servicios, servicioExacto, busqueda, agregadoExacto, serviciosFiltrados, agregado);
                Tuple<List<Producto>, List<Servicio>, List<Tienda>, Boolean, BusquedaTipo, string> tiendasTuple = BuscarTiendas(tiendas, tiendaExacto, busqueda, agregadoExacto, TiendasFiltrados, agregado);

                return Tuple.Create(productosTuple.Item1, serviciosTuple.Item2, tiendasTuple.Item3, false, BusquedaTipo.Producto, busqueda);
            }

            return null;
        }


        public static Tuple<List<Producto>, List<Servicio>, List<Tienda>, Boolean, BusquedaTipo, string> BuscarProductosPorCategoria(TiendaOnlineContext _db, int id, string busqueda, string seccion, bool desdeTienda, Tienda tienda)
        {
            //Inicializacion de variables
            bool agregado = false;
            bool agregadoExacto = false;
            bool esTienda = false; //Será true solo en el caso de que no se encuentren productos o servicios.

            Producto productoExacto = null;
            Servicio servicioExacto = null;
            Tienda tiendaExacto = null;

            List<Producto> productosFiltrados = new List<Producto>();
            List<Servicio> serviciosFiltrados = new List<Servicio>();
            List<Tienda> TiendasFiltrados = new List<Tienda>();

            List<Producto> productos = new List<Producto>();
            List<Servicio> servicios = new List<Servicio>();
            List<Tienda> tiendas = new List<Tienda>();
            //------------------------------


            if (desdeTienda)
            {
                //Realiza busqueda de productos/servicios solamente desde una tienda.
                productos = tienda.Productos.ToList();
                servicios = tienda.Servicios.ToList();
            }
            else
            {
                //Realiza busqueda de todos los productos/servicios.
                productos = _db.Productos.ToList();
                servicios = _db.Servicios.ToList();
                tiendas = _db.Tienda.ToList();
            }

            //Si recibo una categoría, solo filtro productos/servicios pertenecientes a esa categoría.
            Categoria categoria = null;
            if (id != -1)
            {
                categoria = _db.Categorias.Find(id);
                if (categoria.TipoCategoria == Categoria.CategoriaTipo.Producto)
                {
                    productos = categoria.Productos;
                }
                else if (categoria.TipoCategoria == Categoria.CategoriaTipo.Servicio)
                {
                    servicios = categoria.Servicios;
                }

            }

            if (busqueda == null || busqueda.Equals(""))
            {
                return Tuple.Create(productos, servicios, tiendas, false, BusquedaTipo.Producto, busqueda);
            }

            //Busqueda de producto o servicio seleccionado categoria
            if (categoria != null && categoria.TipoCategoria == Categoria.CategoriaTipo.Producto)
            {
                Tuple<List<Producto>, List<Servicio>, List<Tienda>, Boolean, BusquedaTipo, string> productosTuple = BuscarProductos(productos, productoExacto, busqueda, agregadoExacto, productosFiltrados, agregado);
                if (VerificacionTienda(productosTuple.Item1, productosTuple.Item2) == true)
                    esTienda = true;

                return productosTuple;
            }
            else if (categoria != null && categoria.TipoCategoria == Categoria.CategoriaTipo.Servicio)
            {
                Tuple<List<Producto>, List<Servicio>, List<Tienda>, Boolean, BusquedaTipo, string> serviciosTuple = BuscarServicios(servicios, servicioExacto, busqueda, agregadoExacto, serviciosFiltrados, agregado);
                if (VerificacionTienda(serviciosTuple.Item1, serviciosTuple.Item2) == true)
                    esTienda = true;

                return serviciosTuple;
            }
            else if (categoria == null)
            {
                Tuple<List<Producto>, List<Servicio>, List<Tienda>, Boolean, BusquedaTipo, string> productosTuple = BuscarProductos(productos, productoExacto, busqueda, agregadoExacto, productosFiltrados, agregado);
                if (productosTuple.Item1.Count > 0)
                {
                    return productosTuple;
                }

                Tuple<List<Producto>, List<Servicio>, List<Tienda>, Boolean, BusquedaTipo, string> serviciosTuple = BuscarServicios(servicios, servicioExacto, busqueda, agregadoExacto, serviciosFiltrados, agregado);
                if (serviciosTuple.Item2.Count > 0)
                {
                    return serviciosTuple;
                }

                if (VerificacionTienda(productosTuple.Item1, serviciosTuple.Item2) == true)
                    esTienda = true;
            }

            if (esTienda)
            {
                Tuple<List<Producto>, List<Servicio>, List<Tienda>, Boolean, BusquedaTipo, string> TiendasTuple = BuscarTiendas(tiendas, tiendaExacto, busqueda, agregadoExacto, TiendasFiltrados, agregado);
                return TiendasTuple;
            }

            return null;
        }

        public static Tuple<List<Producto>, List<Servicio>, List<Tienda>, Boolean, BusquedaTipo, string> BuscarProductosConModeloMarcaYear(List<Producto> productos, Producto productoExacto, string busqueda, string modelo, string marca, int? year,
            bool agregadoExacto, List<Producto> productosFiltrados, bool agregado)
        {
            List<Servicio> nullListServicio = new List<Servicio>();
            List<Tienda> nullListTienda = new List<Tienda>();

            //Realiza busqueda de productos con el Modelo, Marca y Año
            if (marca != null && !marca.Equals("") || modelo != null && !modelo.Equals("") || year != null && year != 0)
            {
                Tuple<List<Producto>, BusquedaTipo, string, string, int?> productosTuple = FiltrarModeloMarcaYear(productos, BusquedaTipo.Producto, marca, modelo, year);
                productos = productosTuple.Item1;
            }


            foreach (var producto in productos)
            {

                string[] prodArrayDB = Regex.Split(producto.Nombre, @"(?:\s*,\s*)|\s+");
                string[] prodArrayBS = Regex.Split(busqueda, @"(?:\s*,\s*)|\s+");

                if (producto.Nombre.Equals(busqueda, StringComparison.InvariantCultureIgnoreCase))
                {
                    //productosFiltrados.Add(producto);
                    productoExacto = producto;
                    agregadoExacto = true;
                }

                if (!agregadoExacto)
                {

                    //string[] prodArray = producto.Nombre.Split(' ');
                    foreach (string valdb in prodArrayDB.Where(i => !string.IsNullOrEmpty(i)))
                    {
                        foreach (string valbs in prodArrayBS.Where(i => !string.IsNullOrEmpty(i)))
                        {
                            if (valdb.Equals(valbs, StringComparison.InvariantCultureIgnoreCase))
                            {
                                productosFiltrados.Add(producto);
                                agregado = true;
                            }
                        }

                        if (agregado)
                            break;
                    }

                    agregado = false;
                }
                agregadoExacto = false;
            }

            if (productoExacto != null)
            {
                productosFiltrados.Add(productoExacto);
                productosFiltrados.Reverse();
                agregadoExacto = true;
                productoExacto = null;
            }

            return Tuple.Create(productosFiltrados, nullListServicio, nullListTienda, agregadoExacto, BusquedaTipo.Producto, busqueda);
        }

        public static Tuple<List<Producto>, List<Servicio>, List<Tienda>, Boolean, BusquedaTipo, string> BuscarProductos(List<Producto> productos, Producto productoExacto, string busqueda,
            bool agregadoExacto, List<Producto> productosFiltrados, bool agregado)
        {
            List<Servicio> nullListServicio = new List<Servicio>();
            List<Tienda> nullListTienda = new List<Tienda>();

            foreach (var producto in productos)
            {

                string[] prodArrayDB = Regex.Split(producto.Nombre, @"(?:\s*,\s*)|\s+");
                string[] prodArrayBS = Regex.Split(busqueda, @"(?:\s*,\s*)|\s+");

                if (producto.Nombre.Equals(busqueda, StringComparison.InvariantCultureIgnoreCase))
                {
                    //productosFiltrados.Add(producto);
                    productoExacto = producto;
                    agregadoExacto = true;
                }

                if (!agregadoExacto)
                {

                    //string[] prodArray = producto.Nombre.Split(' ');
                    foreach (string valdb in prodArrayDB.Where(i => !string.IsNullOrEmpty(i)))
                    {
                        foreach (string valbs in prodArrayBS.Where(i => !string.IsNullOrEmpty(i)))
                        {
                            if (valdb.Equals(valbs, StringComparison.InvariantCultureIgnoreCase))
                            {
                                productosFiltrados.Add(producto);
                                agregado = true;
                            }
                        }

                        if (agregado)
                            break;
                    }

                    agregado = false;
                }
                agregadoExacto = false;
            }

            if (productoExacto != null)
            {
                productosFiltrados.Add(productoExacto);
                productosFiltrados.Reverse();
                agregadoExacto = true;
                productoExacto = null;
            }

            return Tuple.Create(productosFiltrados, nullListServicio, nullListTienda, agregadoExacto, BusquedaTipo.Producto, busqueda);
        }

        public static Tuple<List<Producto>, List<Servicio>, List<Tienda>, Boolean, BusquedaTipo, string> BuscarServicios(List<Servicio> servicios, Servicio servicioExacto, string busqueda,
            bool agregadoExacto, List<Servicio> serviciosFiltrados, bool agregado)
        {
            List<Producto> nullListProducto = new List<Producto>();
            List<Tienda> nullListTienda = new List<Tienda>();

            foreach (var servicio in servicios)
            {

                string[] prodArrayDB = Regex.Split(servicio.Nombre, @"(?:\s*,\s*)|\s+");
                string[] prodArrayBS = Regex.Split(busqueda, @"(?:\s*,\s*)|\s+");

                if (servicio.Nombre.Equals(busqueda, StringComparison.InvariantCultureIgnoreCase))
                {
                    //productosFiltrados.Add(producto);
                    servicioExacto = servicio;
                    agregadoExacto = true;
                }

                if (!agregadoExacto)
                {

                    //string[] prodArray = producto.Nombre.Split(' ');
                    foreach (string valdb in prodArrayDB.Where(i => !string.IsNullOrEmpty(i)))
                    {
                        foreach (string valbs in prodArrayBS.Where(i => !string.IsNullOrEmpty(i)))
                        {
                            if (valdb.Equals(valbs, StringComparison.InvariantCultureIgnoreCase))
                            {
                                serviciosFiltrados.Add(servicio);
                                agregado = true;
                            }
                        }

                        if (agregado)
                            break;
                    }

                    agregado = false;
                }
                agregadoExacto = false;
            }

            if (servicioExacto != null)
            {
                serviciosFiltrados.Add(servicioExacto);
                serviciosFiltrados.Reverse();
                agregadoExacto = true;
                servicioExacto = null;
            }

            return Tuple.Create(nullListProducto, serviciosFiltrados, nullListTienda, agregadoExacto, BusquedaTipo.Servicio, busqueda);
        }

        public static Tuple<List<Producto>, List<Servicio>, List<Tienda>, Boolean, BusquedaTipo, string> BuscarTiendas(List<Tienda> tiendas, Tienda tiendaExacto, string busqueda,
            bool agregadoExacto, List<Tienda> TiendasFiltrados, bool agregado)
        {
            List<Producto> nullListProducto = new List<Producto>();
            List<Servicio> nullListServicio = new List<Servicio>();

            foreach (Tienda tienda in tiendas)
            {

                string[] prodArrayDB = Regex.Split(tienda.Nombre, @"(?:\s*,\s*)|\s+");
                string[] prodArrayBS = Regex.Split(busqueda, @"(?:\s*,\s*)|\s+");

                if (tienda.Nombre.Equals(busqueda, StringComparison.InvariantCultureIgnoreCase))
                {
                    //productosFiltrados.Add(producto);
                    tiendaExacto = tienda;
                    agregadoExacto = true;
                }

                if (!agregadoExacto)
                {

                    //string[] prodArray = producto.Nombre.Split(' ');
                    foreach (string valdb in prodArrayDB.Where(i => !string.IsNullOrEmpty(i)))
                    {
                        foreach (string valbs in prodArrayBS.Where(i => !string.IsNullOrEmpty(i)))
                        {
                            if (valdb.Equals(valbs, StringComparison.InvariantCultureIgnoreCase))
                            {
                                TiendasFiltrados.Add(tienda);
                                agregado = true;
                            }
                        }

                        if (agregado)
                            break;
                    }

                    agregado = false;
                }
                agregadoExacto = false;
            }

            if (tiendaExacto != null)
            {
                TiendasFiltrados.Add(tiendaExacto);
                TiendasFiltrados.Reverse();
                agregadoExacto = true;
                tiendaExacto = null;
            }

            return Tuple.Create(nullListProducto, nullListServicio, TiendasFiltrados, agregadoExacto, BusquedaTipo.Tienda, busqueda);
        }

        public static List<Categoria> buscarCategoriasPorTienda(TiendaOnlineContext _db, IEnumerable<Producto> productosTienda, IEnumerable<Servicio> serviciosTienda, bool esProducto)
        {
            List<Categoria> categorias = new List<Categoria>();
            foreach (Categoria cat in _db.Categorias.ToList())
            {

                if (esProducto)
                {
                    foreach (var prod in productosTienda)
                    {
                        if (prod.CategoriaId == cat.Id)
                            if (categorias.Contains(cat) == false)
                                categorias.Add(cat);
                    }
                }
                else
                {
                    foreach (var serv in serviciosTienda)
                    {
                        if (serv.CategoriaId == cat.Id)
                            if (categorias.Contains(cat) == false)
                                categorias.Add(cat);
                    }
                }


            }

            return categorias;
        }

        public static Tuple<List<Producto>, BusquedaTipo, string, string, int?> FiltrarModeloMarcaYear(List<Producto> productos, BusquedaTipo tipoBusqueda,
            string modelo, string marca, int? year)
        {

            List<Producto> productosFiltrados = new List<Producto>();
            foreach (var producto in productos)
            {

                if (producto.Modelo != null && producto.Modelo.Equals(modelo, StringComparison.InvariantCultureIgnoreCase)
                    || producto.Marca != null && producto.Marca.Equals(marca, StringComparison.InvariantCultureIgnoreCase)
                    || producto.Year == year)
                {
                    productosFiltrados.Add(producto);
                }
            }

            return Tuple.Create(productosFiltrados, BusquedaTipo.Producto, modelo, marca, year);
        }

        public static bool VerificacionTienda(List<Producto> productos, List<Servicio> servicios)
        {
            if (productos.Count > 0)
            {
                return false;
            }
            else if (servicios.Count > 0)
            {
                return false;
            }

            return true;
        }
    }
}