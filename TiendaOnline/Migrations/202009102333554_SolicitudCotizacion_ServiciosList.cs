namespace TiendaOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SolicitudCotizacion_ServiciosList : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SolicitudCotizacions", "ServicioId", "dbo.Servicio");
            DropIndex("dbo.SolicitudCotizacions", new[] { "ServicioId" });
            CreateTable(
                "dbo.SolicitudCotizacionServicios",
                c => new
                    {
                        SolicitudCotizacion_Id = c.Int(nullable: false),
                        Servicio_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SolicitudCotizacion_Id, t.Servicio_Id })
                .ForeignKey("dbo.SolicitudCotizacions", t => t.SolicitudCotizacion_Id, cascadeDelete: true)
                .ForeignKey("dbo.Servicio", t => t.Servicio_Id, cascadeDelete: true)
                .Index(t => t.SolicitudCotizacion_Id)
                .Index(t => t.Servicio_Id);
            
            DropColumn("dbo.SolicitudCotizacions", "ServicioId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SolicitudCotizacions", "ServicioId", c => c.Int());
            DropForeignKey("dbo.SolicitudCotizacionServicios", "Servicio_Id", "dbo.Servicio");
            DropForeignKey("dbo.SolicitudCotizacionServicios", "SolicitudCotizacion_Id", "dbo.SolicitudCotizacions");
            DropIndex("dbo.SolicitudCotizacionServicios", new[] { "Servicio_Id" });
            DropIndex("dbo.SolicitudCotizacionServicios", new[] { "SolicitudCotizacion_Id" });
            DropTable("dbo.SolicitudCotizacionServicios");
            CreateIndex("dbo.SolicitudCotizacions", "ServicioId");
            AddForeignKey("dbo.SolicitudCotizacions", "ServicioId", "dbo.Servicio", "Id");
        }
    }
}
