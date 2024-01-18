using System.Linq;
using ModelsLibrary;
using DatabaseRepository;

namespace DataAccess {
    public class RecipeNutritionElementAccessor : IRecipeNutritionElementAccessor {

        public RecipesRealmContext context;

        public RecipeNutritionElementAccessor(IRecipesRealmContext db) {
            context = (RecipesRealmContext)db;
        }
        public void AddRecipeNutritionElement(RecipeNutritionElement recipeNutritionElement) {
            context.RecipeNutritionElements.Add(recipeNutritionElement);
            context.SaveChanges();
        }

        public void DeleteAllNutritionElementsForRecipe(long recipeId) {
            var recipeIngreds = context.RecipeNutritionElements.Where(r => r.Recipe_ID == recipeId).ToList();

            context.RecipeNutritionElements.RemoveRange(recipeIngreds);
            context.SaveChanges();
        }

        public void DeleteRecipeNutritionElement(long id) {
            var recipeNutritionElement = context.RecipeNutritionElements.FirstOrDefault(r => r.ID == id);

            if (recipeNutritionElement != null) context.RecipeNutritionElements.Remove(recipeNutritionElement);
            context.SaveChanges();
        }
    }

    public interface IRecipeNutritionElementAccessor {
        void AddRecipeNutritionElement(RecipeNutritionElement recipeNutritionElement);

        void DeleteAllNutritionElementsForRecipe(long recipeId);

        void DeleteRecipeNutritionElement(long id);
    }
}
