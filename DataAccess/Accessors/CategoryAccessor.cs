using ModelsLibrary;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess
{
    public class CategoryAccessor
    {
        public static IEnumerable<Category> GetCategoriesList() {
            using (var context = new DatabaseRepository.RecipesRealmContext()) {
                var categories = context.Categories.ToList();
                return categories;
            }
        }
    }
}
