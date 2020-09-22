namespace TiendaOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CitaCambios : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Cita", "ServicioId", "dbo.Servicio");
            DropIndex("dbo.Cita", new[] { "ServicioId" });
            AddColumn("dbo.Cita", "FechaInicio", c => c.DateTime(nullable: false));
            AddColumn("dbo.Cita", "FechaTermino", c => c.DateTime(nullable: false));
            AddColumn("dbo.Cita", "CotizacionId", c => c.Int());
            CreateIndex("dbo.Cita", "CotizacionId");
            AddForeignKey("dbo.Cita", "CotizacionId", "dbo.Cotizacion", "Id");
            DropColumn("dbo.Cita", "Fecha");
            DropColumn("dbo.Cita", "ServicioId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Cita", "ServicioId", c => c.Int());
            AddColumn("dbo.Cita", "Fecha", c => c.DateTime(nullable: false));
            DropForeignKey("dbo.Cita", "CotizacionId", "dbo.Cotizacion");
            DropIndex("dbo.Cita", new[] { "CotizacionId" });
            DropColumn("dbo.Cita", "CotizacionId");
            DropColumn("dbo.Cita", "FechaTermino");
            DropColumn("dbo.Cita", "FechaInicio");
            CreateIndex("dbo.Cita", "ServicioId");
            AddForeignKey("dbo.Cita", "ServicioId", "dbo.Servicio", "Id");
        }
    }
}
