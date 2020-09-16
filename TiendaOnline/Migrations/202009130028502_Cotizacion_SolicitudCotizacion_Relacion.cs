namespace TiendaOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Cotizacion_SolicitudCotizacion_Relacion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cotizacion", "SolicitudCotizacionId", c => c.Int());
            CreateIndex("dbo.Cotizacion", "SolicitudCotizacionId");
            AddForeignKey("dbo.Cotizacion", "SolicitudCotizacionId", "dbo.SolicitudCotizacions", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Cotizacion", "SolicitudCotizacionId", "dbo.SolicitudCotizacions");
            DropIndex("dbo.Cotizacion", new[] { "SolicitudCotizacionId" });
            DropColumn("dbo.Cotizacion", "SolicitudCotizacionId");
        }
    }
}
