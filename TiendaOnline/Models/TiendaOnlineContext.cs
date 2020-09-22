using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations.Schema;

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
        public DbSet<UsuarioTiendaMecanico> UsuarioTiendaMecanicos { get; set; }
        public DbSet<Carrocompra> CarrosCompra { get; set; }
        public DbSet<Comentario> Comentarios { get; set; }
        public DbSet<ComentarioRespuesta> ComentarioRespuesta { get; set; }
        public DbSet<Imagen> Imagenes { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Servicio> Servicios { get; set; }
        public DbSet<ProductoCarro> ProductosCarro { get; set; }
        public DbSet<PuntuacionProducto> PuntuacionesProducto { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<ReporteErrores> ReportesErrores { get; set; }
        public DbSet<Tienda> Tienda { get; set; }
        public DbSet<SolicitudCotizacion> SolicitudCotizacion { get; set; }
        public DbSet<Cotizacion> Cotizacions { get; set; }
        public DbSet<LogoRemitente> LogoRemitente { get; set; }
        public DbSet<EmailConRefreshToken> EmailsConRefreshTokens { get; set; }
        public DbSet<Calendario> Calendario { get; set; }
        public DbSet<Agenda> Agenda { get; set; }
        public DbSet<Cita> Cita { get; set; }
        public DbSet<HorarioTienda> HorarioTiendas { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //random ejemplo
            //modelBuilder.Entity<Ruta>()
            //    .HasRequired(r => r.Linea)
            //    .WithMany(l => l.Rutas)
            //    .HasForeignKey(r => r.LineaId)
            //    .WillCascadeOnDelete(false);

            //modelBuilder.Entity<UsuarioTienda>()
            //  .HasOptional(t => t.UsuarioTiendaMecanico)
            //  .WithRequired(ht => ht.UsuarioTienda);

            //modelBuilder.Entity<Cotizacion>()
            //  .HasOptional(t => t.Cita)
            //  .WithRequired(ht => ht.Cotizacion);

            //modelBuilder.Configurations.Add(new Class1Map());
        }

        //class Class1Map : EntityTypeConfiguration<Cotizacion>
        //{
        //    public Class1Map()
        //    {
        //        this.HasKey(c => c.Id);
        //        this.Property(c => c.Id)
        //            .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        //        this.HasRequired(c1 => c1.Cita).WithRequiredPrincipal(c2 => c2.Cotizacion);
        //    }
        //}

    }
}