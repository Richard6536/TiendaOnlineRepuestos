namespace TiendaOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class USTM2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UsuarioTiendaMecanico", "Id", "dbo.UsuarioTienda");
            DropForeignKey("dbo.Agenda", "UsuarioTiendaMecanicoId", "dbo.UsuarioTiendaMecanico");
            DropForeignKey("dbo.HorarioTrabajador", "UsuarioTiendaMecanicoId", "dbo.UsuarioTiendaMecanico");
            DropForeignKey("dbo.Cita", "UsuarioTiendaMecanicoId", "dbo.UsuarioTiendaMecanico");
            DropIndex("dbo.UsuarioTiendaMecanico", new[] { "Id" });
            DropPrimaryKey("dbo.UsuarioTiendaMecanico");
            AddColumn("dbo.UsuarioTiendaMecanico", "UsuarioTienda_Id", c => c.Int());
            AlterColumn("dbo.UsuarioTiendaMecanico", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.UsuarioTiendaMecanico", "Id");
            CreateIndex("dbo.UsuarioTiendaMecanico", "UsuarioTienda_Id");
            AddForeignKey("dbo.UsuarioTiendaMecanico", "UsuarioTienda_Id", "dbo.UsuarioTienda", "Id");
            AddForeignKey("dbo.Agenda", "UsuarioTiendaMecanicoId", "dbo.UsuarioTiendaMecanico", "Id");
            AddForeignKey("dbo.HorarioTrabajador", "UsuarioTiendaMecanicoId", "dbo.UsuarioTiendaMecanico", "Id");
            AddForeignKey("dbo.Cita", "UsuarioTiendaMecanicoId", "dbo.UsuarioTiendaMecanico", "Id");
            DropColumn("dbo.UsuarioTiendaMecanico", "UsuarioTiendaId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UsuarioTiendaMecanico", "UsuarioTiendaId", c => c.Int());
            DropForeignKey("dbo.Cita", "UsuarioTiendaMecanicoId", "dbo.UsuarioTiendaMecanico");
            DropForeignKey("dbo.HorarioTrabajador", "UsuarioTiendaMecanicoId", "dbo.UsuarioTiendaMecanico");
            DropForeignKey("dbo.Agenda", "UsuarioTiendaMecanicoId", "dbo.UsuarioTiendaMecanico");
            DropForeignKey("dbo.UsuarioTiendaMecanico", "UsuarioTienda_Id", "dbo.UsuarioTienda");
            DropIndex("dbo.UsuarioTiendaMecanico", new[] { "UsuarioTienda_Id" });
            DropPrimaryKey("dbo.UsuarioTiendaMecanico");
            AlterColumn("dbo.UsuarioTiendaMecanico", "Id", c => c.Int(nullable: false));
            DropColumn("dbo.UsuarioTiendaMecanico", "UsuarioTienda_Id");
            AddPrimaryKey("dbo.UsuarioTiendaMecanico", "Id");
            CreateIndex("dbo.UsuarioTiendaMecanico", "Id");
            AddForeignKey("dbo.Cita", "UsuarioTiendaMecanicoId", "dbo.UsuarioTiendaMecanico", "Id");
            AddForeignKey("dbo.HorarioTrabajador", "UsuarioTiendaMecanicoId", "dbo.UsuarioTiendaMecanico", "Id");
            AddForeignKey("dbo.Agenda", "UsuarioTiendaMecanicoId", "dbo.UsuarioTiendaMecanico", "Id");
            AddForeignKey("dbo.UsuarioTiendaMecanico", "Id", "dbo.UsuarioTienda", "Id");
        }
    }
}
