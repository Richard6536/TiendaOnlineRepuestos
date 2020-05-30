namespace TiendaOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
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
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Carrocompra", t => t.CarroId)
                .ForeignKey("dbo.Producto", t => t.ProductoId)
                .Index(t => t.CarroId)
                .Index(t => t.ProductoId);
            
            CreateTable(
                "dbo.Producto",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                        Stock = c.Int(nullable: false),
                        Precio = c.Single(nullable: false),
                        Descripcion = c.String(),
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
                        TiendaId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tienda", t => t.TiendaId)
                .Index(t => t.TiendaId);
            
            CreateTable(
                "dbo.Tienda",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
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
                "dbo.Usuario",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RolUsuario = c.Int(nullable: false),
                        NombreUsuario = c.String(),
                        Email = c.String(),
                        Password = c.String(),
                        Nombre = c.String(),
                        Apellido = c.String(),
                        NombreCompleto = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Comentario",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Mensaje = c.String(),
                        Fecha = c.DateTime(nullable: false),
                        UsuarioId = c.Int(),
                        ProductoId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Producto", t => t.ProductoId)
                .ForeignKey("dbo.Usuario", t => t.UsuarioId)
                .Index(t => t.UsuarioId)
                .Index(t => t.ProductoId);
            
            CreateTable(
                "dbo.PuntuacionProducto",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Puntuacion = c.Single(nullable: false),
                        Fecha = c.DateTime(nullable: false),
                        UsuarioId = c.Int(),
                        ProductoId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Producto", t => t.ProductoId)
                .ForeignKey("dbo.Usuario", t => t.UsuarioId)
                .Index(t => t.UsuarioId)
                .Index(t => t.ProductoId);
            
            CreateTable(
                "dbo.Imagen",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DireccionImagen = c.String(),
                        NombreImagen = c.String(),
                        DimensionImagen = c.Int(nullable: false),
                        ProductoId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Producto", t => t.ProductoId)
                .Index(t => t.ProductoId);
            
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
            DropForeignKey("dbo.Imagen", "ProductoId", "dbo.Producto");
            DropForeignKey("dbo.Categoria", "TiendaId", "dbo.Tienda");
            DropForeignKey("dbo.UsuarioTienda", "UsuarioId", "dbo.Usuario");
            DropForeignKey("dbo.PuntuacionProducto", "UsuarioId", "dbo.Usuario");
            DropForeignKey("dbo.PuntuacionProducto", "ProductoId", "dbo.Producto");
            DropForeignKey("dbo.Comentario", "UsuarioId", "dbo.Usuario");
            DropForeignKey("dbo.Comentario", "ProductoId", "dbo.Producto");
            DropForeignKey("dbo.Carrocompra", "UsuarioId", "dbo.Usuario");
            DropForeignKey("dbo.UsuarioTienda", "TiendaId", "dbo.Tienda");
            DropForeignKey("dbo.Producto", "TiendaId", "dbo.Tienda");
            DropForeignKey("dbo.Producto", "CategoriaId", "dbo.Categoria");
            DropForeignKey("dbo.ProductoCarro", "CarroId", "dbo.Carrocompra");
            DropIndex("dbo.Imagen", new[] { "ProductoId" });
            DropIndex("dbo.PuntuacionProducto", new[] { "ProductoId" });
            DropIndex("dbo.PuntuacionProducto", new[] { "UsuarioId" });
            DropIndex("dbo.Comentario", new[] { "ProductoId" });
            DropIndex("dbo.Comentario", new[] { "UsuarioId" });
            DropIndex("dbo.UsuarioTienda", new[] { "UsuarioId" });
            DropIndex("dbo.UsuarioTienda", new[] { "TiendaId" });
            DropIndex("dbo.Categoria", new[] { "TiendaId" });
            DropIndex("dbo.Producto", new[] { "TiendaId" });
            DropIndex("dbo.Producto", new[] { "CategoriaId" });
            DropIndex("dbo.ProductoCarro", new[] { "ProductoId" });
            DropIndex("dbo.ProductoCarro", new[] { "CarroId" });
            DropIndex("dbo.Carrocompra", new[] { "UsuarioId" });
            DropTable("dbo.ReporteErrores");
            DropTable("dbo.Imagen");
            DropTable("dbo.PuntuacionProducto");
            DropTable("dbo.Comentario");
            DropTable("dbo.Usuario");
            DropTable("dbo.UsuarioTienda");
            DropTable("dbo.Tienda");
            DropTable("dbo.Categoria");
            DropTable("dbo.Producto");
            DropTable("dbo.ProductoCarro");
            DropTable("dbo.Carrocompra");
        }
    }
}
