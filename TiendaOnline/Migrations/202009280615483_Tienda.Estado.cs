namespace TiendaOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TiendaEstado : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tienda", "EstadoTienda", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tienda", "EstadoTienda");
        }
    }
}
