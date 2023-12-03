namespace Database_Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editModels : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ingredients", "Category", c => c.String(maxLength: 300));
            AlterColumn("dbo.RecipeNutritionElements", "Value", c => c.Long());
            DropColumn("dbo.RecipeIngredients", "Category");
        }
        
        public override void Down()
        {
            AddColumn("dbo.RecipeIngredients", "Category", c => c.String(maxLength: 300));
            AlterColumn("dbo.RecipeNutritionElements", "Value", c => c.Int());
            DropColumn("dbo.Ingredients", "Category");
        }
    }
}
