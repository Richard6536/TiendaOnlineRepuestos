namespace TiendaOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UsuarioTiendaMecanico_TiendaRelacion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UsuarioTiendaMecanico", "TiendaId", c => c.Int());
            CreateIndex("dbo.UsuarioTiendaMecanico", "TiendaId");
            AddForeignKey("dbo.UsuarioTiendaMecanico", "TiendaId", "dbo.Tienda", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UsuarioTiendaMecanico", "TiendaId", "dbo.Tienda");
            DropIndex("dbo.UsuarioTiendaMecanico", new[] { "TiendaId" });
            DropColumn("dbo.UsuarioTiendaMecanico", "TiendaId");
        }
    }
}
