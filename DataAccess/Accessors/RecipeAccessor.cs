using DatabaseRepository;
using ModelsLibrary;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DataAccess {
    public class RecipeAccessor : IRecipeAccessor {

        public RecipesRealmContext context;

        public RecipeAccessor(IRecipesRealmContext db) {
            context = (RecipesRealmContext)db;
        }

        public IEnumerable<Recipe> GetRecipesList() {
            var recipes = context.Recipes.Where(r => r.Is_Active == true).Include("RecipeIngredients.Ingredient")
                .Include(r => r.RecipeSteps)
                .Include("RecipeCategories.Category")
                .Include("RecipeTags.Tag")
                .Include(r => r.User)
                .Include("RecipeNutritionElements.NutritionElement")
                .ToList();
            return recipes;
        }

        public Recipe GetRecipe(long id) {
            var recipe = context.Recipes.Include("RecipeIngredients.Ingredient")
                .Include(r => r.RecipeSteps)
                .Include("RecipeCategories.Category")
                .Include("RecipeTags.Tag")
                .Include(r => r.User)
                .Include("RecipeNutritionElements.NutritionElement")
                .FirstOrDefault(r => r.Recipe_ID == id);
            return recipe;
        }

        public long AddRecipe(Recipe recipe) {
            context.Recipes.Add(recipe);
            context.SaveChanges();

            return recipe.Recipe_ID;
        }

        public void UpdateRecipe(Recipe recipe) {

            Recipe oldRecipe = context.Recipes.FirstOrDefault(r => r.Recipe_ID == recipe.Recipe_ID);
            oldRecipe.Recipe_Name = recipe.Recipe_Name;
            oldRecipe.Cooking_Time = recipe.Cooking_Time;
            oldRecipe.Difficulty_Level = recipe.Difficulty_Level;
            oldRecipe.RecipeSteps = recipe.RecipeSteps;
            oldRecipe.Picture_Path = recipe.Picture_Path;

            context.SaveChanges();
        }

        public void DeactivateRecipe(long id) {
            Recipe recipe = context.Recipes.FirstOrDefault(r => r.Recipe_ID == id);
            recipe.Is_Active = false;

            context.SaveChanges();
        }
        public void ReactivateRecipe(long id) {
            Recipe recipe = context.Recipes.FirstOrDefault(r => r.Recipe_ID == id);
            recipe.Is_Active = true;

            context.SaveChanges();
        }


        public void DeleteRecipe(long id) {
            Recipe recipe = context.Recipes.FirstOrDefault(r => r.Recipe_ID == id);
            context.Recipes.Remove(recipe);

            context.SaveChanges();
        }

        public List<Recipe> GetRecipesForUser(long userId) {
            var recipes = context.Recipes.Where(r => r.Author_User_ID == userId).Include("RecipeIngredients.Ingredient")
                .Include(r => r.RecipeSteps)
                .Include("RecipeCategories.Category")
                .Include("RecipeTags.Tag")
                .Include(r => r.User)
                .Include("RecipeNutritionElements.NutritionElement")
                .ToList();
            return recipes;
        }
    }

    public interface IRecipeAccessor {
        IEnumerable<Recipe> GetRecipesList();

        Recipe GetRecipe(long id);

        long AddRecipe(Recipe recipe);

        void UpdateRecipe(Recipe recipe);

        void DeactivateRecipe(long id);
        void ReactivateRecipe(long id);

        void DeleteRecipe(long id);

        List<Recipe> GetRecipesForUser(long userId);

    }
}
