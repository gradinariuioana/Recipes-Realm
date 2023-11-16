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
        public static IEnumerable<Recipe> GetRecipesForTag(long id)
        {
            using (var context = new DatabaseRepository.RecipesRealmContext())
            {
                var recipes = context.RecipeTags.Where(r => r.Tag_ID == id).Include(r => r.Recipe).Select(r => r.Recipe).ToList();
                return recipes;
            }
        }

        public static IEnumerable<Tag> GetTagsForRecipe(long id) {
            using (var context = new DatabaseRepository.RecipesRealmContext()) {
                var tags = context.RecipeTags.Where(r => r.Recipe_ID == id).Include(r => r.Tag).Select(r => r.Tag).ToList();
                return tags;
            }
        }

        public static ICollection<long> GetTagIdsForRecipe(long id)
        {
            using (var context = new DatabaseRepository.RecipesRealmContext())
            {
                var tags = context.RecipeTags.Where(r => r.Recipe_ID == id).Include(r => r.Tag).Select(r => r.Tag.Tag_ID).ToList();
                return tags;
            }
        }
        public static void AddRecipeTag(RecipeTag recipeTag) {
            using (var context = new DatabaseRepository.RecipesRealmContext()) {
                var tags = context.RecipeTags.Add(recipeTag);
                context.SaveChanges();
            }
        }

        public static void DeleteAllTagsForRecipe(long recipeId) {
            using (var context = new DatabaseRepository.RecipesRealmContext()) {
                var recipeIngreds = context.RecipeTags.Where(r => r.Recipe_ID == recipeId).ToList();

                context.RecipeTags.RemoveRange(recipeIngreds);
                context.SaveChanges();
            }
        }

        public static void DeleteRecipeTag(RecipeTag recipeTag) {
            using (var context = new DatabaseRepository.RecipesRealmContext()) {
                context.RecipeTags.Remove(recipeTag);
                context.SaveChanges();
            }
        }

    }
}
