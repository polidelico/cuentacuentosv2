namespace Cuentos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Stories", "StoryType", c => c.Int(nullable: false));
            AddColumn("dbo.Stories", "VideoUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Stories", "VideoUrl");
            DropColumn("dbo.Stories", "StoryType");
        }
    }
}
