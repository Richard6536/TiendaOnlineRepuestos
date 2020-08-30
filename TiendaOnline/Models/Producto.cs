using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;
using System.ComponentModel;

namespace TiendaOnline.Models
{
    [Table("Producto")]
    public class Producto
    {

        public enum OrderByType { PrecioAscendente, PrecioDescendente, Relevante }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo Nombre es obligatorio")]
        public string Nombre { get; set; }

        public int Stock { get; set; }

        public float Precio { get; set; }

        public string Descripcion { get; set; }

        public string Modelo { get; set; }

        public string Marca { get; set; }

        public bool EsGenerico { get; set; }

        public int Year { get; set; }

        public float PuntuacionActual { get; set; }

        public bool SoloCotizable { get; set; }

        public int? CategoriaId { get; set; }
        [ForeignKey("CategoriaId")]
        public virtual Categoria Categoria { get; set; }

        public int? TiendaId { get; set; }
        [ForeignKey("TiendaId")]
        public virtual Tienda Tienda { get; set; }


        public virtual List<PuntuacionProducto> Puntuaciones { get; set; }
        public virtual List<Comentario> Comentarios { get; set; }
        public virtual List<Imagen> Imagenes { get; set; }


        public virtual List<ProductoCarro> ProductosCarro { get; set; }

        public static bool CrearProducto(Producto _producto, Tienda _tienda, TiendaOnlineContext _db)
        {
            Producto prod = _tienda.Productos.Where(p => p.Nombre == _producto.Nombre).FirstOrDefault();
            if (prod != null)
                return false;

            _tienda.Productos.Add(_producto);
            _db.SaveChanges();

            return true;
        }

        public static bool EditarProducto(Producto _producto, Tienda _tienda, TiendaOnlineContext _db)
        {
            Producto prod = _tienda.Productos.Where(p => p.Id == _producto.Id).FirstOrDefault();
            if (prod == null)
                return false;

            Categoria categoria = _db.Categorias.Find(_producto.CategoriaId);

            prod.Nombre = _producto.Nombre;
            prod.Precio = _producto.Precio;
            prod.Categoria = categoria;
            prod.SoloCotizable = _producto.SoloCotizable;
            prod.Descripcion = _producto.Descripcion;
            prod.Stock = _producto.Stock;
            prod.Marca = _producto.Marca;
            prod.Modelo = _producto.Modelo;
            prod.Year = _producto.Year;

            _db.SaveChanges();

            return true;
        }

        public static void EliminarProducto(TiendaOnlineContext _db, Tienda _tienda, int id)
        {
            Producto producto = _tienda.Productos.Where(p => p.Id == id).FirstOrDefault();

            if (producto == null)
                return;

            List<Imagen> imagenesProducto = producto.Imagenes.ToList();

            try
            {
                foreach (Imagen imagen in imagenesProducto)
                {
                    System.IO.File.Delete(imagen.DireccionImagen);
                }

            }
            catch
            {
                //carpeta no existe
            }

            _db.Imagenes.RemoveRange(imagenesProducto);

            _db.Productos.Remove(producto);
            _db.SaveChanges();
        }

        public static bool AumentarStock(int id, int cantidad, Tienda _tienda, TiendaOnlineContext _db)
        {
            Producto prod = _tienda.Productos.Where(p => p.Id == id).FirstOrDefault();
            if (prod == null)
                return false;

            prod.Stock += cantidad;

            _db.SaveChanges();

            return true;
        }

        public static void CambiarPuntuacion(int id, int iduser, int ratingValue, TiendaOnlineContext _db)
        {
            Producto producto = _db.Productos.Find(id);
            Usuario usuario = _db.Usuarios.Find(iduser);

            PuntuacionProducto existePuntuacionUsuario = producto.Puntuaciones.Where(ppto => ppto.Usuario.Id == iduser).FirstOrDefault();

            PuntuacionProducto pp = new PuntuacionProducto();
            pp.Puntuacion = (float)ratingValue;
            pp.Producto = producto;
            pp.Fecha = DateTime.Now;
            pp.Usuario = usuario;

            if (existePuntuacionUsuario != null)
            {
                _db.PuntuacionesProducto.Remove(existePuntuacionUsuario);
                _db.SaveChanges();
            }

            producto.Puntuaciones.Add(pp);
            float total = 0;

            Producto productoAct = _db.Productos.Find(id);
            foreach (var p in productoAct.Puntuaciones)
            {
                total += p.Puntuacion;
            }

            productoAct.PuntuacionActual = total / productoAct.Puntuaciones.Count();

            _db.SaveChanges();
        }

        public static Comentario EnviarComentario(int id, int iduser, string comentario, TiendaOnlineContext _db)
        {
            Producto producto = _db.Productos.Find(id);
            Usuario usuario = _db.Usuarios.Find(iduser);

            Comentario cp = new Comentario();
            cp.Mensaje = comentario;
            cp.Fecha = DateTime.Now;
            cp.Usuario = usuario;
            cp.Producto = producto;

            producto.Comentarios.Add(cp);

            _db.SaveChanges();

            return cp;
        }

        public static void EliminarComentario(TiendaOnlineContext _db, Producto _producto, int _idComentario)
        {
            Comentario comentario = _producto.Comentarios.Where(p => p.Id == _idComentario).FirstOrDefault();

            if (comentario == null)
                return;

            if (comentario.ComentariosRespuesta.Count > 0)
            {
                _db.ComentarioRespuesta.Remove(comentario.ComentariosRespuesta.First());
            }

            _db.Comentarios.Remove(comentario);
            _db.SaveChanges();
        }

        public static Tuple<Comentario, Boolean> EditarComentario(TiendaOnlineContext _db, Producto _producto, int _idComentario, string _mensajeEdit)
        {
            Comentario comentario = _producto.Comentarios.Where(p => p.Id == _idComentario).FirstOrDefault();
            if (comentario == null)
                return new Tuple<Comentario, Boolean>(null, false);

            comentario.Mensaje = _mensajeEdit;
            comentario.Fecha = DateTime.Now;

            _db.SaveChanges();

            return new Tuple<Comentario, Boolean>(comentario, true);
        }

        public static string Truncate(string value, int maxChars)
        {
            return value.Length <= maxChars ? value : value.Substring(0, maxChars) + "...";
        }

        public static Tuple<IEnumerable<Producto>, string> SortProduct(IEnumerable<Producto> productos, OrderByType orderByType)
        {
            String orderByProductName = "";
            IEnumerable<Producto> productosOrdenados = null;
            switch (orderByType)
            {
                case OrderByType.PrecioAscendente:
                    productosOrdenados = productos.OrderBy(p => p.Precio).ToList();
                    orderByProductName = "Precio Menor";
                    break;

                case OrderByType.PrecioDescendente:
                    productosOrdenados = productos.OrderByDescending(p => p.Precio).ToList();
                    orderByProductName = "Precio Mayor";
                    break;
            }

            return new Tuple<IEnumerable<Producto>, string>(productosOrdenados, orderByProductName);
        }
    }
}