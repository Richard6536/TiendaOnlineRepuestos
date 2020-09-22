using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;
using System.ComponentModel;
using System.Globalization;


namespace TiendaOnline.Models
{
    [Table("HorarioTrabajador")]
    public class HorarioTrabajador
    {
        public enum DiaHorarioTrabajador { Libre, Lunes, Martes, Miercoles, Jueves, Viernes, Sabado, Domingo }

        [Key]
        public int Id { get; set; }
        public DiaHorarioTrabajador Dia { get; set; }
        public DateTime HoraEntrada { get; set; }
        public DateTime HoraSalida { get; set; }
        public DateTime HoraDescansoInicio { get; set; }
        public DateTime HoraDescansoTermino { get; set; }
        public bool HorarioDescanso { get; set; }

        public int? UsuarioTiendaMecanicoId { get; set; }
        [ForeignKey("UsuarioTiendaMecanicoId")]
        public virtual UsuarioTiendaMecanico UsuarioTiendaMecanico { get; set; }
    
        public static void CrearHorarioPorDia(TiendaOnlineContext _db, int tiendaId, int usuarioTiendaMecanicoId, DiaHorarioTrabajador diaHorarioTrabajador,
            string hEntrada, string hDescansoInicio, string hDescansoTermino, string hSalida, bool hDescanso)
        {
            Tienda tienda = _db.Tienda.Where(t => t.Id == tiendaId).FirstOrDefault();
            UsuarioTienda usuarioTienda = tienda.UsuariosTienda.Where(ut => ut.Id == usuarioTiendaMecanicoId).FirstOrDefault();

            HorarioTrabajador horarioTrabajadorDia = new HorarioTrabajador();
            horarioTrabajadorDia.Dia = diaHorarioTrabajador;
            horarioTrabajadorDia.HoraEntrada = DateTime.ParseExact(hEntrada, "hh:mmtt", CultureInfo.InvariantCulture);
            horarioTrabajadorDia.HoraDescansoInicio = DateTime.ParseExact(hDescansoInicio, "hh:mmtt", CultureInfo.InvariantCulture);
            horarioTrabajadorDia.HoraDescansoTermino = DateTime.ParseExact(hDescansoTermino, "hh:mmtt", CultureInfo.InvariantCulture);
            horarioTrabajadorDia.HoraSalida = DateTime.ParseExact(hSalida, "hh:mmtt", CultureInfo.InvariantCulture);
            horarioTrabajadorDia.HorarioDescanso = hDescanso;

            usuarioTienda.UsuariosTiendaMecanicos.First().Horario.Add(horarioTrabajadorDia);
            _db.SaveChanges();

        }
    }
}