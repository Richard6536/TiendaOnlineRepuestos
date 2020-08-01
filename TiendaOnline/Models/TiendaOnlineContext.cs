using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace TiendaOnline.Models
{
    public class TiendaOnlineContext : DbContext
    {
        //Local
        public TiendaOnlineContext() : base("name=TiendaRepuestosOnlineDB")
        //REAL
        //public TiendaOnlineContext() : base("data source=173.248.151.67,1533;initial catalog=inmobiliariadb;user id=oscarch;password=121212.;MultipleActiveResultSets=True;App=EntityFramework")
        { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<UsuarioTienda> UsuariosTienda { get; set; }

        public DbSet<Carrocompra> CarrosCompra { get; set; }
        public DbSet<Comentario> Comentarios { get; set; }
        public DbSet<Imagen> Imagenes { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Servicio> Servicios { get; set; }
        public DbSet<ProductoCarro> ProductosCarro { get; set; }
        public DbSet<PuntuacionProducto> PuntuacionesProducto { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<ReporteErrores> ReportesErrores { get; set; }
        public DbSet<Tienda> Tienda { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //random ejemplo
            //modelBuilder.Entity<Ruta>()
            //    .HasRequired(r => r.Linea)
            //    .WithMany(l => l.Rutas)
            //    .HasForeignKey(r => r.LineaId)
            //    .WillCascadeOnDelete(false);


        }
    }
}