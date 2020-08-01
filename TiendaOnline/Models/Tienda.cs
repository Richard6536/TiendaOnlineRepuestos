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
    [Table("Tienda")]
    public class Tienda
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo Nombre es obligatorio")]
        public string Nombre { get; set; }

        public virtual List<Producto> Productos { get; set; }
        public virtual List<Servicio> Servicios { get; set; }

        public virtual List<UsuarioTienda> UsuariosTienda { get; set; }

        public static Tienda CrearNuevaTienda(TiendaOnlineContext _db, Tienda _model)
        {
            Tienda tienda = new Tienda();
            tienda = _model;

            _db.Tienda.Add(tienda);
            _db.SaveChanges();

            return tienda;
        }

        public static Tienda EditarTienda(TiendaOnlineContext _db, Tienda _model)
        {
            Tienda tienda = _db.Tienda.Where(t => t.Id == _model.Id).FirstOrDefault();
            tienda = _model;

            _db.Tienda.Add(tienda);
            _db.SaveChanges();

            return tienda;
        }

        public static void EliminarTienda(TiendaOnlineContext _db, int _id)
        {
            Tienda tienda = _db.Tienda.Where(t => t.Id == _id).FirstOrDefault();

            List<UsuarioTienda> usuariosTienda = tienda.UsuariosTienda.ToList();
            List<Usuario> usuariosParaRemoverTienda = new List<Usuario>();

            foreach (var usTienda in usuariosTienda)
            {
                usuariosParaRemoverTienda.Add(usTienda.Usuario);
            }

            foreach (var u in usuariosParaRemoverTienda)
            {
                u.UsuarioTiendas = null;
            }

            _db.SaveChanges();

            _db.Tienda.Remove(tienda);

            _db.SaveChanges();
        }
    }
}