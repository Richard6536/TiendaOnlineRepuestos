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
    [Table("Servicio")]
    public class Servicio
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo Nombre es obligatorio")]
        public string Nombre { get; set; }

        public float Precio { get; set; }

        public string Descripcion { get; set; }

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

        public virtual List<ServicioCotizado> ServiciosCotizados { get; set; }
        public virtual List<ServicioSolicitadoCotizacion> ServiciosSolicitadosCotizacion { get; set; }

        public virtual List<SolicitudCotizacion> SolicitudCotizaciones { get; set; }
        public virtual List<ProductoCarro> ProductosCarro { get; set; }

        public static bool CrearServicio(Servicio _servicio, Tienda _tienda, TiendaOnlineContext _db)
        {
            Servicio serv = _tienda.Servicios.Where(p => p.Nombre == _servicio.Nombre).FirstOrDefault();
  
            _tienda.Servicios.Add(_servicio);
            _db.SaveChanges();

            return true;
        }

        public static bool EditarServicio(Servicio _servicio, Tienda _tienda, TiendaOnlineContext _db)
        {
            Servicio serv = _tienda.Servicios.Where(p => p.Id == _servicio.Id).FirstOrDefault();
            if (serv == null)
                return false;

            Categoria categoria = _db.Categorias.Find(_servicio.CategoriaId);

            serv.Nombre = _servicio.Nombre;
            serv.Precio = _servicio.Precio;
            serv.Categoria = categoria;
            serv.SoloCotizable = _servicio.SoloCotizable;
            serv.Descripcion = _servicio.Descripcion;

            _db.SaveChanges();

            return true;
        }

        public static void EliminarServicio(TiendaOnlineContext _db, Tienda _tienda, int id)
        {
            Servicio servicio = _tienda.Servicios.Where(p => p.Id == id).FirstOrDefault();

            if (servicio == null)
                return;

            List<Imagen> imagenesServicio = servicio.Imagenes.ToList();

            try
            {
                foreach (Imagen imagen in imagenesServicio)
                {
                    System.IO.File.Delete(imagen.DireccionImagen);
                }

            }
            catch
            {
                //carpeta no existe
            }

            _db.Imagenes.RemoveRange(imagenesServicio);

            _db.Servicios.Remove(servicio);
            _db.SaveChanges();
        }


        public static Comentario EnviarComentario(int id, int iduser, string comentario, TiendaOnlineContext _db)
        {
            Servicio servicio = _db.Servicios.Find(id);
            Usuario usuario = _db.Usuarios.Find(iduser);

            Comentario cp = new Comentario();
            cp.Mensaje = comentario;
            cp.Fecha = DateTime.Now;
            cp.Usuario = usuario;
            cp.Servicio = servicio;

            servicio.Comentarios.Add(cp);

            _db.SaveChanges();

            return cp;
        }

        public static void EliminarComentario(TiendaOnlineContext _db, Servicio _servicio, int _idComentario)
        {
            Comentario comentario = _servicio.Comentarios.Where(p => p.Id == _idComentario).FirstOrDefault();

            if (comentario == null)
                return;

            _db.Comentarios.Remove(comentario);
            _db.SaveChanges();
        }

        public static Tuple<Comentario, Boolean> EditarComentario(TiendaOnlineContext _db, Servicio _servicio, int _idComentario, string _mensajeEdit)
        {
            Comentario comentario = _servicio.Comentarios.Where(p => p.Id == _idComentario).FirstOrDefault();
            if (comentario == null)
                return new Tuple<Comentario, Boolean>(null, false);

            comentario.Mensaje = _mensajeEdit;
            comentario.Fecha = DateTime.Now;

            _db.SaveChanges();

            return new Tuple<Comentario, Boolean>(comentario, true); ;
        }

        public static string Truncate(string value, int maxChars)
        {
            return value.Length <= maxChars ? value : value.Substring(0, maxChars) + "...";
        }
    }
}