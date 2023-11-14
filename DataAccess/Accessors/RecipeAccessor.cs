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
        public static void ShowRecipes()
        {
            using (var context = new DatabaseRepository.RecipesRealmContext())
            {
                Console.WriteLine("--- Recipes ---");

                var recipes = context.Recipes.ToList();
                foreach (Recipe item in recipes)
                {
                    //User - Lazy Loading
                    Console.WriteLine("Recipe Name: {0}\nRecipe Description: {1}\nCooking Time: {2}, Difficulty Level: {3}\nAuthor: {4} \n\n", 
                                       item.Recipe_Name, item.Recipe_Description, item.Cooking_Time, item.Difficulty_Level, item.User.User_Name);
                }
            }
        }

        public static Recipe GetRecipe(long idx)
        {
            using (var context = new DatabaseRepository.RecipesRealmContext())
            {
                var recipe = context.Recipes.AsNoTracking().FirstOrDefault(r => r.Recipe_ID == idx);
                return recipe;
            }
        }
    }
}
