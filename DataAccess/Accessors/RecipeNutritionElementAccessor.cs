using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using ModelsLibrary;

namespace DataAccess
{
    public class RecipeNutritionElementAccessor
    {
        public static IEnumerable<RecipeNutritionElement> GetNutritionElementsForRecipe(long id)
        {
            using (var context = new DatabaseRepository.RecipesRealmContext()) {
                var nElems = context.RecipeNutritionElements.Where(r => r.Recipe_ID == id).Include(r => r.NutritionElement).ToList();

                return nElems;
            }
        }

        public static ICollection<long> GetNutritionElementsIdsForRecipe(long id) {
            using (var context = new DatabaseRepository.RecipesRealmContext()) {
                var nElems = context.RecipeNutritionElements.Where(r => r.Recipe_ID == id).Select(r => r.NutritionElement_ID).ToList();

                return nElems;
            }
        }

        public static void AddRecipeNutritionElement(RecipeNutritionElement recipeNutritionElement) {
            using (var context = new DatabaseRepository.RecipesRealmContext()) {
                context.RecipeNutritionElements.Add(recipeNutritionElement);
                context.SaveChanges();
            }
        }

        public static void DeleteAllNutritionElementsForRecipe(long recipeId) {
            using (var context = new DatabaseRepository.RecipesRealmContext()) {
                var recipeIngreds = context.RecipeNutritionElements.Where(r => r.Recipe_ID == recipeId).ToList();

                context.RecipeNutritionElements.RemoveRange(recipeIngreds);
                context.SaveChanges();
            }
        }

        public static void DeleteRecipeNutritionElement(RecipeNutritionElement recipeNutritionElement) {
            using (var context = new DatabaseRepository.RecipesRealmContext()) {
                context.RecipeNutritionElements.Remove(recipeNutritionElement);
                context.SaveChanges();
            }
        }
    }
}
