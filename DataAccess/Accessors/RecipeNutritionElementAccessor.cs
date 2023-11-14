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
        public static void ShowRecipeNutritionElements()
        {
            using (var context = new DatabaseRepository.RecipesRealmContext())
            {
                Console.WriteLine("--- Recipe Nutrition Elements ---");

                var recipeNutritionElements = context.RecipeNutritionElements.ToList();
                foreach (RecipeNutritionElement item in recipeNutritionElements)
                {
                    //Recipe and Nutrition Element - Lazy Loading
                    Console.WriteLine("Recipe Name: {0}\nNutrition Element Name:{1}\nNutrition Element Value: {2}{3}\n\n",
                                       item.Recipe.Recipe_Name, item.NutritionElement.Element_Name, item.Value, item.Measurement_Unit);
                }
            }
        }

        public static string GetNutritionElementName(int idx)
        {
            using (var context = new DatabaseRepository.RecipesRealmContext())
            {
                //Nutrition Element - Eager Loading
                var recipeNutritionElement = context.RecipeNutritionElements.Include(r => r.NutritionElement).FirstOrDefault(r => r.ID == idx);
                return recipeNutritionElement.NutritionElement.Element_Name;
            }
        }

        public static string GetRecipeName(int idx)
        {
            using (var context = new DatabaseRepository.RecipesRealmContext())
            {
                //Recipe - Eager Loading
                var recipeNutritionElement = context.RecipeNutritionElements.Include(r => r.Recipe).FirstOrDefault(r => r.ID == idx);
                return recipeNutritionElement.Recipe.Recipe_Name;
            }
        }
    }
}
