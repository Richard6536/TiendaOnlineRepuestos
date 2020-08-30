namespace TiendaOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tienda : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Producto", "Year", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Producto", "Year");
        }
    }
}
