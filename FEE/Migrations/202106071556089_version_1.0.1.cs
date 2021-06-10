namespace FEE.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class version_101 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DepartmentInMenus",
                c => new
                    {
                        DepartmentId = c.Int(nullable: false),
                        MenuId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DepartmentId, t.MenuId });
            
            AddColumn("dbo.Partners", "Url", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Partners", "Url");
            DropTable("dbo.DepartmentInMenus");
        }
    }
}
