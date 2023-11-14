using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using ModelsLibrary;

namespace DataAccess
{
    public class RecipeIngredientAccessor
    {
        public static void ShowRecipeIngredients()
        {
            using (var context = new DatabaseRepository.RecipesRealmContext())
            {
                Console.WriteLine("--- Recipe Ingredients ---");

                var recipeIngredients = context.RecipeIngredients.ToList();
                foreach (RecipeIngredient item in recipeIngredients)
                {
                    //Recipe and Ingredient - Lazy Loading
                    Console.WriteLine("Recipe Name: {0}\nIngredient Name:{1}\nIngredient Quantity: {2}{3}\n\n",
                                       item.Recipe.Recipe_Name, item.Ingredient.Ingredient_Name, item.Quantity, item.Measurement_Unit);
                }
            }
        }

        public static string GetIngredientName(int idx)
        {
            using (var context = new DatabaseRepository.RecipesRealmContext())
            {
                //Ingredient - Eager Loading
                var recipeIngredient = context.RecipeIngredients.Include(r => r.Ingredient).FirstOrDefault(r => r.ID == idx);
                return recipeIngredient.Ingredient.Ingredient_Name;
            }
        }

        public static string GetRecipeName(int idx)
        {
            using (var context = new DatabaseRepository.RecipesRealmContext())
            {
                //Recipe - Eager Loading
                var recipeIngredient = context.RecipeIngredients.Include(r => r.Recipe).FirstOrDefault(r => r.ID == idx);
                return recipeIngredient.Recipe.Recipe_Name;
            }
        }
    }
}
