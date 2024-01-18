using System.Linq;
using ModelsLibrary;
using DatabaseRepository;

namespace DataAccess {
    public class RecipeIngredientAccessor : IRecipeIngredientAccessor {

        public RecipesRealmContext context;

        public RecipeIngredientAccessor(IRecipesRealmContext db) {
            context = (RecipesRealmContext)db;
        }

        public void AddRecipeIngredient(RecipeIngredient recipeIngredient) {
            context.RecipeIngredients.Add(recipeIngredient);

            context.SaveChanges();
        }

        public void DeleteAllIngredientsForRecipe(long recipeId) {
            var recipeIngreds = context.RecipeIngredients.Where(r => r.Recipe_ID == recipeId).ToList();

            context.RecipeIngredients.RemoveRange(recipeIngreds);
            context.SaveChanges();
        }

        public void DeleteRecipeIngredient(long id) {
            var recipeIngred = context.RecipeIngredients.FirstOrDefault(r => r.ID == id);

            if (recipeIngred != null) context.RecipeIngredients.Remove(recipeIngred);

            context.SaveChanges();
        }
    }

    public interface IRecipeIngredientAccessor {
        void AddRecipeIngredient(RecipeIngredient recipeIngredient);

        void DeleteAllIngredientsForRecipe(long recipeId);

        void DeleteRecipeIngredient(long id);
    }
}
