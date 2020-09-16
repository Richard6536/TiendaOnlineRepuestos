using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;
using System.ComponentModel;
using TiendaOnline.Clases;

namespace TiendaOnline.Models
{
    [Table("Usuario")]
    public class Usuario
    {

        public enum TipoUsuario { SuperAdmin, Cliente, Bloqueado }

        [Key]
        public int Id { get; set; }

        public TipoUsuario RolUsuario { get; set; }

        [Required(ErrorMessage = "El campo NombreUsuario es obligatorio")]
        public string NombreUsuario { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "El campo Email debe formato de correo electrónico.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El campo Password es obligatorio")]
        public string Password { get; set; }

        public string Nombre { get; set; }
        public string Apellido { get; set; }

        public string NombreCompleto { get; set; }

        public string Rut { get; set; }
        public string Domicilio { get; set; }
        public string Ciudad { get; set; }
        public string Pais { get; set; }
        public string Telefono { get; set; }

        public virtual List<Carrocompra> CarroCompra { get; set; }
        public virtual List<PuntuacionProducto> Puntuaciones { get; set; }
        public virtual List<Comentario> Comentarios { get; set; }
        public virtual List<ComentarioRespuesta> ComentariosRespuestas { get; set; }
        public virtual List<Cotizacion> Cotizaciones { get; set; }

        //----------------------------------
        public virtual List<UsuarioTienda> UsuarioTiendas { get; set; } //Debe ser un solo UsuarioTienda
        //public virtual List<UsuarioCliente> UsuarioCliente { get; set; } //Debe ser un solo Usuario
        //----------------------------------

        [NotMapped] //solo usado al crear/editar usuarios. No está asociado a la base de datos
        public UsuarioTienda.RolEnTienda RolEnTienda { get; set; }


        public static Usuario CrearNuevoUsuario(TiendaOnlineContext _db, Usuario _model, 
            int _tiendaId = 0, UsuarioTienda.RolEnTienda _rolTienda = UsuarioTienda.RolEnTienda.Admin) //opcionales
        {
            if (_model.NombreUsuario != null)
                _model.NombreUsuario = _model.NombreUsuario.ToUpper();
            if (_model.Nombre != null)
                _model.Nombre = _model.Nombre.ToUpper();
            if (_model.Apellido != null)
                _model.Apellido = _model.Apellido.ToUpper();

            Usuario existe = _db.Usuarios.Where(u => u.NombreUsuario == _model.NombreUsuario).FirstOrDefault();

            if (existe != null)
                return null;


            string pass = PasswordHash.CreateHash(_model.Password);
            _model.Password = pass;
            _model.NombreCompleto = _model.Nombre + " " + _model.Apellido;

            if(_tiendaId > 0)
            {
                _model.UsuarioTiendas = new List<UsuarioTienda>();

                Tienda tienda = _db.Tienda.Where(t => t.Id == _tiendaId).FirstOrDefault();
                UsuarioTienda ut = new UsuarioTienda();
                ut.RolTienda = _rolTienda;
                ut.Tienda = tienda;
                ut.TiendaId = tienda.Id;

                _model.UsuarioTiendas.Add(ut);
            }

            _db.Usuarios.Add(_model);
            _db.SaveChanges();


            return _model;
        }


        public static Usuario EditarUsuario(TiendaOnlineContext _db, Usuario _model,
            int _tiendaId = 0, UsuarioTienda.RolEnTienda _rolTienda = UsuarioTienda.RolEnTienda.Admin) //opcionales)
        {
            if (_model.NombreUsuario != null)
                _model.NombreUsuario = _model.NombreUsuario.ToUpper();
            if (_model.Nombre != null)
                _model.Nombre = _model.Nombre.ToUpper();
            if (_model.Apellido != null)
                _model.Apellido = _model.Apellido.ToUpper();


            Usuario usuarioAEditar = _db.Usuarios.Find(_model.Id);

            usuarioAEditar.NombreUsuario = _model.NombreUsuario;
            usuarioAEditar.Nombre = _model.Nombre;
            usuarioAEditar.Apellido = _model.Apellido;
            usuarioAEditar.Email = _model.Email;
            usuarioAEditar.RolUsuario = _model.RolUsuario;
            usuarioAEditar.NombreCompleto = _model.Nombre + " " + _model.Apellido;

            if(_tiendaId > 0)
            {
                UsuarioTienda existeAsoc = usuarioAEditar.UsuarioTiendas.Where(ut => ut.Tienda.Id == _tiendaId).FirstOrDefault();

                if (existeAsoc != null)
                {
                    existeAsoc.RolTienda = _rolTienda;
                }
                else
                {
                    Tienda tienda = _db.Tienda.Where(t => t.Id == _tiendaId).FirstOrDefault();
                    UsuarioTienda ut = new UsuarioTienda();
                    ut.RolTienda = _rolTienda;
                    ut.Tienda = tienda;
                    ut.TiendaId = tienda.Id;

                    _model.UsuarioTiendas.Add(ut);
                }
            }


            _db.SaveChanges();

            return usuarioAEditar;
        }

        public static Usuario EliminarUsuario(TiendaOnlineContext _db, int id)
        {
            Usuario usuario = _db.Usuarios.Find(id);

            List<UsuarioTienda> ustEliminar = new List<UsuarioTienda>();
            foreach(UsuarioTienda ut in usuario.UsuarioTiendas)
            {
                ut.Usuario = null;
                ut.Tienda = null;
                ustEliminar.Add(ut);
            }
            _db.UsuariosTienda.RemoveRange(ustEliminar);

            List<Carrocompra> carrosEliminar = new List<Carrocompra>();
            foreach (Carrocompra carr in usuario.CarroCompra)
            {
                carr.Usuario = null;
                carrosEliminar.Add(carr);
            }
            _db.CarrosCompra.RemoveRange(carrosEliminar);

            List<Comentario> comentariosEliminar = new List<Comentario>();
            foreach (Comentario com in usuario.Comentarios)
            {
                com.Usuario = null;
                comentariosEliminar.Add(com);
            }
            _db.Comentarios.RemoveRange(comentariosEliminar);

            List<PuntuacionProducto> puntuacionesEliminar = new List<PuntuacionProducto>();
            foreach (PuntuacionProducto punt in usuario.Puntuaciones)
            {
                punt.Usuario = null;
                puntuacionesEliminar.Add(punt);
            }
            _db.PuntuacionesProducto.RemoveRange(puntuacionesEliminar);

            _db.Usuarios.Remove(usuario);
            _db.SaveChanges();

            return usuario;
        }

        public static Usuario AgregarCarroCompra(TiendaOnlineContext _db, Usuario _model, Carrocompra _carroCompra)
        {
            Usuario user = _db.Usuarios.Where(u => u.NombreUsuario == _model.NombreUsuario).FirstOrDefault();
            user.CarroCompra.Add(_carroCompra);

            _db.SaveChanges();

            return user;
        }

        public static bool CambiarPassword(int id, string _passwordAntigua, string _password, TiendaOnlineContext _db)
        {
            Usuario user = _db.Usuarios.Find(id);
            bool valido = PasswordHash.ValidatePassword(_passwordAntigua, user.Password);

            if (valido == false)
                return false;

            string pass = PasswordHash.CreateHash(_password);
            user.Password = pass;

            _db.SaveChanges();

            return true;
        }

    }
}