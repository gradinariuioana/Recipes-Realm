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
            using (var context = new DatabaseRepository.RecipesRealmContext()) {
                var ingreds = context.RecipeIngredients.Where(r => r.Recipe_ID == id).Include(r => r.Ingredient).ToList();
                return ingreds;
            }
        }

        public static ICollection<long> GetIngredientsIdsForRecipe(long id) {
            using (var context = new DatabaseRepository.RecipesRealmContext()) {
                var ingreds = context.RecipeIngredients.Where(r => r.Recipe_ID == id).Select(r => r.Ingredient_ID).ToList();
                return ingreds;
            }
        }

        public static void AddRecipeIngredient(RecipeIngredient recipeIngredient) {
            using (var context = new DatabaseRepository.RecipesRealmContext()) {
                context.RecipeIngredients.Add(recipeIngredient);

                context.SaveChanges();
            }
        }

        public static void DeleteAllIngredientsForRecipe(long recipeId) {
            using (var context = new DatabaseRepository.RecipesRealmContext()) {
                var recipeIngreds = context.RecipeIngredients.Where(r => r.Recipe_ID == recipeId).ToList();

                context.RecipeIngredients.RemoveRange(recipeIngreds);
                context.SaveChanges();
            }
        }

        public static void DeleteRecipeIngredient(RecipeIngredient recipeIngredient) {
            using (var context = new DatabaseRepository.RecipesRealmContext()) {
                context.RecipeIngredients.Remove(recipeIngredient);
                context.SaveChanges();
            }
        }
    }
}
