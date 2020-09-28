namespace TiendaOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ServicioSolicitadoCotizacion : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SolicitudCotizacionServicios", "SolicitudCotizacion_Id", "dbo.SolicitudCotizacions");
            DropForeignKey("dbo.SolicitudCotizacionServicios", "Servicio_Id", "dbo.Servicio");
            DropIndex("dbo.SolicitudCotizacionServicios", new[] { "SolicitudCotizacion_Id" });
            DropIndex("dbo.SolicitudCotizacionServicios", new[] { "Servicio_Id" });
            CreateTable(
                "dbo.ServicioSolicitadoCotizacion",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Cantidad = c.Int(nullable: false),
                        Valor = c.Single(nullable: false),
                        ValorTotal = c.Single(nullable: false),
                        Comentario = c.String(),
                        ServicioId = c.Int(nullable: false),
                        SolicitudCotizacion_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Servicio", t => t.ServicioId, cascadeDelete: true)
                .ForeignKey("dbo.SolicitudCotizacions", t => t.SolicitudCotizacion_Id)
                .Index(t => t.ServicioId)
                .Index(t => t.SolicitudCotizacion_Id);
            
            AddColumn("dbo.SolicitudCotizacions", "Servicio_Id", c => c.Int());
            CreateIndex("dbo.SolicitudCotizacions", "Servicio_Id");
            AddForeignKey("dbo.SolicitudCotizacions", "Servicio_Id", "dbo.Servicio", "Id");
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
            
            DropForeignKey("dbo.SolicitudCotizacions", "Servicio_Id", "dbo.Servicio");
            DropForeignKey("dbo.ServicioSolicitadoCotizacion", "SolicitudCotizacion_Id", "dbo.SolicitudCotizacions");
            DropForeignKey("dbo.ServicioSolicitadoCotizacion", "ServicioId", "dbo.Servicio");
            DropIndex("dbo.SolicitudCotizacions", new[] { "Servicio_Id" });
            DropIndex("dbo.ServicioSolicitadoCotizacion", new[] { "SolicitudCotizacion_Id" });
            DropIndex("dbo.ServicioSolicitadoCotizacion", new[] { "ServicioId" });
            DropColumn("dbo.SolicitudCotizacions", "Servicio_Id");
            DropTable("dbo.ServicioSolicitadoCotizacion");
            CreateIndex("dbo.SolicitudCotizacionServicios", "Servicio_Id");
            CreateIndex("dbo.SolicitudCotizacionServicios", "SolicitudCotizacion_Id");
            AddForeignKey("dbo.SolicitudCotizacionServicios", "Servicio_Id", "dbo.Servicio", "Id", cascadeDelete: true);
            AddForeignKey("dbo.SolicitudCotizacionServicios", "SolicitudCotizacion_Id", "dbo.SolicitudCotizacions", "Id", cascadeDelete: true);
        }
    }
}
