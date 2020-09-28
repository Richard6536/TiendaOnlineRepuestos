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
    [Table("Carrocompra")]
    public class Carrocompra
    {
        [Key]
        public int Id { get; set; }

        public float Total { get; set; }

        public virtual List<ProductoCarro> ProductosCarro { get; set; }

        public int? UsuarioId { get; set; }
        [ForeignKey("UsuarioId")]
        public virtual Usuario Usuario { get; set; }


        public static Carrocompra CrearCarroCompra(int _idUsuario, TiendaOnlineContext _db)
        {
            Usuario usuario = _db.Usuarios.Where(u => u.Id == _idUsuario).FirstOrDefault();
            if (usuario.CarroCompra.Count > 0)
            {
                return usuario.CarroCompra.First();
            }

            Carrocompra carroCompra = new Carrocompra();
            carroCompra.Total = 0;
            carroCompra.Usuario = usuario;

            _db.CarrosCompra.Add(carroCompra);
            _db.SaveChanges();

            return carroCompra;
        }

        public static void AgregarProductoAlCarroCompra(int _idUsuario, ProductoCarro _productoCarro, TiendaOnlineContext _db)
        {
            Usuario usuario = _db.Usuarios.Where(u => u.Id == _idUsuario).FirstOrDefault();
            usuario.CarroCompra.First().ProductosCarro = new List<ProductoCarro>(); //SOLO POR AHORA PORQUE SE DUPLICAN LOS ITEMS
            usuario.CarroCompra.First().ProductosCarro.Add(_productoCarro);
            usuario.CarroCompra.First().Total += _productoCarro.Total;

            _db.SaveChanges();
        }

        public static void EliminarTodosLosProductosEnCarro(Usuario _usuario, TiendaOnlineContext _db)
        {
            List<ProductoCarro> productosCarro = _db.ProductosCarro.ToList();

            if (_usuario.CarroCompra.Count == 0)
                return;

            Carrocompra miCarroCompra = _usuario.CarroCompra.First();

            if (productosCarro.Count > 0)
            {
                foreach (var prodCarro in productosCarro)
                {
                    _db.ProductosCarro.Remove(prodCarro);
                }
            }


            _db.SaveChanges();
        }

        public static Carrocompra ActualizarProductoEnCarro(int _idUsuario, int _idProducto, int _cantidad, TiendaOnlineContext _db)
        {
            Usuario usuario = _db.Usuarios.Where(u => u.Id == _idUsuario).FirstOrDefault();
            Carrocompra miCarro = usuario.CarroCompra.First();

            ProductoCarro prodCarro = miCarro.ProductosCarro.Where(pc => pc.Id == _idProducto).FirstOrDefault();
            prodCarro.Cantidad = _cantidad;
            prodCarro.Total = prodCarro.Producto.Precio * prodCarro.Cantidad;

            _db.SaveChanges();

            return miCarro;
        }

        public static void EliminarProductoCarroCompra(int? _idProductoCarro, TiendaOnlineContext _db)
        {

            ProductoCarro prodCarro = _db.ProductosCarro.Where(pc => pc.Id == _idProductoCarro).FirstOrDefault();
            _db.ProductosCarro.Remove(prodCarro);
            _db.SaveChanges();
        }
    }
}