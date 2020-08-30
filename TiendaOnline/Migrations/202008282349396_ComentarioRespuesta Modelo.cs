namespace TiendaOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ComentarioRespuestaModelo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ComentarioRespuesta",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Mensaje = c.String(),
                        Fecha = c.DateTime(nullable: false),
                        ComentarioId = c.Int(),
                        UsuarioId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Comentario", t => t.ComentarioId)
                .ForeignKey("dbo.Usuario", t => t.UsuarioId)
                .Index(t => t.ComentarioId)
                .Index(t => t.UsuarioId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ComentarioRespuesta", "UsuarioId", "dbo.Usuario");
            DropForeignKey("dbo.ComentarioRespuesta", "ComentarioId", "dbo.Comentario");
            DropIndex("dbo.ComentarioRespuesta", new[] { "UsuarioId" });
            DropIndex("dbo.ComentarioRespuesta", new[] { "ComentarioId" });
            DropTable("dbo.ComentarioRespuesta");
        }
    }
}
