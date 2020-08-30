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

    }
}