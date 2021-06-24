namespace FEE.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatewebinfo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WebInfo", "Copyright", c => c.String());
            AddColumn("dbo.WebInfo", "Website", c => c.String());
            AddColumn("dbo.WebInfo", "Time", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.WebInfo", "Time");
            DropColumn("dbo.WebInfo", "Website");
            DropColumn("dbo.WebInfo", "Copyright");
        }
    }
}
