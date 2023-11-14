using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using ModelsLibrary;

namespace DataAccess
{
    public class RecipeTagAccessor
    {
        public static IEnumerable<Recipe> GetRecipesByTag(long id)
        {
            using (var context = new DatabaseRepository.RecipesRealmContext())
            {
                var recipes = context.RecipeTags.Where(r => r.Tag_ID == id).Include(r => r.Recipe).Select(r => r.Recipe).ToList();

                return recipes;
            }
        }

        public static IEnumerable<Tag> GetTagsForRecipe(long id)
        {
            using (var context = new DatabaseRepository.RecipesRealmContext())
            {
                var tags = context.RecipeTags.Where(r => r.Recipe_ID == id).Include(r => r.Tag).Select(r => r.Tag).ToList();

                return tags;
            }
        }
    }
}
