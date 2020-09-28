using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace TiendaOnline.Models
{
    [Table("LogoRemitente")]
    public class LogoRemitente
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre identificador es obligatorio.")]
        public string Nombre { get; set; }
        public string DireccionLogo { get; set; }
        public string NombreImagen { get; set; }

        public int? TiendaId { get; set; }
        [ForeignKey("TiendaId")]
        public virtual Tienda Tienda { get; set; }
        public virtual List<Cotizacion> Cotizaciones { get; set; }

        public static LogoRemitente CrearNuevoLogo(TiendaOnlineContext _db, LogoRemitente _model, int idtienda, HttpPostedFileBase file)
        {
            if (_model.Nombre != null)
                _model.Nombre = _model.Nombre.ToUpper();

            _model.Nombre = _model.Nombre.Replace("\"", "");

            string nombreImagen = file.FileName;
            Tienda tienda = _db.Tienda.Where(t => t.Id == idtienda).FirstOrDefault();

            Imagen imagen = new Imagen();
            string path = Imagen.GuardarImagenEnTienda(tienda,"LogosRemitente", nombreImagen, file);

            if (path != null || !path.Equals("existe"))
            {
                LogoRemitente nuevo = new LogoRemitente();
                nuevo.Nombre = _model.Nombre;
                nuevo.Tienda = tienda;
                nuevo.DireccionLogo = path;
                nuevo.NombreImagen = nombreImagen;

                _db.LogoRemitente.Add(nuevo);

                _db.SaveChanges();

                return nuevo;
            }
            else
            {
                return null;
            }

        }

        public static void EliminarLogoRemitente(TiendaOnlineContext _db, LogoRemitente logoRemitente)
        {
            logoRemitente.Tienda = null;
            logoRemitente.Cotizaciones = new List<Cotizacion>();

            try {
                File.Delete(logoRemitente.DireccionLogo);
            }
            catch {
                //carpeta no existe
            }

            _db.LogoRemitente.Remove(logoRemitente);
            _db.SaveChanges();
        }
    }
}