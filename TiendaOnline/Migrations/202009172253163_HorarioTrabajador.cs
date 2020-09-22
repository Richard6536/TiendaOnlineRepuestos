namespace TiendaOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HorarioTrabajador : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.HorarioTienda", "Id", "dbo.Tienda");
            DropIndex("dbo.HorarioTienda", new[] { "Id" });
            DropPrimaryKey("dbo.HorarioTienda");
            CreateTable(
                "dbo.HorarioTrabajador",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Dia = c.Int(nullable: false),
                        HoraEntrada = c.DateTime(nullable: false),
                        HoraSalida = c.DateTime(nullable: false),
                        HoraDescansoInicio = c.DateTime(nullable: false),
                        HoraDescansoTermino = c.DateTime(nullable: false),
                        HorarioDescanso = c.Boolean(nullable: false),
                        UsuarioTiendaMecanicoId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UsuarioTiendaMecanico", t => t.UsuarioTiendaMecanicoId)
                .Index(t => t.UsuarioTiendaMecanicoId);
            
            AddColumn("dbo.HorarioTienda", "TiendaId", c => c.Int());
            AlterColumn("dbo.HorarioTienda", "Id", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.HorarioTienda", "Id");
            CreateIndex("dbo.HorarioTienda", "TiendaId");
            AddForeignKey("dbo.HorarioTienda", "TiendaId", "dbo.Tienda", "Id");
            DropColumn("dbo.UsuarioTiendaMecanico", "HoraEntrada");
            DropColumn("dbo.UsuarioTiendaMecanico", "HoraSalida");
            DropColumn("dbo.UsuarioTiendaMecanico", "HoraDescansoInicio");
            DropColumn("dbo.UsuarioTiendaMecanico", "HoraDescansoTermino");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UsuarioTiendaMecanico", "HoraDescansoTermino", c => c.DateTime(nullable: false));
            AddColumn("dbo.UsuarioTiendaMecanico", "HoraDescansoInicio", c => c.DateTime(nullable: false));
            AddColumn("dbo.UsuarioTiendaMecanico", "HoraSalida", c => c.DateTime(nullable: false));
            AddColumn("dbo.UsuarioTiendaMecanico", "HoraEntrada", c => c.DateTime(nullable: false));
            DropForeignKey("dbo.HorarioTienda", "TiendaId", "dbo.Tienda");
            DropForeignKey("dbo.HorarioTrabajador", "UsuarioTiendaMecanicoId", "dbo.UsuarioTiendaMecanico");
            DropIndex("dbo.HorarioTrabajador", new[] { "UsuarioTiendaMecanicoId" });
            DropIndex("dbo.HorarioTienda", new[] { "TiendaId" });
            DropPrimaryKey("dbo.HorarioTienda");
            AlterColumn("dbo.HorarioTienda", "Id", c => c.Int(nullable: false));
            DropColumn("dbo.HorarioTienda", "TiendaId");
            DropTable("dbo.HorarioTrabajador");
            AddPrimaryKey("dbo.HorarioTienda", "Id");
            CreateIndex("dbo.HorarioTienda", "Id");
            AddForeignKey("dbo.HorarioTienda", "Id", "dbo.Tienda", "Id");
        }
    }
}
