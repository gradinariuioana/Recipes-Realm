namespace Database_Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class recipemodelupdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Recipes", "Is_Active", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Recipes", "Is_Active");
        }
    }
}
