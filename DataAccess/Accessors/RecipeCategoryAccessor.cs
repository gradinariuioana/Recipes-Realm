using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using ModelsLibrary;
using DatabaseRepository;

namespace DataAccess {
    public class RecipeCategoryAccessor : IRecipeCategoryAccessor {

        public RecipesRealmContext context;

        public RecipeCategoryAccessor(IRecipesRealmContext db) {
            context = (RecipesRealmContext)db;
        }

        public IEnumerable<Recipe> GetRecipesForCategory(long id) {
            var recipes = context.RecipeCategories.Where(r => r.Category_ID == id).Include(r => r.Recipe).Select(r => r.Recipe).Where(r => r.Is_Active == true).ToList();
            return recipes;
        }

        public IEnumerable<Recipe> GetRecipes(long id) {
            var recipes = context.RecipeCategories.Where(r => r.Category_ID == id).Select(r => r.Recipe).Where(r => r.Is_Active == true).Include("RecipeIngredients.Ingredient")
                .Include(r => r.RecipeSteps)
                .Include("RecipeCategories.Category")
                .Include("RecipeTags.Tag")
                .Include(r => r.User)
                .Include("RecipeNutritionElements.NutritionElement").ToList();

            return recipes;
        }

        public ICollection<long> GetCategoriesIdsForRecipe(long id) {
            var categories = context.RecipeCategories.Where(r => r.Recipe_ID == id).Select(r => r.Category_ID).ToList();
            return categories;
        }

        public void AddRecipeCategory(RecipeCategory recipeCategory) {
            var categories = context.RecipeCategories.Add(recipeCategory);
            context.SaveChanges();
        }

        public void DeleteAllCategoriesForRecipe(long recipeId) {
            var recipeCategs = context.RecipeCategories.Where(r => r.Recipe_ID == recipeId).ToList();

            context.RecipeCategories.RemoveRange(recipeCategs);
            context.SaveChanges();
        }

        public void DeleteRecipeCategory(long id) {
            var recipeCategory = context.RecipeCategories.FirstOrDefault(r => r.ID == id);

            if (recipeCategory != null) context.RecipeCategories.Remove(recipeCategory);
            context.SaveChanges();
        }
    }

    public interface IRecipeCategoryAccessor {
        IEnumerable<Recipe> GetRecipesForCategory(long id);

        IEnumerable<Recipe> GetRecipes(long id);

        ICollection<long> GetCategoriesIdsForRecipe(long id);

        void AddRecipeCategory(RecipeCategory recipeCategory);

        void DeleteAllCategoriesForRecipe(long recipeId);

        void DeleteRecipeCategory(long id);
    }
}
