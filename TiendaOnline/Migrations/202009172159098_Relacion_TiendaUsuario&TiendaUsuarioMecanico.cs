namespace TiendaOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Relacion_TiendaUsuarioTiendaUsuarioMecanico : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Agenda", "UsuarioTiendaMecanicoId", "dbo.UsuarioTiendaMecanico");
            DropForeignKey("dbo.Cita", "UsuarioTiendaMecanicoId", "dbo.UsuarioTiendaMecanico");
            DropPrimaryKey("dbo.UsuarioTiendaMecanico");
            AlterColumn("dbo.UsuarioTiendaMecanico", "Id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.UsuarioTiendaMecanico", "Id");
            CreateIndex("dbo.UsuarioTiendaMecanico", "Id");
            AddForeignKey("dbo.UsuarioTiendaMecanico", "Id", "dbo.UsuarioTienda", "Id");
            AddForeignKey("dbo.Agenda", "UsuarioTiendaMecanicoId", "dbo.UsuarioTiendaMecanico", "Id");
            AddForeignKey("dbo.Cita", "UsuarioTiendaMecanicoId", "dbo.UsuarioTiendaMecanico", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Cita", "UsuarioTiendaMecanicoId", "dbo.UsuarioTiendaMecanico");
            DropForeignKey("dbo.Agenda", "UsuarioTiendaMecanicoId", "dbo.UsuarioTiendaMecanico");
            DropForeignKey("dbo.UsuarioTiendaMecanico", "Id", "dbo.UsuarioTienda");
            DropIndex("dbo.UsuarioTiendaMecanico", new[] { "Id" });
            DropPrimaryKey("dbo.UsuarioTiendaMecanico");
            AlterColumn("dbo.UsuarioTiendaMecanico", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.UsuarioTiendaMecanico", "Id");
            AddForeignKey("dbo.Cita", "UsuarioTiendaMecanicoId", "dbo.UsuarioTiendaMecanico", "Id");
            AddForeignKey("dbo.Agenda", "UsuarioTiendaMecanicoId", "dbo.UsuarioTiendaMecanico", "Id");
        }
    }
}
