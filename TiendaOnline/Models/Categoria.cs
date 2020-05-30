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
        [Key]
        public int Id { get; set; }

        public string NombreCategoria { get; set; }

        public int? TiendaId { get; set; }
        [ForeignKey("TiendaId")]
        public virtual Tienda Tienda { get; set; }

        public virtual List<Producto> Productos { get; set; }


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

        public static Tuple<List<Producto>, Boolean> BuscarProductosPorCategoria(TiendaOnlineContext _db, int id, string busqueda)
        {
            bool agregado = false;
            bool agregadoExacto = false;
            Producto productoExacto = null;

            List<Producto> productosFiltrados = new List<Producto>();
            List<Producto> productos = _db.Categorias.Find(id).Productos;
            if (busqueda == null || busqueda.Equals(""))
            {
                return Tuple.Create(productos, false);
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

            return Tuple.Create(productosFiltrados, agregadoExacto);

        }
    }
}