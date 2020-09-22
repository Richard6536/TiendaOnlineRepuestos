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
    [Table("Calendario")]
    public class Calendario
    {
        [Key]
        public int Id { get; set; }
        public int? TiendaId { get; set; }
        [ForeignKey("TiendaId")]
        public virtual Tienda Tienda { get; set; }

        public List<Agenda> Agendas = new List<Agenda>();

        public static Calendario ObtenerAgendasTienda(TiendaOnlineContext _db, int idTienda)
        {
            Tienda tienda = _db.Tienda.Where(t => t.Id == idTienda).FirstOrDefault();
            Calendario calendario = tienda.Calendarios.First();

            List<UsuarioTiendaMecanico> usuarioTiendaMecanico = tienda.UsuarioTiendaMecanicos.Where(m => m.Agendas.Count > 0).ToList();
            
            foreach (var usm in usuarioTiendaMecanico)
            {
                calendario.Agendas.Add(usm.Agendas.First());
            }

            return calendario;
        }
    }
}