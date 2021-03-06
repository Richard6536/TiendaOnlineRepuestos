﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.IO;
using System.Drawing;

namespace TiendaOnline.Models
{
    [Table("Imagen")]
    public class Imagen
    {
        //primer numero son MB
        public static float MAX_SIZE = 1.5f * 1024 * 1024;

        public static float MAX_SIZE_IN_MB
        {
            get
            {
                return MAX_SIZE / 1024 / 1024;
            }
        }


        public enum ImagenSize { Regular, MediaPagina, PaginaEntera }
        public enum ImagenTipo { Producto, Servicio, ProfileTienda, HeaderTienda }

        [Key]
        public int Id { get; set; }
        public string DireccionImagen { get; set; }
        public string NombreImagen { get; set; }
        public ImagenSize DimensionImagen { get; set; }
        public ImagenTipo TipoImagen { get; set; }

        public int? ProductoId { get; set; }
        [ForeignKey("ProductoId")]
        public virtual Producto Producto { get; set; }

        public int? ServicioId { get; set; }
        [ForeignKey("ServicioId")]
        public virtual Servicio Servicio { get; set; }

        public int? TiendaId { get; set; }
        [ForeignKey("TiendaId")]
        public virtual Tienda Tienda { get; set; }

        public static Producto SubirImagenesProducto(List<HttpPostedFileBase> files, TiendaOnlineContext _db, Producto _producto)
        {
            Producto producto = _db.Productos.Where(e => e.Nombre == _producto.Nombre).FirstOrDefault();

            foreach (HttpPostedFileBase file in files)
            {
                Imagen nuevaImagen = new Imagen();
                string filename = Path.GetRandomFileName() + Path.GetExtension(file.FileName);
                string path = GuardarImagenEnTienda(_producto.Tienda, "Productos", filename, file);

                if (path != "existe")
                {
                    nuevaImagen.DireccionImagen = path;
                    nuevaImagen.DimensionImagen = ImagenSize.Regular;
                    nuevaImagen.TipoImagen = ImagenTipo.Producto;
                    nuevaImagen.Producto = producto;
                    nuevaImagen.NombreImagen = filename;

                    _db.Imagenes.Add(nuevaImagen);
                    _db.SaveChanges();

                }
            }

            return producto;
        }

        public static Servicio SubirImagenesServicio(List<HttpPostedFileBase> files, TiendaOnlineContext _db, Servicio _servicio)
        {
            Servicio servicio = _db.Servicios.Where(e => e.Nombre == _servicio.Nombre).FirstOrDefault();

            foreach (HttpPostedFileBase file in files)
            {
                Imagen nuevaImagen = new Imagen();
                string filename = Path.GetRandomFileName() + Path.GetExtension(file.FileName);
                string path = GuardarImagenEnTienda(_servicio.Tienda, "Servicios", filename, file);

                if (path != "existe")
                {
                    nuevaImagen.DireccionImagen = path;
                    nuevaImagen.DimensionImagen = ImagenSize.Regular;
                    nuevaImagen.TipoImagen = ImagenTipo.Servicio;
                    nuevaImagen.Servicio = servicio;
                    nuevaImagen.NombreImagen = filename;

                    _db.Imagenes.Add(nuevaImagen);
                    _db.SaveChanges();

                }
            }



            return servicio;
        }

        //sirve para subir cualquier imagen, no solo de productos
        public static string GuardarImagenEnTienda(Tienda tienda, string carpeta, string nombrefile, HttpPostedFileBase file)
        {
            int tiendaId = tienda.Id;
            string fullPath = "";

            var basePath = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/ImagenesUsers/"+tiendaId+"/"+carpeta));
            if (!File.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }

            fullPath = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/ImagenesUsers/" + tiendaId + "/" + carpeta), nombrefile);
            try
            {
                if (File.Exists(fullPath))
                {
                    return "existe";
                }
                else
                {
                    file.SaveAs(fullPath);
                }
            }
            catch (Exception e)
            {
                String message = e.StackTrace;
                return null;
            }

            return fullPath;
        }

        public static string GuardarLogoEnTienda(TiendaOnlineContext _db, Tienda tienda, string carpeta, string nombrefile, string file, ImagenTipo tipoImagen)
        {
            string convertedBase64String = file.Replace("data:image/png;base64,", String.Empty);

            int tiendaId = tienda.Id;
            string fullPath = "";

            var basePath = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/ImagenesUsers/" + tiendaId + "/" + carpeta));
            if (!File.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }

            fullPath = Path.Combine(System.Web.Hosting.HostingEnvironment.MapPath("~/ImagenesUsers/" + tiendaId + "/" + carpeta), nombrefile);
            try
            {
                if (File.Exists(fullPath)){
                    if (ImagenTipo.HeaderTienda == tipoImagen)
                        EliminarImagen(_db, tienda.ImageHeader.Id);
                    else if (ImagenTipo.ProfileTienda == tipoImagen)
                        EliminarImagen(_db, tienda.ImageProfile.Id);
                }


                Imagen nuevaImagen = new Imagen();

                nuevaImagen.DireccionImagen = fullPath;
                nuevaImagen.DimensionImagen = ImagenSize.Regular;
                nuevaImagen.TipoImagen = tipoImagen;
                nuevaImagen.Tienda = tienda;
                nuevaImagen.NombreImagen = nombrefile;

                if (ImagenTipo.HeaderTienda == tipoImagen)
                    tienda.ImageHeader = nuevaImagen;
                else if (ImagenTipo.ProfileTienda == tipoImagen)
                    tienda.ImageProfile = nuevaImagen;

                _db.Imagenes.Add(nuevaImagen);
                _db.SaveChanges();

                File.WriteAllBytes(fullPath, Convert.FromBase64String(convertedBase64String));

            }
            catch (Exception e)
            {
                String message = e.StackTrace;
                return null;
            }

            return fullPath;
        }

        public static bool EliminarImagen(TiendaOnlineContext _db, int id)
        {
            Imagen imagen = _db.Imagenes.Find(id);
            if (imagen == null)
                return false;

            imagen.Producto = null;
            imagen.ProductoId = null;

            imagen.Servicio = null;
            imagen.ServicioId = null;

            imagen.Tienda = null;
            imagen.TiendaId = null;

            try
            {
                File.Delete(imagen.DireccionImagen);
            }
            catch
            {
                //carpeta no existe
            }

            _db.Imagenes.Remove(imagen);
            _db.SaveChanges();

            return true;
        }

    }
}