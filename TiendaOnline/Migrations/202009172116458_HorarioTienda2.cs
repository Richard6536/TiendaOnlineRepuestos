namespace TiendaOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HorarioTienda2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HorarioTienda", "HorarioDescanso", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.HorarioTienda", "HorarioDescanso");
        }
    }
}
