namespace TiendaOnline.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ImageHeaderandImageProfile : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tienda", "ImageHeader_Id", c => c.Int());
            AddColumn("dbo.Tienda", "ImageProfile_Id", c => c.Int());
            CreateIndex("dbo.Tienda", "ImageHeader_Id");
            CreateIndex("dbo.Tienda", "ImageProfile_Id");
            AddForeignKey("dbo.Tienda", "ImageHeader_Id", "dbo.Imagen", "Id");
            AddForeignKey("dbo.Tienda", "ImageProfile_Id", "dbo.Imagen", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tienda", "ImageProfile_Id", "dbo.Imagen");
            DropForeignKey("dbo.Tienda", "ImageHeader_Id", "dbo.Imagen");
            DropIndex("dbo.Tienda", new[] { "ImageProfile_Id" });
            DropIndex("dbo.Tienda", new[] { "ImageHeader_Id" });
            DropColumn("dbo.Tienda", "ImageProfile_Id");
            DropColumn("dbo.Tienda", "ImageHeader_Id");
        }
    }
}
