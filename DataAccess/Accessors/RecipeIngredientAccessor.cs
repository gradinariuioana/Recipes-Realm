using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using ModelsLibrary;

namespace DataAccess
{
    public class RecipeIngredientAccessor
    {
        public static IEnumerable<RecipeIngredient> GetIngredientsForRecipe(long id)
        {
            using (var context = new DatabaseRepository.RecipesRealmContext())
            {
                var ingreds = context.RecipeIngredients.Where(r => r.Recipe_ID == id).Include(r => r.Ingredient).ToList();

                return ingreds;
            }
        }

        public static void AddRecipeIngredient(RecipeIngredient recipeIngredient) {
            using (var context = new DatabaseRepository.RecipesRealmContext()) {
                context.RecipeIngredients.Add(recipeIngredient);

                context.SaveChanges();
            }
        }
    }
}
