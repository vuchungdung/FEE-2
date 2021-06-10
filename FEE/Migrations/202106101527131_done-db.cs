namespace FEE.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class donedb : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Posts", "Author");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Posts", "Author", c => c.String());
        }
    }
}
