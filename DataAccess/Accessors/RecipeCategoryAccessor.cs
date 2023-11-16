using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using ModelsLibrary;

namespace DataAccess
{
    public class RecipeCategoryAccessor
    {
        public static IEnumerable<Recipe> GetRecipesForCategory(long id)
        {
            using (var context = new DatabaseRepository.RecipesRealmContext())  {
                var recipes = context.RecipeCategories.Where(r => r.Category_ID == id).Include(r => r.Recipe).Select(r => r.Recipe).ToList();
                return recipes;
            }
        }

        public static IEnumerable<Category> GetCategoriesForRecipe(long id)
        {
            using (var context = new DatabaseRepository.RecipesRealmContext()) {
                var categories = context.RecipeCategories.Where(r => r.Recipe_ID == id).Include(r => r.Category).Select(r => r.Category).ToList();
                return categories;
            }
        }

        public static ICollection<long> GetCategoriesIdsForRecipe(long id) {
            using (var context = new DatabaseRepository.RecipesRealmContext()) {
                var categories = context.RecipeCategories.Where(r => r.Recipe_ID == id).Select(r => r.Category_ID).ToList();
                return categories;
            }
        }

        public static void AddRecipeCategory(RecipeCategory recipeCategory) {
            using (var context = new DatabaseRepository.RecipesRealmContext()) {
                var categories = context.RecipeCategories.Add(recipeCategory);
                context.SaveChanges();
            }
        }

        public static void DeleteAllCategoriesForRecipe(long recipeId) {
            using (var context = new DatabaseRepository.RecipesRealmContext()) {
                var recipeCategs = context.RecipeCategories.Where(r => r.Recipe_ID == recipeId).ToList();

                context.RecipeCategories.RemoveRange(recipeCategs);
                context.SaveChanges();
            }
        }

        public static void DeleteRecipeCategory(RecipeCategory recipeCategory) {
            using (var context = new DatabaseRepository.RecipesRealmContext()) {
                context.RecipeCategories.Remove(recipeCategory);
                context.SaveChanges();
            }
        }
    }
}
