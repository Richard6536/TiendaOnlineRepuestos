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

namespace TiendaOnline.Models
{
    [Table("Categoria")]
    public class Categoria
    {
        public enum CategoriaTipo { Producto, Servicio }

        [Key]
        public int Id { get; set; }

        public string NombreCategoria { get; set; }

        public CategoriaTipo TipoCategoria { get; set; }

        public int? TiendaId { get; set; }
        [ForeignKey("TiendaId")]
        public virtual Tienda Tienda { get; set; }

        public virtual List<Producto> Productos { get; set; }
        public virtual List<Servicio> Servicios { get; set; }

        public static Boolean CrearCategoria(TiendaOnlineContext _db, Categoria _categoria)
        {
            //Categoria existe = _db.Categorias.Where(c => c.NombreCategoria == _categoria.NombreCategoria).FirstOrDefault();
            //if (existe != null)
            //{
            //    return false;
            //}

            _db.Categorias.Add(_categoria);
            _db.SaveChanges();

            return true;
        }

        public static Boolean EditarCategoria(TiendaOnlineContext _db, Categoria _categoria)
        {
            //Categoria categoriaMismoNombre = _db.Categorias.Where(c => c.NombreCategoria == _categoria.NombreCategoria).FirstOrDefault();

            //if (categoriaMismoNombre != null)
            //{
            //    return false;
            //}

            Categoria categoriaOriginal = _db.Categorias.Find(_categoria.Id);
            categoriaOriginal.NombreCategoria = _categoria.NombreCategoria;
            categoriaOriginal.TipoCategoria = _categoria.TipoCategoria;
            
            _db.SaveChanges();

            return true;

        }

        public static Categoria EliminarCategoria(TiendaOnlineContext _db, int id)
        {
            Categoria categoria = _db.Categorias.Find(id);

            foreach (Producto p in categoria.Productos)
            {
                p.Categoria = null;
                p.CategoriaId = null;
            }

            _db.Categorias.Remove(categoria);

            _db.SaveChanges();

            return categoria;
        }

        public static Tuple<List<Producto>, List<Servicio>, Boolean> BuscarProductosPorCategoria(TiendaOnlineContext _db, int id, string busqueda, string seccion, bool desdeTienda, Tienda tienda)
        {
            bool agregado = false;
            bool agregadoExacto = false;

            Producto productoExacto = null;
            Servicio servicioExacto = null;

            List<Producto> productosFiltrados = new List<Producto>();
            List<Servicio> serviciosFiltrados = new List<Servicio>();

            List<Producto> productos = new List<Producto>();
            List<Servicio> servicios = new List<Servicio>();

            if (desdeTienda) {
                //Realiza busqueda de productos/servicios solamente desde una tienda.
                productos = tienda.Productos.ToList();
                servicios = tienda.Servicios.ToList();
            }
            else {
                //Realiza busqueda de todos los productos/servicios.
                productos = _db.Productos.ToList();
                servicios = _db.Servicios.ToList();
            }


            //Si recibo una categoría, solo filtro productos/servicios pertenecientes a esa categoría.
            if (id != -1)
            {
                if (seccion.Equals("producto"))
                {
                    productos = _db.Categorias.Find(id).Productos;
                }
                else if (seccion.Equals("servicio"))
                {
                    servicios = _db.Categorias.Find(id).Servicios;
                }
                
            }

            if (busqueda == null || busqueda.Equals(""))
            {
                return Tuple.Create(productos, servicios, false);
            }

            if (seccion.Equals("producto"))
            {
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

                return Tuple.Create(productosFiltrados, servicios, agregadoExacto);
            }
            else if (seccion.Equals("servicio"))
            {
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

                return Tuple.Create(productos, serviciosFiltrados, agregadoExacto);
            }

            return null;
        }


        public static List<Categoria> buscarCategoriasPorTienda(TiendaOnlineContext _db, IEnumerable<Producto> productosTienda, IEnumerable<Servicio> serviciosTienda, bool esProducto)
        {
            List<Categoria> categorias = new List<Categoria>();
            foreach (Categoria cat in _db.Categorias.ToList()){

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
    }
}