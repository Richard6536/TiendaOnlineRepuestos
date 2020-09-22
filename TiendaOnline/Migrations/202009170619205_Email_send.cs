namespace TiendaOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Email_send : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RegistroEnvio",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Fecha = c.DateTime(nullable: false),
                        MedioDeContacto = c.String(),
                        Remitente = c.String(),
                        Destinatario = c.String(),
                        Notas = c.String(),
                        GeneradoAutomaticamente = c.Boolean(nullable: false),
                        CotizacionId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cotizacion", t => t.CotizacionId)
                .Index(t => t.CotizacionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RegistroEnvio", "CotizacionId", "dbo.Cotizacion");
            DropIndex("dbo.RegistroEnvio", new[] { "CotizacionId" });
            DropTable("dbo.RegistroEnvio");
        }
    }
}
