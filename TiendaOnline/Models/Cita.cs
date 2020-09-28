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
    [Table("Cita")]
    public class Cita
    {
        [Key]
        public int Id { get; set; }
        public string Codigo { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaTermino { get; set; }

        public int? UsuarioTiendaMecanicoId { get; set; }
        [ForeignKey("UsuarioTiendaMecanicoId")]
        public virtual UsuarioTiendaMecanico UsuarioTiendaMecanico { get; set; }

        public int? CotizacionId { get; set; }
        [ForeignKey("CotizacionId")]
        public virtual Cotizacion Cotizacion { get; set; }

        public int? UsuarioId { get; set; }
        [ForeignKey("UsuarioId")]
        public virtual Usuario Usuario { get; set; }

        public static Cita CrearCita(TiendaOnlineContext _db, Cita _model)
        {
            UsuarioTiendaMecanico usuarioTiendaMecanico = _db.UsuarioTiendaMecanicos.Where(utm => utm.Id == _model.UsuarioTiendaMecanicoId).FirstOrDefault();
            Cotizacion cotizacion = _db.Cotizacions.Where(c => c.Id == _model.CotizacionId).FirstOrDefault();
            Usuario usuario = _db.Usuarios.Where(us => us.Id == _model.UsuarioId).FirstOrDefault();

            Cita cita = new Cita();
            cita.Codigo = "CITA"+cotizacion.Id.ToString()+usuario.NombreCompleto+usuario.Id.ToString();
            cita.FechaInicio = _model.FechaInicio;
            cita.FechaTermino = _model.FechaTermino;
            cita.Cotizacion = cotizacion;
            cita.Usuario = usuario;
            cita.UsuarioTiendaMecanico = usuarioTiendaMecanico;

            if (usuarioTiendaMecanico.Agendas == null)
                usuarioTiendaMecanico.Agendas = new List<Agenda>();

            //CREAR UNA AGENDA
            //Agenda agenda = new Agenda();
            //agenda.UsuarioTiendaMecanico = usuarioTiendaMecanico;
            //agenda.Citas = new List<Cita>();

            //usuarioTiendaMecanico.Agendas.Add(agenda);

            if (usuarioTiendaMecanico.Agendas.First().Citas == null)
                usuarioTiendaMecanico.Agendas.First().Citas = new List<Cita>();

            usuarioTiendaMecanico.Agendas.First().Citas.Add(cita);
            cotizacion.Citas.Add(cita);

            _db.SaveChanges();
            return cita;
        }
    }
}