namespace GooglePlus.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Create_Activities : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Activity",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        created = c.DateTime(nullable: false),
                        Author_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.Author_Id)
                .Index(t => t.Author_Id);
            
            CreateTable(
                "dbo.Photo",
                c => new
                    {
                        Id = c.Long(nullable: false),
                        src = c.String(),
                        comment = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activity", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Share",
                c => new
                    {
                        Id = c.Long(nullable: false),
                        content = c.String(),
                        comment = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activity", t => t.Id)
                .Index(t => t.Id);
            
            CreateTable(
                "dbo.Post",
                c => new
                    {
                        Id = c.Long(nullable: false),
                        text = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Activity", t => t.Id)
                .Index(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Post", new[] { "Id" });
            DropIndex("dbo.Share", new[] { "Id" });
            DropIndex("dbo.Photo", new[] { "Id" });
            DropIndex("dbo.Activity", new[] { "Author_Id" });
            DropForeignKey("dbo.Post", "Id", "dbo.Activity");
            DropForeignKey("dbo.Share", "Id", "dbo.Activity");
            DropForeignKey("dbo.Photo", "Id", "dbo.Activity");
            DropForeignKey("dbo.Activity", "Author_Id", "dbo.Users");
            DropTable("dbo.Post");
            DropTable("dbo.Share");
            DropTable("dbo.Photo");
            DropTable("dbo.Activity");
        }
    }
}
