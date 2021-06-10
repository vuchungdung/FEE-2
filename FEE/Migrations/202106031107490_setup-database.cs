namespace FEE.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class setupdatabase : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 256),
                        Status = c.Boolean(nullable: false),
                        CreateBy = c.Int(),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                        UpdateBy = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CategoryId)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.CommandInFunctions",
                c => new
                    {
                        CommandId = c.String(nullable: false, maxLength: 128),
                        FunctionId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.CommandId, t.FunctionId });
            
            CreateTable(
                "dbo.Commands",
                c => new
                    {
                        CommandId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.CommandId)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.Contacts",
                c => new
                    {
                        ContactId = c.Int(nullable: false, identity: true),
                        Content = c.String(),
                        Name = c.String(),
                        Email = c.String(),
                        Phone = c.String(),
                        Status = c.Boolean(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        ReplyDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ContactId);
            
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        DepartmentId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.DepartmentId);
            
            CreateTable(
                "dbo.Functions",
                c => new
                    {
                        FunctionId = c.String(nullable: false, maxLength: 128),
                        Name = c.String(maxLength: 100),
                        Url = c.String(),
                        SortOrder = c.Int(),
                        ParentId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.FunctionId)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        ImgId = c.Int(nullable: false, identity: true),
                        Img = c.String(),
                        Status = c.Boolean(nullable: false),
                        CreateBy = c.Int(),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                        UpdateBy = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ImgId);
            
            CreateTable(
                "dbo.Menus",
                c => new
                    {
                        MenuId = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 100),
                        URL = c.String(),
                        DisplayOrder = c.Int(),
                        Status = c.Boolean(nullable: false),
                        ParentId = c.Int(),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.MenuId);
            
            CreateTable(
                "dbo.Partners",
                c => new
                    {
                        PartId = c.Int(nullable: false, identity: true),
                        Img = c.String(),
                        Status = c.Boolean(nullable: false),
                        CreateBy = c.Int(),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                        UpdateBy = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PartId);
            
            CreateTable(
                "dbo.Permissions",
                c => new
                    {
                        FunctionId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.Int(nullable: false),
                        CommandId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.FunctionId, t.RoleId, t.CommandId });
            
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        PostId = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 256),
                        Alias = c.String(maxLength: 256),
                        CategoryId = c.Int(nullable: false),
                        MenuId = c.Int(nullable: false),
                        Description = c.String(),
                        Content = c.String(),
                        HomeFlag = c.Boolean(nullable: false),
                        HotFlag = c.Boolean(nullable: false),
                        Img = c.String(),
                        Status = c.Int(nullable: false),
                        DepartmentId = c.Int(nullable: false),
                        IsShow = c.Boolean(nullable: false),
                        ViewCount = c.Int(nullable: false),
                        LikeCount = c.Int(nullable: false),
                        Tag = c.String(),
                        Author = c.String(),
                        CreateBy = c.Int(),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                        UpdateBy = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PostId)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        RoleId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.RoleId);
            
            CreateTable(
                "dbo.Slides",
                c => new
                    {
                        SlideId = c.Int(nullable: false, identity: true),
                        Img = c.String(),
                        Status = c.Boolean(nullable: false),
                        CreateBy = c.Int(),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                        UpdateBy = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.SlideId);
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Email = c.String(maxLength: 256, unicode: false),
                        Phone = c.String(maxLength: 20, unicode: false),
                        DepartmentId = c.Int(nullable: false),
                        Username = c.String(maxLength: 250, unicode: false),
                        Password = c.String(),
                        Status = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                        CreateBy = c.Int(),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                        UpdateBy = c.Int(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Email, unique: true)
                .Index(t => t.Phone, unique: true)
                .Index(t => t.Username, unique: true);
            
            CreateTable(
                "dbo.WebInfo",
                c => new
                    {
                        WebInfoId = c.Int(nullable: false, identity: true),
                        Logo = c.String(),
                        Email = c.String(),
                        Phone = c.String(),
                        Address = c.String(),
                        Facebook = c.String(),
                        Youtube = c.String(),
                        Zalo = c.String(),
                    })
                .PrimaryKey(t => t.WebInfoId);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Users", new[] { "Username" });
            DropIndex("dbo.Users", new[] { "Phone" });
            DropIndex("dbo.Users", new[] { "Email" });
            DropIndex("dbo.Posts", new[] { "Name" });
            DropIndex("dbo.Functions", new[] { "Name" });
            DropIndex("dbo.Commands", new[] { "Name" });
            DropIndex("dbo.Categories", new[] { "Name" });
            DropTable("dbo.WebInfo");
            DropTable("dbo.Users");
            DropTable("dbo.Tags");
            DropTable("dbo.Slides");
            DropTable("dbo.Roles");
            DropTable("dbo.Posts");
            DropTable("dbo.Permissions");
            DropTable("dbo.Partners");
            DropTable("dbo.Menus");
            DropTable("dbo.Images");
            DropTable("dbo.Functions");
            DropTable("dbo.Departments");
            DropTable("dbo.Contacts");
            DropTable("dbo.Commands");
            DropTable("dbo.CommandInFunctions");
            DropTable("dbo.Categories");
        }
    }
}
