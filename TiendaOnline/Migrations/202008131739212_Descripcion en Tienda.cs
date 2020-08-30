namespace TiendaOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DescripcionenTienda : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tienda", "Descripcion", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tienda", "Descripcion");
        }
    }
}
