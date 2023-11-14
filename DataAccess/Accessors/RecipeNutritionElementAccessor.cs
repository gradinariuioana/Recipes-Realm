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
            using (var context = new DatabaseRepository.RecipesRealmContext())
            {
                var nElems = context.RecipeNutritionElements.Where(r => r.Recipe_ID == id).Include(r => r.NutritionElement).ToList();

                return nElems;
            }
        }
    }
}
