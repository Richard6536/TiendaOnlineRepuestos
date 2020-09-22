namespace TiendaOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class USTM_CotizacionRelationship : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Cotizacion", "UsuarioTiendaMecanicoId", c => c.Int());
            CreateIndex("dbo.Cotizacion", "UsuarioTiendaMecanicoId");
            AddForeignKey("dbo.Cotizacion", "UsuarioTiendaMecanicoId", "dbo.UsuarioTiendaMecanico", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Cotizacion", "UsuarioTiendaMecanicoId", "dbo.UsuarioTiendaMecanico");
            DropIndex("dbo.Cotizacion", new[] { "UsuarioTiendaMecanicoId" });
            DropColumn("dbo.Cotizacion", "UsuarioTiendaMecanicoId");
        }
    }
}
