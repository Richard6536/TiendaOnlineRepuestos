namespace TiendaOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ServicioCotizadocampoNombre : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ServicioCotizado", "Nombre", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ServicioCotizado", "Nombre");
        }
    }
}
