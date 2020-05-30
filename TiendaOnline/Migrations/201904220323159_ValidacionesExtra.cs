namespace TiendaOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ValidacionesExtra : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Producto", "Nombre", c => c.String(nullable: false));
            AlterColumn("dbo.Tienda", "Nombre", c => c.String(nullable: false));
            AlterColumn("dbo.Usuario", "NombreUsuario", c => c.String(nullable: false));
            AlterColumn("dbo.Usuario", "Password", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Usuario", "Password", c => c.String());
            AlterColumn("dbo.Usuario", "NombreUsuario", c => c.String());
            AlterColumn("dbo.Tienda", "Nombre", c => c.String());
            AlterColumn("dbo.Producto", "Nombre", c => c.String());
        }
    }
}
