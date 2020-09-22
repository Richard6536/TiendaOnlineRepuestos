namespace TiendaOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HorarioDescanso_Mecanico : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UsuarioTiendaMecanico", "HoraDescansoInicio", c => c.DateTime(nullable: false));
            AddColumn("dbo.UsuarioTiendaMecanico", "HoraDescansoTermino", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UsuarioTiendaMecanico", "HoraDescansoTermino");
            DropColumn("dbo.UsuarioTiendaMecanico", "HoraDescansoInicio");
        }
    }
}
