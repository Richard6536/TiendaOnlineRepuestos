namespace TiendaOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Cotizacion_AddManual : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cotizacion", "Manual", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Cotizacion", "Manual");
        }
    }
}
