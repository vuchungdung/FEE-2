﻿namespace FEE.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class version_100 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Partners", "Url", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Partners", "Url");
        }
    }
}
