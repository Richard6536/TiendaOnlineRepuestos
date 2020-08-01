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
    [Table("ProductoCarro")]
    public class ProductoCarro
    {
        [Key]
        public int Id { get; set; }

        public int Cantidad { get; set; }
        public float Total { get; set; }

        public int? CarroId { get; set; }
        [ForeignKey("CarroId")]
        public virtual Carrocompra CarroCompra { get; set; }

        public int? ProductoId { get; set; }
        [ForeignKey("ProductoId")]
        public virtual Producto Producto { get; set; }

        public int? ServicioId { get; set; }
        [ForeignKey("ServicioId")]
        public virtual Servicio Servicio { get; set; }

        public static ProductoCarro CrearProductoCarro(int? _idProducto, int _idUsuario, int _cantidad, TiendaOnlineContext _db)
        {
            Usuario usuario = _db.Usuarios.Where(u => u.Id == _idUsuario).FirstOrDefault();
            Producto producto = _db.Productos.Where(p => p.Id == _idProducto).FirstOrDefault();
            Carrocompra miCarro = usuario.CarroCompra.First();

            ProductoCarro existeProdCarro = miCarro.ProductosCarro.Where(pc => pc.Producto.Nombre == producto.Nombre).FirstOrDefault();
            if (existeProdCarro != null)
            {
                existeProdCarro.Cantidad += _cantidad;
                existeProdCarro.Total = producto.Precio * existeProdCarro.Cantidad;

                _db.SaveChanges();

                return existeProdCarro;

            }
            else
            {

                ProductoCarro productoCarro = new ProductoCarro();
                productoCarro.Cantidad = _cantidad;
                productoCarro.Total = producto.Precio * _cantidad;
                productoCarro.Producto = producto;
                productoCarro.CarroCompra = usuario.CarroCompra.First();

                _db.ProductosCarro.Add(productoCarro);

                _db.SaveChanges();

                return productoCarro;
            }

        }
    }
}