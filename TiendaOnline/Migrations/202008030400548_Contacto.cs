namespace TiendaOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Contacto : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tienda", "Correo", c => c.String());
            AddColumn("dbo.Tienda", "Telefono", c => c.Int(nullable: false));
            AddColumn("dbo.Tienda", "Local", c => c.String());
            AddColumn("dbo.Tienda", "Direccion", c => c.String());
            AddColumn("dbo.Tienda", "Latitud", c => c.Double(nullable: false));
            AddColumn("dbo.Tienda", "Longitud", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tienda", "Longitud");
            DropColumn("dbo.Tienda", "Latitud");
            DropColumn("dbo.Tienda", "Direccion");
            DropColumn("dbo.Tienda", "Local");
            DropColumn("dbo.Tienda", "Telefono");
            DropColumn("dbo.Tienda", "Correo");
        }
    }
}
