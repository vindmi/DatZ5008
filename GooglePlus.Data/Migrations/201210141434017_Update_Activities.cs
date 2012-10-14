namespace GooglePlus.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update_Activities : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Activity", "google_id", c => c.String());
            AddColumn("dbo.Photo", "url", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Photo", "url");
            DropColumn("dbo.Activity", "google_id");
        }
    }
}
