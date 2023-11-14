using ModelsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class IngredientAccessor
    {
        public static void ShowIngredients()
        {
            using (var context = new DatabaseRepository.RecipesRealmContext())
            {
                Console.WriteLine("--- Ingredients ---");

                var ingredients = context.Ingredients.ToList();
                foreach (Ingredient item in ingredients)
                {
                    Console.WriteLine("Ingredient Name: {0}", 
                                       item.Ingredient_Name);
                }
            }
        }

        public static string GetIngredientName(int idx)
        {
            using (var context = new DatabaseRepository.RecipesRealmContext())
            {
                var ingredient = context.Ingredients.FirstOrDefault(i => i.Ingredient_ID == idx);
                return ingredient.Ingredient_Name;
            }
        }
    }
}
