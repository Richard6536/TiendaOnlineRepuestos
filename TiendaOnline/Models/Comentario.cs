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
    [Table("Comentario")]
    public class Comentario
    {
        [Key]
        public int Id { get; set; }

        public string Mensaje { get; set; }
        public DateTime Fecha { get; set; }
        public virtual List<ComentarioRespuesta> ComentariosRespuesta { get; set; }

        public int? UsuarioId { get; set; }
        [ForeignKey("UsuarioId")]
        public virtual Usuario Usuario { get; set; }

        public int? ProductoId { get; set; }
        [ForeignKey("ProductoId")]
        public virtual Producto Producto { get; set; }

        public int? ServicioId { get; set; }
        [ForeignKey("ServicioId")]
        public virtual Servicio Servicio { get; set; }

        public static ComentarioRespuesta ResponderComentario(TiendaOnlineContext _db, int _idComentario, int _idUsuario, string _respuesta)
        {
            Comentario comentario = _db.Comentarios.Find(_idComentario);
            Usuario usuario = _db.Usuarios.Find(_idUsuario);
            ComentarioRespuesta comentarioRespuesta = new ComentarioRespuesta();

            if (usuario.RolEnTienda == UsuarioTienda.RolEnTienda.Admin)
            {
                comentarioRespuesta.Mensaje = _respuesta;
                comentarioRespuesta.Fecha = DateTime.Now;
                comentarioRespuesta.Comentario = comentario;
                comentarioRespuesta.Usuario = usuario;

                comentario.ComentariosRespuesta.Add(comentarioRespuesta);
                _db.SaveChanges();
            }


            return comentarioRespuesta;

        }

        public static void EliminarComentarioRespuesta(TiendaOnlineContext _db, Comentario _comentario, int _idComentarioRespuesta)
        {
            ComentarioRespuesta comentarioRespuesta = _comentario.ComentariosRespuesta.Where(cr => cr.Id == _idComentarioRespuesta).FirstOrDefault();
            if (comentarioRespuesta == null)
                return;

            _db.ComentarioRespuesta.Remove(comentarioRespuesta);
            _db.SaveChanges();
        }

        public static Tuple<ComentarioRespuesta, Boolean> EditarComentarioRespuesta(TiendaOnlineContext _db, Comentario _comentario, int _idComentarioRespuesta, string _mensajeEdit)
        {
            ComentarioRespuesta comentarioRespuesta = _comentario.ComentariosRespuesta.Where(cr => cr.Id == _idComentarioRespuesta).FirstOrDefault();

            if (comentarioRespuesta == null)
                return new Tuple<ComentarioRespuesta, Boolean>(null, false);

            comentarioRespuesta.Mensaje = _mensajeEdit;
            comentarioRespuesta.Fecha = DateTime.Now;

            _db.SaveChanges();

            return new Tuple<ComentarioRespuesta, Boolean>(comentarioRespuesta, true);
        }
    }

}