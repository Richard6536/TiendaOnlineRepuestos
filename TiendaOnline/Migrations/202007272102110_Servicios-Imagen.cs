namespace TiendaOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ServiciosImagen : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Imagen", "TipoImagen", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Imagen", "TipoImagen");
        }
    }
}
