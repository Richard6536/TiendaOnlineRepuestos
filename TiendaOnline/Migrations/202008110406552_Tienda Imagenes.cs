namespace TiendaOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TiendaImagenes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Imagen", "TiendaId", c => c.Int());
            CreateIndex("dbo.Imagen", "TiendaId");
            AddForeignKey("dbo.Imagen", "TiendaId", "dbo.Tienda", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Imagen", "TiendaId", "dbo.Tienda");
            DropIndex("dbo.Imagen", new[] { "TiendaId" });
            DropColumn("dbo.Imagen", "TiendaId");
        }
    }
}
