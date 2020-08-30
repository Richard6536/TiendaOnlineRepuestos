namespace TiendaOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nuevoscamposproducto : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Producto", "Modelo", c => c.String());
            AddColumn("dbo.Producto", "Marca", c => c.String());
            AddColumn("dbo.Producto", "EsGenerico", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Producto", "EsGenerico");
            DropColumn("dbo.Producto", "Marca");
            DropColumn("dbo.Producto", "Modelo");
        }
    }
}
