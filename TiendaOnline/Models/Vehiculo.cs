using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TiendaOnline.Models
{
    [Table("Vehiculo")]
    public class Vehiculo
    {
        
        [Key]
        public int Id { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public int Year { get; set; }
        public int? SolicitudCotizacionId { get; set; }
        [ForeignKey("SolicitudCotizacionId")]
        public virtual SolicitudCotizacion SolicitudCotizacion { get; set; }

        public int? CotizacionId { get; set; }
        [ForeignKey("CotizacionId")]
        public virtual Cotizacion Cotizacion { get; set; }

        public static Vehiculo CrearVehiculo(TiendaOnlineContext _db, string _marca, string _modelo, int _year)
        {
            Vehiculo vehiculo = new Vehiculo();
            vehiculo.Marca = _marca;
            vehiculo.Modelo = _modelo;
            vehiculo.Year = _year;

            _db.Vehiculos.Add(vehiculo);
            _db.SaveChanges();

            return vehiculo;
        }
    }
}