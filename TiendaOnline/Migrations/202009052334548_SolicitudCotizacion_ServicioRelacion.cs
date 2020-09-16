namespace TiendaOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SolicitudCotizacion_ServicioRelacion : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SolicitudCotizacionServicios", "SolicitudCotizacion_Id", "dbo.SolicitudCotizacions");
            DropForeignKey("dbo.SolicitudCotizacionServicios", "Servicio_Id", "dbo.Servicio");
            DropIndex("dbo.SolicitudCotizacionServicios", new[] { "SolicitudCotizacion_Id" });
            DropIndex("dbo.SolicitudCotizacionServicios", new[] { "Servicio_Id" });
            AddColumn("dbo.SolicitudCotizacions", "ServicioId", c => c.Int());
            CreateIndex("dbo.SolicitudCotizacions", "ServicioId");
            AddForeignKey("dbo.SolicitudCotizacions", "ServicioId", "dbo.Servicio", "Id");
            DropTable("dbo.SolicitudCotizacionServicios");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.SolicitudCotizacionServicios",
                c => new
                    {
                        SolicitudCotizacion_Id = c.Int(nullable: false),
                        Servicio_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.SolicitudCotizacion_Id, t.Servicio_Id });
            
            DropForeignKey("dbo.SolicitudCotizacions", "ServicioId", "dbo.Servicio");
            DropIndex("dbo.SolicitudCotizacions", new[] { "ServicioId" });
            DropColumn("dbo.SolicitudCotizacions", "ServicioId");
            CreateIndex("dbo.SolicitudCotizacionServicios", "Servicio_Id");
            CreateIndex("dbo.SolicitudCotizacionServicios", "SolicitudCotizacion_Id");
            AddForeignKey("dbo.SolicitudCotizacionServicios", "Servicio_Id", "dbo.Servicio", "Id", cascadeDelete: true);
            AddForeignKey("dbo.SolicitudCotizacionServicios", "SolicitudCotizacion_Id", "dbo.SolicitudCotizacions", "Id", cascadeDelete: true);
        }
    }
}
