namespace TiendaOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Email : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cotizacion", "Referencia", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Cotizacion", "Referencia");
        }
    }
}
