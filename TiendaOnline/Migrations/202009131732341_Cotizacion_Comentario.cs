namespace TiendaOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Cotizacion_Comentario : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cotizacion", "Comentario", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Cotizacion", "Comentario");
        }
    }
}
