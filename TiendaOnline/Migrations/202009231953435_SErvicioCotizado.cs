namespace TiendaOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SErvicioCotizado : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Servicio", new[] { "Cotizacion_Id" });
            CreateTable(
                "dbo.ServicioCotizado",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Cantidad = c.Int(nullable: false),
                        Valor = c.Single(nullable: false),
                        ValorTotal = c.Single(nullable: false),
                        Comentario = c.String(),
                        ServicioId = c.Int(nullable: false),
                        Cotizacion_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Servicio", t => t.ServicioId, cascadeDelete: true)
                .Index(t => t.ServicioId)
                .Index(t => t.Cotizacion_Id);
            
            DropColumn("dbo.Servicio", "Cotizacion_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Servicio", "Cotizacion_Id", c => c.Int());
            DropForeignKey("dbo.ServicioCotizado", "ServicioId", "dbo.Servicio");
            DropIndex("dbo.ServicioCotizado", new[] { "Cotizacion_Id" });
            DropIndex("dbo.ServicioCotizado", new[] { "ServicioId" });
            DropTable("dbo.ServicioCotizado");
            CreateIndex("dbo.Servicio", "Cotizacion_Id");
        }
    }
}
