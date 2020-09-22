using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace TiendaOnline.Models
{
    [Table("HorarioTienda")]
    public class HorarioTienda
    {
        public enum DiaHorarioTienda { Cerrado, Lunes, Martes, Miercoles, Jueves, Viernes, Sabado, Domingo }
        public enum EstadoTienda { Abierto, Cerrado}

        [Key]
        public int Id { get; set; }
        public DiaHorarioTienda Dia { get; set; }
        public EstadoTienda Estado { get; set; }
        public DateTime HoraApertura { get; set; }
        public DateTime HoraCierre { get; set; }
        public DateTime HoraDescansoInicio { get; set; }
        public DateTime HoraDescansoTermino { get; set; }
        public bool HorarioDescanso { get; set; }

        public int? TiendaId { get; set; }
        [ForeignKey("TiendaId")]
        public virtual Tienda Tienda { get; set; }

        public static void CrearHorarioPorDia(TiendaOnlineContext _db, int tiendaId, DiaHorarioTienda diaHorarioTienda,
            string hApertura, string hDescansoInicio, string hDescansoTermino, string hCierre, bool hDescanso)
        {
            Tienda tienda = _db.Tienda.Where(t => t.Id == tiendaId).FirstOrDefault();
       

            HorarioTienda horarioTiendaDia = new HorarioTienda();
            horarioTiendaDia.Dia = diaHorarioTienda;
            horarioTiendaDia.HoraApertura = DateTime.ParseExact(hApertura, "hh:mmtt", CultureInfo.InvariantCulture);
            horarioTiendaDia.HoraDescansoInicio = DateTime.ParseExact(hDescansoInicio, "hh:mmtt", CultureInfo.InvariantCulture);
            horarioTiendaDia.HoraDescansoTermino = DateTime.ParseExact(hDescansoTermino, "hh:mmtt", CultureInfo.InvariantCulture);
            horarioTiendaDia.HoraCierre = DateTime.ParseExact(hCierre, "hh:mmtt", CultureInfo.InvariantCulture);
            horarioTiendaDia.Estado = EstadoTienda.Abierto;
            horarioTiendaDia.HorarioDescanso = hDescanso;
            
            horarioTiendaDia.Tienda = tienda;
            tienda.Horario.Add(horarioTiendaDia);
            _db.SaveChanges();

        }
    }
}