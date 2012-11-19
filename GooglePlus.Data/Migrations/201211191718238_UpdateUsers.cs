namespace GooglePlus.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateUsers : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Activity", "Author_Id", "dbo.Users");
            DropIndex("dbo.Activity", new[] { "Author_Id" });
            AddColumn("dbo.Users", "birthday", c => c.DateTime());
            AddColumn("dbo.Users", "location", c => c.String(maxLength: 255));
            AddColumn("dbo.Users", "education", c => c.String());
            DropPrimaryKey("dbo.Users", "PK_dbo.Users");
            AlterColumn("dbo.Users", "Id", c => c.Int());
            AlterColumn("dbo.Activity", "Author_Id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Users", "Id", "PK_dbo.Users");
            AddForeignKey("dbo.Activity", "Author_Id", "dbo.Users", "Id", cascadeDelete: true);
            CreateIndex("dbo.Activity", "Author_Id");
            DropColumn("dbo.Users", "password");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "password", c => c.String());
            DropIndex("dbo.Activity", new[] { "Author_Id" });
            DropForeignKey("dbo.Activity", "Author_Id", "dbo.Users");
            DropPrimaryKey("dbo.Users", "PK_dbo.Users");
            AlterColumn("dbo.Users", "Id", c => c.Long());
            AlterColumn("dbo.Activity", "Author_Id", c => c.Long());
            AddPrimaryKey("dbo.Users", "Id", "PK_dbo.Users");
            DropColumn("dbo.Users", "education");
            DropColumn("dbo.Users", "location");
            DropColumn("dbo.Users", "birthday");
            CreateIndex("dbo.Activity", "Author_Id");
            AddForeignKey("dbo.Activity", "Author_Id", "dbo.Users", "Id");
        }
    }
}
