namespace TiendaOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Vhiculo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Vehiculo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Marca = c.String(),
                        Modelo = c.String(),
                        Year = c.Int(nullable: false),
                        SolicitudCotizacionId = c.Int(),
                        CotizacionId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cotizacion", t => t.CotizacionId)
                .ForeignKey("dbo.SolicitudCotizacions", t => t.SolicitudCotizacionId)
                .Index(t => t.SolicitudCotizacionId)
                .Index(t => t.CotizacionId);
            
            AddColumn("dbo.SolicitudCotizacions", "ComentarioCliente", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Vehiculo", "SolicitudCotizacionId", "dbo.SolicitudCotizacions");
            DropForeignKey("dbo.Vehiculo", "CotizacionId", "dbo.Cotizacion");
            DropIndex("dbo.Vehiculo", new[] { "CotizacionId" });
            DropIndex("dbo.Vehiculo", new[] { "SolicitudCotizacionId" });
            DropColumn("dbo.SolicitudCotizacions", "ComentarioCliente");
            DropTable("dbo.Vehiculo");
        }
    }
}
