namespace FEE.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedb : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Menus", "Link", c => c.String());
            AlterColumn("dbo.Menus", "DisplayOrder", c => c.Int(nullable: false));
            AlterColumn("dbo.Menus", "ParentId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Menus", "ParentId", c => c.Int());
            AlterColumn("dbo.Menus", "DisplayOrder", c => c.Int());
            DropColumn("dbo.Menus", "Link");
        }
    }
}
