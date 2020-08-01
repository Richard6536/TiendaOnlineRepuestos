namespace TiendaOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Servicios : DbMigration
    {
        public override void Up()
        {
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
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categoria", t => t.CategoriaId)
                .ForeignKey("dbo.Tienda", t => t.TiendaId)
                .Index(t => t.CategoriaId)
                .Index(t => t.TiendaId);
            
            AddColumn("dbo.ProductoCarro", "ServicioId", c => c.Int());
            AddColumn("dbo.Categoria", "TipoCategoria", c => c.Int(nullable: false));
            AddColumn("dbo.Comentario", "ServicioId", c => c.Int());
            AddColumn("dbo.PuntuacionProducto", "ServicioId", c => c.Int());
            AddColumn("dbo.Imagen", "ServicioId", c => c.Int());
            CreateIndex("dbo.ProductoCarro", "ServicioId");
            CreateIndex("dbo.Comentario", "ServicioId");
            CreateIndex("dbo.PuntuacionProducto", "ServicioId");
            CreateIndex("dbo.Imagen", "ServicioId");
            AddForeignKey("dbo.Comentario", "ServicioId", "dbo.Servicio", "Id");
            AddForeignKey("dbo.PuntuacionProducto", "ServicioId", "dbo.Servicio", "Id");
            AddForeignKey("dbo.Imagen", "ServicioId", "dbo.Servicio", "Id");
            AddForeignKey("dbo.ProductoCarro", "ServicioId", "dbo.Servicio", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductoCarro", "ServicioId", "dbo.Servicio");
            DropForeignKey("dbo.Imagen", "ServicioId", "dbo.Servicio");
            DropForeignKey("dbo.Servicio", "TiendaId", "dbo.Tienda");
            DropForeignKey("dbo.PuntuacionProducto", "ServicioId", "dbo.Servicio");
            DropForeignKey("dbo.Comentario", "ServicioId", "dbo.Servicio");
            DropForeignKey("dbo.Servicio", "CategoriaId", "dbo.Categoria");
            DropIndex("dbo.Imagen", new[] { "ServicioId" });
            DropIndex("dbo.PuntuacionProducto", new[] { "ServicioId" });
            DropIndex("dbo.Comentario", new[] { "ServicioId" });
            DropIndex("dbo.Servicio", new[] { "TiendaId" });
            DropIndex("dbo.Servicio", new[] { "CategoriaId" });
            DropIndex("dbo.ProductoCarro", new[] { "ServicioId" });
            DropColumn("dbo.Imagen", "ServicioId");
            DropColumn("dbo.PuntuacionProducto", "ServicioId");
            DropColumn("dbo.Comentario", "ServicioId");
            DropColumn("dbo.Categoria", "TipoCategoria");
            DropColumn("dbo.ProductoCarro", "ServicioId");
            DropTable("dbo.Servicio");
        }
    }
}
