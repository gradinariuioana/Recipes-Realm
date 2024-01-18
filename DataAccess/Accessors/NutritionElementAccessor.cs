using DatabaseRepository;
using ModelsLibrary;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess {
    public class NutritionElementAccessor : INutritionElementAccessor {

        public RecipesRealmContext context;

        public NutritionElementAccessor(IRecipesRealmContext db) {
            context = (RecipesRealmContext)db;
        }
        public IEnumerable<NutritionElement> GetNutritionElementsList() {

            var nutritionElements = context.NutritionElements.ToList();

            return nutritionElements;
        }
    }

    public interface INutritionElementAccessor {
        IEnumerable<NutritionElement> GetNutritionElementsList();
    }
}
