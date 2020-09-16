namespace TiendaOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LogoRemitente : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LogoRemitente",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(nullable: false),
                        DireccionLogo = c.String(),
                        NombreImagen = c.String(),
                        TiendaId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tienda", t => t.TiendaId)
                .Index(t => t.TiendaId);
            
            AddColumn("dbo.Cotizacion", "LogoId", c => c.Int());
            CreateIndex("dbo.Cotizacion", "LogoId");
            AddForeignKey("dbo.Cotizacion", "LogoId", "dbo.LogoRemitente", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LogoRemitente", "TiendaId", "dbo.Tienda");
            DropForeignKey("dbo.Cotizacion", "LogoId", "dbo.LogoRemitente");
            DropIndex("dbo.LogoRemitente", new[] { "TiendaId" });
            DropIndex("dbo.Cotizacion", new[] { "LogoId" });
            DropColumn("dbo.Cotizacion", "LogoId");
            DropTable("dbo.LogoRemitente");
        }
    }
}
