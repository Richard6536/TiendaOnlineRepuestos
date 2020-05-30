namespace TiendaOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ValidacionEmail : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Usuario", "Email", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Usuario", "Email", c => c.String());
        }
    }
}
