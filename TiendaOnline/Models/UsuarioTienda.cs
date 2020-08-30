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
    [Table("UsuarioTienda")]
    public class UsuarioTienda
    {
        public enum RolEnTienda { Admin, Moderador, Bloqueado}

        [Key]
        public int Id { get; set; }

        public RolEnTienda RolTienda { get; set; }

        public int? TiendaId { get; set; }
        [ForeignKey("TiendaId")]
        public virtual Tienda Tienda { get; set; }

        public int? UsuarioId { get; set; }
        [ForeignKey("UsuarioId")]
        public virtual Usuario Usuario { get; set; }
        public static UsuarioTienda CrearUsuarioTienda(TiendaOnlineContext _db, int idUsuario, Tienda tienda)
        {
            //Asignar Rol Administrador a Usuario
            Usuario usuario = _db.Usuarios.Find(idUsuario);
            Usuario.EditarUsuario(_db, usuario, tienda.Id, UsuarioTienda.RolEnTienda.Admin);
            UsuarioTienda usTienda = _db.UsuariosTienda.Where(ut => ut.Usuario.Id == idUsuario).FirstOrDefault();

            return usTienda;
        }

    }

}