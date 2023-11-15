using ModelsLibrary;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess
{
    public class NutritionElementAccessor
    {
        public static IEnumerable<NutritionElement> GetNutritionElementsList() {
            using (var context = new DatabaseRepository.RecipesRealmContext()) {
                var nutritionElements = context.NutritionElements.ToList();
                return nutritionElements;
            }
        }
    }
}
