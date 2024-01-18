using DatabaseRepository;
using ModelsLibrary;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess {
    public class CategoryAccessor : ICategoryAccessor {

        public RecipesRealmContext context;

        public CategoryAccessor(IRecipesRealmContext db) {
            context = (RecipesRealmContext)db;
        }

        public Category GetCategoryById(long id) {
            var category = context.Categories.FirstOrDefault(c => c.Category_ID == id);
            return category;
        }

        public IEnumerable<Category> GetCategoriesList() {
            var categories = context.Categories.ToList();
            return categories;
        }
    }

    public interface ICategoryAccessor {
        IEnumerable<Category> GetCategoriesList();
        Category GetCategoryById(long id);
    }

}
