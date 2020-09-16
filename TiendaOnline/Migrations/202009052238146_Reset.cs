namespace TiendaOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Reset : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Carrocompra",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Total = c.Single(nullable: false),
                        UsuarioId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Usuario", t => t.UsuarioId)
                .Index(t => t.UsuarioId);
            
            CreateTable(
                "dbo.ProductoCarro",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Cantidad = c.Int(nullable: false),
                        Total = c.Single(nullable: false),
                        CarroId = c.Int(),
                        ProductoId = c.Int(),
                        ServicioId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Carrocompra", t => t.CarroId)
                .ForeignKey("dbo.Servicio", t => t.ServicioId)
                .ForeignKey("dbo.Producto", t => t.ProductoId)
                .Index(t => t.CarroId)
                .Index(t => t.ProductoId)
                .Index(t => t.ServicioId);
            
            CreateTable(
                "dbo.Producto",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false),
                        Stock = c.Int(nullable: false),
                        Precio = c.Single(nullable: false),
                        Descripcion = c.String(),
                        Modelo = c.String(),
                        Marca = c.String(),
                        EsGenerico = c.Boolean(nullable: false),
                        Year = c.Int(nullable: false),
                        PuntuacionActual = c.Single(nullable: false),
                        SoloCotizable = c.Boolean(nullable: false),
                        CategoriaId = c.Int(),
                        TiendaId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categoria", t => t.CategoriaId)
                .ForeignKey("dbo.Tienda", t => t.TiendaId)
                .Index(t => t.CategoriaId)
                .Index(t => t.TiendaId);
            
            CreateTable(
                "dbo.Categoria",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NombreCategoria = c.String(),
                        TipoCategoria = c.Int(nullable: false),
                        TiendaId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tienda", t => t.TiendaId)
                .Index(t => t.TiendaId);
            
            CreateTable(
                "dbo.Servicio",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false),
                        Precio = c.Single(nullable: false),
                        Descripcion = c.String(),
                        PuntuacionActual = c.Single(nullable: false),
                        SoloCotizable = c.Boolean(nullable: false),
                        CategoriaId = c.Int(),
                        TiendaId = c.Int(),
                        Cotizacion_Id = c.Int(),
                        SolicitudCotizacion_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categoria", t => t.CategoriaId)
                .ForeignKey("dbo.Cotizacion", t => t.Cotizacion_Id)
                .ForeignKey("dbo.Tienda", t => t.TiendaId)
                .ForeignKey("dbo.SolicitudCotizacions", t => t.SolicitudCotizacion_Id)
                .Index(t => t.CategoriaId)
                .Index(t => t.TiendaId)
                .Index(t => t.Cotizacion_Id)
                .Index(t => t.SolicitudCotizacion_Id);
            
            CreateTable(
                "dbo.Comentario",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Mensaje = c.String(),
                        Fecha = c.DateTime(nullable: false),
                        UsuarioId = c.Int(),
                        ProductoId = c.Int(),
                        ServicioId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Usuario", t => t.UsuarioId)
                .ForeignKey("dbo.Producto", t => t.ProductoId)
                .ForeignKey("dbo.Servicio", t => t.ServicioId)
                .Index(t => t.UsuarioId)
                .Index(t => t.ProductoId)
                .Index(t => t.ServicioId);
            
            CreateTable(
                "dbo.ComentarioRespuesta",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Mensaje = c.String(),
                        Fecha = c.DateTime(nullable: false),
                        ComentarioId = c.Int(),
                        UsuarioId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Comentario", t => t.ComentarioId)
                .ForeignKey("dbo.Usuario", t => t.UsuarioId)
                .Index(t => t.ComentarioId)
                .Index(t => t.UsuarioId);
            
            CreateTable(
                "dbo.Usuario",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RolUsuario = c.Int(nullable: false),
                        NombreUsuario = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        Nombre = c.String(),
                        Apellido = c.String(),
                        NombreCompleto = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PuntuacionProducto",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Puntuacion = c.Single(nullable: false),
                        Fecha = c.DateTime(nullable: false),
                        UsuarioId = c.Int(),
                        ProductoId = c.Int(),
                        ServicioId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Producto", t => t.ProductoId)
                .ForeignKey("dbo.Servicio", t => t.ServicioId)
                .ForeignKey("dbo.Usuario", t => t.UsuarioId)
                .Index(t => t.UsuarioId)
                .Index(t => t.ProductoId)
                .Index(t => t.ServicioId);
            
            CreateTable(
                "dbo.UsuarioTienda",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RolTienda = c.Int(nullable: false),
                        TiendaId = c.Int(),
                        UsuarioId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tienda", t => t.TiendaId)
                .ForeignKey("dbo.Usuario", t => t.UsuarioId)
                .Index(t => t.TiendaId)
                .Index(t => t.UsuarioId);
            
            CreateTable(
                "dbo.Tienda",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false),
                        Correo = c.String(),
                        Telefono = c.Int(nullable: false),
                        Descripcion = c.String(),
                        Local = c.String(),
                        Direccion = c.String(),
                        Latitud = c.Double(nullable: false),
                        Longitud = c.Double(nullable: false),
                        ImageHeader_Id = c.Int(),
                        ImageProfile_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Imagen", t => t.ImageHeader_Id)
                .ForeignKey("dbo.Imagen", t => t.ImageProfile_Id)
                .Index(t => t.ImageHeader_Id)
                .Index(t => t.ImageProfile_Id);
            
            CreateTable(
                "dbo.ClienteTienda",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TiendaId = c.Int(),
                        UsuarioId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tienda", t => t.TiendaId)
                .ForeignKey("dbo.Usuario", t => t.UsuarioId)
                .Index(t => t.TiendaId)
                .Index(t => t.UsuarioId);
            
            CreateTable(
                "dbo.Cotizacion",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Codigo = c.String(),
                        Fecha = c.DateTime(nullable: false),
                        SubTotalNeto = c.Single(nullable: false),
                        Descuento = c.Single(nullable: false),
                        TotalNeto = c.Single(nullable: false),
                        IVA = c.Single(nullable: false),
                        TotalAPagar = c.Single(nullable: false),
                        UsuarioTiendaId = c.Int(),
                        TiendaId = c.Int(),
                        UsuarioId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tienda", t => t.TiendaId)
                .ForeignKey("dbo.Usuario", t => t.UsuarioId)
                .ForeignKey("dbo.UsuarioTienda", t => t.UsuarioTiendaId)
                .Index(t => t.UsuarioTiendaId)
                .Index(t => t.TiendaId)
                .Index(t => t.UsuarioId);
            
            CreateTable(
                "dbo.Imagen",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DireccionImagen = c.String(),
                        NombreImagen = c.String(),
                        DimensionImagen = c.Int(nullable: false),
                        TipoImagen = c.Int(nullable: false),
                        ProductoId = c.Int(),
                        ServicioId = c.Int(),
                        TiendaId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Producto", t => t.ProductoId)
                .ForeignKey("dbo.Servicio", t => t.ServicioId)
                .ForeignKey("dbo.Tienda", t => t.TiendaId)
                .Index(t => t.ProductoId)
                .Index(t => t.ServicioId)
                .Index(t => t.TiendaId);
            
            CreateTable(
                "dbo.SolicitudCotizacions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TiendaId = c.Int(),
                        UsuarioId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tienda", t => t.TiendaId)
                .ForeignKey("dbo.Usuario", t => t.UsuarioId)
                .Index(t => t.TiendaId)
                .Index(t => t.UsuarioId);
            
            CreateTable(
                "dbo.ReporteErrores",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Fecha = c.DateTime(nullable: false),
                        Mensaje = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductoCarro", "ProductoId", "dbo.Producto");
            DropForeignKey("dbo.Categoria", "TiendaId", "dbo.Tienda");
            DropForeignKey("dbo.ProductoCarro", "ServicioId", "dbo.Servicio");
            DropForeignKey("dbo.Comentario", "ServicioId", "dbo.Servicio");
            DropForeignKey("dbo.Comentario", "ProductoId", "dbo.Producto");
            DropForeignKey("dbo.UsuarioTienda", "UsuarioId", "dbo.Usuario");
            DropForeignKey("dbo.UsuarioTienda", "TiendaId", "dbo.Tienda");
            DropForeignKey("dbo.SolicitudCotizacions", "UsuarioId", "dbo.Usuario");
            DropForeignKey("dbo.SolicitudCotizacions", "TiendaId", "dbo.Tienda");
            DropForeignKey("dbo.Servicio", "SolicitudCotizacion_Id", "dbo.SolicitudCotizacions");
            DropForeignKey("dbo.Servicio", "TiendaId", "dbo.Tienda");
            DropForeignKey("dbo.Producto", "TiendaId", "dbo.Tienda");
            DropForeignKey("dbo.Tienda", "ImageProfile_Id", "dbo.Imagen");
            DropForeignKey("dbo.Tienda", "ImageHeader_Id", "dbo.Imagen");
            DropForeignKey("dbo.Imagen", "TiendaId", "dbo.Tienda");
            DropForeignKey("dbo.Imagen", "ServicioId", "dbo.Servicio");
            DropForeignKey("dbo.Imagen", "ProductoId", "dbo.Producto");
            DropForeignKey("dbo.Cotizacion", "UsuarioTiendaId", "dbo.UsuarioTienda");
            DropForeignKey("dbo.Cotizacion", "UsuarioId", "dbo.Usuario");
            DropForeignKey("dbo.Cotizacion", "TiendaId", "dbo.Tienda");
            DropForeignKey("dbo.Servicio", "Cotizacion_Id", "dbo.Cotizacion");
            DropForeignKey("dbo.ClienteTienda", "UsuarioId", "dbo.Usuario");
            DropForeignKey("dbo.ClienteTienda", "TiendaId", "dbo.Tienda");
            DropForeignKey("dbo.PuntuacionProducto", "UsuarioId", "dbo.Usuario");
            DropForeignKey("dbo.PuntuacionProducto", "ServicioId", "dbo.Servicio");
            DropForeignKey("dbo.PuntuacionProducto", "ProductoId", "dbo.Producto");
            DropForeignKey("dbo.ComentarioRespuesta", "UsuarioId", "dbo.Usuario");
            DropForeignKey("dbo.Comentario", "UsuarioId", "dbo.Usuario");
            DropForeignKey("dbo.Carrocompra", "UsuarioId", "dbo.Usuario");
            DropForeignKey("dbo.ComentarioRespuesta", "ComentarioId", "dbo.Comentario");
            DropForeignKey("dbo.Servicio", "CategoriaId", "dbo.Categoria");
            DropForeignKey("dbo.Producto", "CategoriaId", "dbo.Categoria");
            DropForeignKey("dbo.ProductoCarro", "CarroId", "dbo.Carrocompra");
            DropIndex("dbo.SolicitudCotizacions", new[] { "UsuarioId" });
            DropIndex("dbo.SolicitudCotizacions", new[] { "TiendaId" });
            DropIndex("dbo.Imagen", new[] { "TiendaId" });
            DropIndex("dbo.Imagen", new[] { "ServicioId" });
            DropIndex("dbo.Imagen", new[] { "ProductoId" });
            DropIndex("dbo.Cotizacion", new[] { "UsuarioId" });
            DropIndex("dbo.Cotizacion", new[] { "TiendaId" });
            DropIndex("dbo.Cotizacion", new[] { "UsuarioTiendaId" });
            DropIndex("dbo.ClienteTienda", new[] { "UsuarioId" });
            DropIndex("dbo.ClienteTienda", new[] { "TiendaId" });
            DropIndex("dbo.Tienda", new[] { "ImageProfile_Id" });
            DropIndex("dbo.Tienda", new[] { "ImageHeader_Id" });
            DropIndex("dbo.UsuarioTienda", new[] { "UsuarioId" });
            DropIndex("dbo.UsuarioTienda", new[] { "TiendaId" });
            DropIndex("dbo.PuntuacionProducto", new[] { "ServicioId" });
            DropIndex("dbo.PuntuacionProducto", new[] { "ProductoId" });
            DropIndex("dbo.PuntuacionProducto", new[] { "UsuarioId" });
            DropIndex("dbo.ComentarioRespuesta", new[] { "UsuarioId" });
            DropIndex("dbo.ComentarioRespuesta", new[] { "ComentarioId" });
            DropIndex("dbo.Comentario", new[] { "ServicioId" });
            DropIndex("dbo.Comentario", new[] { "ProductoId" });
            DropIndex("dbo.Comentario", new[] { "UsuarioId" });
            DropIndex("dbo.Servicio", new[] { "SolicitudCotizacion_Id" });
            DropIndex("dbo.Servicio", new[] { "Cotizacion_Id" });
            DropIndex("dbo.Servicio", new[] { "TiendaId" });
            DropIndex("dbo.Servicio", new[] { "CategoriaId" });
            DropIndex("dbo.Categoria", new[] { "TiendaId" });
            DropIndex("dbo.Producto", new[] { "TiendaId" });
            DropIndex("dbo.Producto", new[] { "CategoriaId" });
            DropIndex("dbo.ProductoCarro", new[] { "ServicioId" });
            DropIndex("dbo.ProductoCarro", new[] { "ProductoId" });
            DropIndex("dbo.ProductoCarro", new[] { "CarroId" });
            DropIndex("dbo.Carrocompra", new[] { "UsuarioId" });
            DropTable("dbo.ReporteErrores");
            DropTable("dbo.SolicitudCotizacions");
            DropTable("dbo.Imagen");
            DropTable("dbo.Cotizacion");
            DropTable("dbo.ClienteTienda");
            DropTable("dbo.Tienda");
            DropTable("dbo.UsuarioTienda");
            DropTable("dbo.PuntuacionProducto");
            DropTable("dbo.Usuario");
            DropTable("dbo.ComentarioRespuesta");
            DropTable("dbo.Comentario");
            DropTable("dbo.Servicio");
            DropTable("dbo.Categoria");
            DropTable("dbo.Producto");
            DropTable("dbo.ProductoCarro");
            DropTable("dbo.Carrocompra");
        }
    }
}
