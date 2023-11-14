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
        public static IEnumerable<RecipeStep> GetStepsForRecipe(long id)
        {
            using (var context = new DatabaseRepository.RecipesRealmContext())
            {
                var steps = context.RecipeSteps.Where(r => r.Recipe_ID == id).ToList();

                return steps;
            }
        }
    }
}
