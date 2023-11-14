using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using ModelsLibrary;

namespace DataAccess
{
    public class RecipeStepAccessor
    {
        public static void ShowRecipeSteps()
        {
            using (var context = new DatabaseRepository.RecipesRealmContext())
            {
                Console.WriteLine("--- Recipe Steps ---");

                var recipeSteps = context.RecipeSteps.ToList();
                foreach (RecipeStep item in recipeSteps)
                {
                    //Recipe - Lazy Loading
                    Console.WriteLine("Recipe Name: {0}\nStep Number: {1}\nStep Description: {2}\n\n",
                                       item.Recipe.Recipe_Name, item.Step_Number, item.Step_Description);
                }
            }
        }

        public static string GetRecipeStepTitle(int idx)
        {
            using (var context = new DatabaseRepository.RecipesRealmContext())
            {
                var recipeStep = context.RecipeSteps.AsNoTracking().FirstOrDefault(r => r.Step_ID == idx);
                return recipeStep.Step_Title;
            }
        }
    }
}
