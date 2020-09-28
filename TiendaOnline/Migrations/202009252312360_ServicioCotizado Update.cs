namespace TiendaOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ServicioCotizadoUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ServicioCotizado", "EsManual", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ServicioCotizado", "EsManual");
        }
    }
}
