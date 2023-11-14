namespace Database_Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editIngredient : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.RecipeIngredients", "Quantity", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.RecipeIngredients", "Quantity", c => c.Int(nullable: false));
        }
    }
}
