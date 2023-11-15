using ModelsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class RecipeAccessor
    {
        public static IEnumerable<Recipe> GetRecipesList() {
            using (var context = new DatabaseRepository.RecipesRealmContext()) {
                var recipes = context.Recipes.ToList();
                return recipes;
            }
        }

        public static Recipe GetRecipe(long id) {
            using (var context = new DatabaseRepository.RecipesRealmContext()) {
                var recipe = context.Recipes.FirstOrDefault(r => r.Recipe_ID == id);
                return recipe;
            }
        }

        public static long AddRecipe(Recipe recipe) {
            using (var context = new DatabaseRepository.RecipesRealmContext()) {
                context.Recipes.Add(recipe);
                context.SaveChanges();

                return recipe.Recipe_ID;
            }
        }

        public static void UpdateRecipe(Recipe recipe) {
            using (var context = new DatabaseRepository.RecipesRealmContext()) {
                Recipe oldRecipe = context.Recipes.FirstOrDefault(r => r.Recipe_ID == recipe.Recipe_ID);
                oldRecipe.Recipe_Name = recipe.Recipe_Name;
                oldRecipe.Cooking_Time = recipe.Cooking_Time;
                oldRecipe.Difficulty_Level = recipe.Difficulty_Level;
                oldRecipe.RecipeSteps = recipe.RecipeSteps;

                context.SaveChanges();
            }
        }

        public static void DeleteRecipe(long id) {
            using (var context = new DatabaseRepository.RecipesRealmContext()) {
                Recipe recipe = context.Recipes.FirstOrDefault(r => r.Recipe_ID == id);

                context.Recipes.Remove(recipe);
                context.SaveChanges();
            }
        }
    }
}
