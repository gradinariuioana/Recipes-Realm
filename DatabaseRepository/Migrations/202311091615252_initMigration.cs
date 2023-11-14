namespace Database_Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Category_ID = c.Long(nullable: false, identity: true),
                        Category_Name = c.String(nullable: false, maxLength: 200),
                        Category_Description = c.String(),
                    })
                .PrimaryKey(t => t.Category_ID);
            
            CreateTable(
                "dbo.RecipeCategories",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        Category_ID = c.Long(nullable: false),
                        Recipe_ID = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Categories", t => t.Category_ID)
                .ForeignKey("dbo.Recipes", t => t.Recipe_ID)
                .Index(t => t.Category_ID)
                .Index(t => t.Recipe_ID);
            
            CreateTable(
                "dbo.Recipes",
                c => new
                    {
                        Recipe_ID = c.Long(nullable: false, identity: true),
                        Recipe_Name = c.String(nullable: false, maxLength: 500),
                        Recipe_Description = c.String(),
                        Cooking_Time = c.String(maxLength: 50),
                        Difficulty_Level = c.Int(),
                        Creation_Date = c.DateTime(nullable: false),
                        Picture_Path = c.String(),
                        Servings = c.Int(),
                        Author_User_ID = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Recipe_ID)
                .ForeignKey("dbo.Users", t => t.Author_User_ID)
                .Index(t => t.Author_User_ID);
            
            CreateTable(
                "dbo.Ratings",
                c => new
                    {
                        Rating_ID = c.Long(nullable: false, identity: true),
                        Rating_Value = c.Single(nullable: false),
                        Rating_Date = c.DateTime(nullable: false),
                        User_ID = c.Long(nullable: false),
                        Recipe_ID = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Rating_ID)
                .ForeignKey("dbo.Recipes", t => t.Recipe_ID)
                .ForeignKey("dbo.Users", t => t.User_ID)
                .Index(t => t.User_ID)
                .Index(t => t.Recipe_ID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        User_ID = c.Long(nullable: false, identity: true),
                        User_Name = c.String(nullable: false, maxLength: 300),
                        Password = c.Binary(nullable: false, maxLength: 32),
                        Email_Address = c.String(nullable: false, maxLength: 300),
                        Profile_Picture_Path = c.String(),
                        Registration_Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.User_ID);
            
            CreateTable(
                "dbo.Reviews",
                c => new
                    {
                        Review_ID = c.Long(nullable: false, identity: true),
                        Review_Text = c.String(),
                        Review_Date = c.DateTime(nullable: false),
                        User_ID = c.Long(nullable: false),
                        Recipe_ID = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Review_ID)
                .ForeignKey("dbo.Recipes", t => t.Recipe_ID)
                .ForeignKey("dbo.Users", t => t.User_ID)
                .Index(t => t.User_ID)
                .Index(t => t.Recipe_ID);
            
            CreateTable(
                "dbo.SavedRecipes",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        Saving_Date = c.DateTime(nullable: false),
                        User_ID = c.Long(nullable: false),
                        Recipe_ID = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Recipes", t => t.Recipe_ID)
                .ForeignKey("dbo.Users", t => t.User_ID)
                .Index(t => t.User_ID)
                .Index(t => t.Recipe_ID);
            
            CreateTable(
                "dbo.RecipeIngredients",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        Quantity = c.Int(nullable: false),
                        Measurement_Unit = c.String(maxLength: 200),
                        IsOptional = c.Boolean(),
                        Category = c.String(maxLength: 300),
                        Other_Info = c.String(maxLength: 300),
                        Recipe_ID = c.Long(nullable: false),
                        Ingredient_ID = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Ingredients", t => t.Ingredient_ID)
                .ForeignKey("dbo.Recipes", t => t.Recipe_ID)
                .Index(t => t.Recipe_ID)
                .Index(t => t.Ingredient_ID);
            
            CreateTable(
                "dbo.Ingredients",
                c => new
                    {
                        Ingredient_ID = c.Long(nullable: false, identity: true),
                        Ingredient_Name = c.String(nullable: false, maxLength: 200),
                    })
                .PrimaryKey(t => t.Ingredient_ID);
            
            CreateTable(
                "dbo.RecipeNutritionElements",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        Value = c.Int(),
                        Measurement_Unit = c.String(maxLength: 200),
                        Recipe_ID = c.Long(nullable: false),
                        NutritionElement_ID = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.NutritionElements", t => t.NutritionElement_ID)
                .ForeignKey("dbo.Recipes", t => t.Recipe_ID)
                .Index(t => t.Recipe_ID)
                .Index(t => t.NutritionElement_ID);
            
            CreateTable(
                "dbo.NutritionElements",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        Element_Name = c.String(nullable: false, maxLength: 200),
                        Element_Description = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.RecipeSteps",
                c => new
                    {
                        Step_ID = c.Long(nullable: false, identity: true),
                        Step_Number = c.Int(),
                        Step_Description = c.String(),
                        Step_Title = c.String(maxLength: 300),
                        IsOptional = c.Boolean(),
                        Picture_Path = c.String(),
                        Recipe_ID = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Step_ID)
                .ForeignKey("dbo.Recipes", t => t.Recipe_ID)
                .Index(t => t.Recipe_ID);
            
            CreateTable(
                "dbo.RecipeTags",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        Tag_ID = c.Long(nullable: false),
                        Recipe_ID = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Recipes", t => t.Recipe_ID)
                .ForeignKey("dbo.Tags", t => t.Tag_ID)
                .Index(t => t.Tag_ID)
                .Index(t => t.Recipe_ID);
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        Tag_ID = c.Long(nullable: false, identity: true),
                        Tag_Name = c.String(nullable: false, maxLength: 200),
                    })
                .PrimaryKey(t => t.Tag_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RecipeTags", "Tag_ID", "dbo.Tags");
            DropForeignKey("dbo.RecipeTags", "Recipe_ID", "dbo.Recipes");
            DropForeignKey("dbo.RecipeSteps", "Recipe_ID", "dbo.Recipes");
            DropForeignKey("dbo.RecipeNutritionElements", "Recipe_ID", "dbo.Recipes");
            DropForeignKey("dbo.RecipeNutritionElements", "NutritionElement_ID", "dbo.NutritionElements");
            DropForeignKey("dbo.RecipeIngredients", "Recipe_ID", "dbo.Recipes");
            DropForeignKey("dbo.RecipeIngredients", "Ingredient_ID", "dbo.Ingredients");
            DropForeignKey("dbo.RecipeCategories", "Recipe_ID", "dbo.Recipes");
            DropForeignKey("dbo.SavedRecipes", "User_ID", "dbo.Users");
            DropForeignKey("dbo.SavedRecipes", "Recipe_ID", "dbo.Recipes");
            DropForeignKey("dbo.Reviews", "User_ID", "dbo.Users");
            DropForeignKey("dbo.Reviews", "Recipe_ID", "dbo.Recipes");
            DropForeignKey("dbo.Recipes", "Author_User_ID", "dbo.Users");
            DropForeignKey("dbo.Ratings", "User_ID", "dbo.Users");
            DropForeignKey("dbo.Ratings", "Recipe_ID", "dbo.Recipes");
            DropForeignKey("dbo.RecipeCategories", "Category_ID", "dbo.Categories");
            DropIndex("dbo.RecipeTags", new[] { "Recipe_ID" });
            DropIndex("dbo.RecipeTags", new[] { "Tag_ID" });
            DropIndex("dbo.RecipeSteps", new[] { "Recipe_ID" });
            DropIndex("dbo.RecipeNutritionElements", new[] { "NutritionElement_ID" });
            DropIndex("dbo.RecipeNutritionElements", new[] { "Recipe_ID" });
            DropIndex("dbo.RecipeIngredients", new[] { "Ingredient_ID" });
            DropIndex("dbo.RecipeIngredients", new[] { "Recipe_ID" });
            DropIndex("dbo.SavedRecipes", new[] { "Recipe_ID" });
            DropIndex("dbo.SavedRecipes", new[] { "User_ID" });
            DropIndex("dbo.Reviews", new[] { "Recipe_ID" });
            DropIndex("dbo.Reviews", new[] { "User_ID" });
            DropIndex("dbo.Ratings", new[] { "Recipe_ID" });
            DropIndex("dbo.Ratings", new[] { "User_ID" });
            DropIndex("dbo.Recipes", new[] { "Author_User_ID" });
            DropIndex("dbo.RecipeCategories", new[] { "Recipe_ID" });
            DropIndex("dbo.RecipeCategories", new[] { "Category_ID" });
            DropTable("dbo.Tags");
            DropTable("dbo.RecipeTags");
            DropTable("dbo.RecipeSteps");
            DropTable("dbo.NutritionElements");
            DropTable("dbo.RecipeNutritionElements");
            DropTable("dbo.Ingredients");
            DropTable("dbo.RecipeIngredients");
            DropTable("dbo.SavedRecipes");
            DropTable("dbo.Reviews");
            DropTable("dbo.Users");
            DropTable("dbo.Ratings");
            DropTable("dbo.Recipes");
            DropTable("dbo.RecipeCategories");
            DropTable("dbo.Categories");
        }
    }
}
