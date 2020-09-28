namespace TiendaOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ServicioTemporal : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ServicioCotizado", "Categoria_Id", c => c.Int());
            CreateIndex("dbo.ServicioCotizado", "Categoria_Id");
            AddForeignKey("dbo.ServicioCotizado", "Categoria_Id", "dbo.Categoria", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ServicioCotizado", "Categoria_Id", "dbo.Categoria");
            DropIndex("dbo.ServicioCotizado", new[] { "Categoria_Id" });
            DropColumn("dbo.ServicioCotizado", "Categoria_Id");
        }
    }
}
