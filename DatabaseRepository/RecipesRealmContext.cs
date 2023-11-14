using ModelsLibrary;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace DatabaseRepository
{
    public partial class RecipesRealmContext : DbContext
    {
        public RecipesRealmContext() : base("RecipesRealmDBConnectionString")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
        }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Ingredient> Ingredients { get; set; }
        public virtual DbSet<NutritionElement> NutritionElements { get; set; }
        public virtual DbSet<Rating> Ratings { get; set; }
        public virtual DbSet<Recipe> Recipes { get; set; }
        public virtual DbSet<RecipeCategory> RecipeCategories { get; set; }
        public virtual DbSet<RecipeIngredient> RecipeIngredients { get; set; }
        public virtual DbSet<RecipeNutritionElement> RecipeNutritionElements { get; set; }
        public virtual DbSet<RecipeStep> RecipeSteps { get; set; }
        public virtual DbSet<RecipeTag> RecipeTags { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<SavedRecipe> SavedRecipes { get; set; }
    }
}
