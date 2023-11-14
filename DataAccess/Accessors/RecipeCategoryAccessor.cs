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
            using (var context = new DatabaseRepository.RecipesRealmContext())
            {
                var recipes = context.RecipeCategories.Where(r => r.Category_ID == id).Include(r => r.Recipe).Select(r => r.Recipe).ToList();

                return recipes;
            }
        }

        public static IEnumerable<Category> GetCategoriesForRecipe(long id)
        {
            using (var context = new DatabaseRepository.RecipesRealmContext())
            {
                var categories = context.RecipeCategories.Where(r => r.Recipe_ID == id).Include(r => r.Category).Select(r => r.Category).ToList();

                return categories;
            }
        }
    }
}
