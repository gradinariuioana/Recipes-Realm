using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using ModelsLibrary;
using DatabaseRepository;

namespace DataAccess {
    public class RecipeTagAccessor : IRecipeTagAccessor {

        public RecipesRealmContext context;

        public RecipeTagAccessor(IRecipesRealmContext db) {
            context = (RecipesRealmContext)db;
        }
        public IEnumerable<Recipe> GetRecipesForTag(long id) {
            var recipes = context.RecipeTags.Where(r => r.Tag_ID == id).Include(r => r.Recipe).Select(r => r.Recipe).Where(r => r.Is_Active == true).ToList();
            return recipes;
        }

        public ICollection<long> GetTagsIdsForRecipe(long id) {
            var tags = context.RecipeTags.Where(r => r.Recipe_ID == id).Select(r => r.Tag_ID).ToList();
            return tags;
        }
        public void AddRecipeTag(RecipeTag recipeTag) {
            var tags = context.RecipeTags.Add(recipeTag);
            context.SaveChanges();
        }

        public void DeleteAllTagsForRecipe(long recipeId) {
            var recipeIngreds = context.RecipeTags.Where(r => r.Recipe_ID == recipeId).ToList();

            context.RecipeTags.RemoveRange(recipeIngreds);
            context.SaveChanges();
        }

        public void DeleteRecipeTag(long id) {
            var recipeTag = context.RecipeTags.FirstOrDefault(r => r.ID == id);

            if (recipeTag != null) {
                context.RecipeTags.Remove(recipeTag);
            }
            context.SaveChanges();
        }

    }

    public interface IRecipeTagAccessor {
        IEnumerable<Recipe> GetRecipesForTag(long id);

        ICollection<long> GetTagsIdsForRecipe(long id);
        void AddRecipeTag(RecipeTag recipeTag);

        void DeleteAllTagsForRecipe(long recipeId);

        void DeleteRecipeTag(long id);
    }
}
