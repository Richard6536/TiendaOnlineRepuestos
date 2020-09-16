namespace TiendaOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SolicitudCotizacion_AddFecha : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Servicio", "SolicitudCotizacion_Id", "dbo.SolicitudCotizacions");
            DropIndex("dbo.Servicio", new[] { "SolicitudCotizacion_Id" });
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
            
            AddColumn("dbo.SolicitudCotizacions", "Codigo", c => c.String());
            AddColumn("dbo.SolicitudCotizacions", "Fecha", c => c.DateTime(nullable: false));
            DropColumn("dbo.Servicio", "SolicitudCotizacion_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Servicio", "SolicitudCotizacion_Id", c => c.Int());
            DropForeignKey("dbo.SolicitudCotizacionServicios", "Servicio_Id", "dbo.Servicio");
            DropForeignKey("dbo.SolicitudCotizacionServicios", "SolicitudCotizacion_Id", "dbo.SolicitudCotizacions");
            DropIndex("dbo.SolicitudCotizacionServicios", new[] { "Servicio_Id" });
            DropIndex("dbo.SolicitudCotizacionServicios", new[] { "SolicitudCotizacion_Id" });
            DropColumn("dbo.SolicitudCotizacions", "Fecha");
            DropColumn("dbo.SolicitudCotizacions", "Codigo");
            DropTable("dbo.SolicitudCotizacionServicios");
            CreateIndex("dbo.Servicio", "SolicitudCotizacion_Id");
            AddForeignKey("dbo.Servicio", "SolicitudCotizacion_Id", "dbo.SolicitudCotizacions", "Id");
        }
    }
}
