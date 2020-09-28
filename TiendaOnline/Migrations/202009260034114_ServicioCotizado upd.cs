namespace TiendaOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ServicioCotizadoupd : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ServicioCotizado", "ServicioId", "dbo.Servicio");
            DropIndex("dbo.ServicioCotizado", new[] { "ServicioId" });
            RenameColumn(table: "dbo.ServicioCotizado", name: "ServicioId", newName: "Servicio_Id");
            AlterColumn("dbo.ServicioCotizado", "Servicio_Id", c => c.Int());
            CreateIndex("dbo.ServicioCotizado", "Servicio_Id");
            AddForeignKey("dbo.ServicioCotizado", "Servicio_Id", "dbo.Servicio", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ServicioCotizado", "Servicio_Id", "dbo.Servicio");
            DropIndex("dbo.ServicioCotizado", new[] { "Servicio_Id" });
            AlterColumn("dbo.ServicioCotizado", "Servicio_Id", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.ServicioCotizado", name: "Servicio_Id", newName: "ServicioId");
            CreateIndex("dbo.ServicioCotizado", "ServicioId");
            AddForeignKey("dbo.ServicioCotizado", "ServicioId", "dbo.Servicio", "Id", cascadeDelete: true);
        }
    }
}
