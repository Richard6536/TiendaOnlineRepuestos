namespace TiendaOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UsuarioTiendaCambio : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UsuarioTiendaMecanico", "UsuarioTiendaId", "dbo.UsuarioTienda");
            DropIndex("dbo.UsuarioTiendaMecanico", new[] { "UsuarioTiendaId" });
            DropColumn("dbo.UsuarioTiendaMecanico", "UsuarioTiendaId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UsuarioTiendaMecanico", "UsuarioTiendaId", c => c.Int());
            CreateIndex("dbo.UsuarioTiendaMecanico", "UsuarioTiendaId");
            AddForeignKey("dbo.UsuarioTiendaMecanico", "UsuarioTiendaId", "dbo.UsuarioTienda", "Id");
        }
    }
}
