using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using TiendaOnline.Models;
using TiendaOnline.Controllers;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace TiendaOnline.Clases
{
    public class Email
    {

        [Required, Display(Name = "Your name")]
        public string Tema { get; set; }

        [Required, Display(Name = "Your email"), EmailAddress]
        public string EmailDesde { get; set; }

        public string EmailPara { get; set; }
        public int ClienteId { get; set; }
        public string NombreDestinatario { get; set; }
        [Required]
        public string Mensaje { get; set; }
        public string Token { get; set; }
        public Cotizacion Cotizacion { get; set; }
        public int IdTienda { get; set; }
        public int IdUsuario { get; set; }
        public int IdDocumento { get; set; }
        public string CodigoDocumento { get; set; }
        public string NombreDocumento { get; set; }
        public string PathDocumentoPDF { get; set; }

        public string CrearDirectorio()
        {

            string path = @"C:\CotizacionPdf";

            if (!(Directory.Exists(path)))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }
    }
}