namespace TiendaOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Datos_Usuario : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Producto", "Codigo", c => c.String());
            AddColumn("dbo.Usuario", "Rut", c => c.String());
            AddColumn("dbo.Usuario", "Domicilio", c => c.String());
            AddColumn("dbo.Usuario", "Ciudad", c => c.String());
            AddColumn("dbo.Usuario", "Pais", c => c.String());
            AddColumn("dbo.Usuario", "Telefono", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Usuario", "Telefono");
            DropColumn("dbo.Usuario", "Pais");
            DropColumn("dbo.Usuario", "Ciudad");
            DropColumn("dbo.Usuario", "Domicilio");
            DropColumn("dbo.Usuario", "Rut");
            DropColumn("dbo.Producto", "Codigo");
        }
    }
}
