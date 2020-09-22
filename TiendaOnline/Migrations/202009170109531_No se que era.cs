namespace TiendaOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Nosequeera : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Agenda",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UsuarioTiendaMecanicoId = c.Int(),
                        CalendarioId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Calendario", t => t.CalendarioId)
                .ForeignKey("dbo.UsuarioTiendaMecanico", t => t.UsuarioTiendaMecanicoId)
                .Index(t => t.UsuarioTiendaMecanicoId)
                .Index(t => t.CalendarioId);
            
            CreateTable(
                "dbo.Calendario",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Cita",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Codigo = c.String(),
                        Fecha = c.DateTime(nullable: false),
                        UsuarioTiendaMecanicoId = c.Int(),
                        ServicioId = c.Int(),
                        UsuarioId = c.Int(),
                        Agenda_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Servicio", t => t.ServicioId)
                .ForeignKey("dbo.Usuario", t => t.UsuarioId)
                .ForeignKey("dbo.UsuarioTiendaMecanico", t => t.UsuarioTiendaMecanicoId)
                .ForeignKey("dbo.Agenda", t => t.Agenda_Id)
                .Index(t => t.UsuarioTiendaMecanicoId)
                .Index(t => t.ServicioId)
                .Index(t => t.UsuarioId)
                .Index(t => t.Agenda_Id);
            
            CreateTable(
                "dbo.UsuarioTiendaMecanico",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Disponibilidad = c.Int(nullable: false),
                        UsuarioTiendaId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UsuarioTienda", t => t.UsuarioTiendaId)
                .Index(t => t.UsuarioTiendaId);
            
            CreateTable(
                "dbo.EmailConRefreshToken",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(),
                        AccessToken = c.String(),
                        RefreshToken = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Cita", "Agenda_Id", "dbo.Agenda");
            DropForeignKey("dbo.Cita", "UsuarioTiendaMecanicoId", "dbo.UsuarioTiendaMecanico");
            DropForeignKey("dbo.UsuarioTiendaMecanico", "UsuarioTiendaId", "dbo.UsuarioTienda");
            DropForeignKey("dbo.Agenda", "UsuarioTiendaMecanicoId", "dbo.UsuarioTiendaMecanico");
            DropForeignKey("dbo.Cita", "UsuarioId", "dbo.Usuario");
            DropForeignKey("dbo.Cita", "ServicioId", "dbo.Servicio");
            DropForeignKey("dbo.Agenda", "CalendarioId", "dbo.Calendario");
            DropIndex("dbo.UsuarioTiendaMecanico", new[] { "UsuarioTiendaId" });
            DropIndex("dbo.Cita", new[] { "Agenda_Id" });
            DropIndex("dbo.Cita", new[] { "UsuarioId" });
            DropIndex("dbo.Cita", new[] { "ServicioId" });
            DropIndex("dbo.Cita", new[] { "UsuarioTiendaMecanicoId" });
            DropIndex("dbo.Agenda", new[] { "CalendarioId" });
            DropIndex("dbo.Agenda", new[] { "UsuarioTiendaMecanicoId" });
            DropTable("dbo.EmailConRefreshToken");
            DropTable("dbo.UsuarioTiendaMecanico");
            DropTable("dbo.Cita");
            DropTable("dbo.Calendario");
            DropTable("dbo.Agenda");
        }
    }
}
