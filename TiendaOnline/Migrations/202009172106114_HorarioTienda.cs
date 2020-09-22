namespace TiendaOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HorarioTienda : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HorarioTienda",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Dia = c.Int(nullable: false),
                        Estado = c.Int(nullable: false),
                        HoraApertura = c.DateTime(nullable: false),
                        HoraCierre = c.DateTime(nullable: false),
                        HoraDescansoInicio = c.DateTime(nullable: false),
                        HoraDescansoTermino = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tienda", t => t.Id)
                .Index(t => t.Id);
            
            AddColumn("dbo.UsuarioTiendaMecanico", "HoraEntrada", c => c.DateTime(nullable: false));
            AddColumn("dbo.UsuarioTiendaMecanico", "HoraSalida", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.HorarioTienda", "Id", "dbo.Tienda");
            DropIndex("dbo.HorarioTienda", new[] { "Id" });
            DropColumn("dbo.UsuarioTiendaMecanico", "HoraSalida");
            DropColumn("dbo.UsuarioTiendaMecanico", "HoraEntrada");
            DropTable("dbo.HorarioTienda");
        }
    }
}
