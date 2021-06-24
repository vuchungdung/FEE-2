namespace FEE.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatewebinfoaddfieldnote : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.WebInfo", "Note", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.WebInfo", "Note");
        }
    }
}
