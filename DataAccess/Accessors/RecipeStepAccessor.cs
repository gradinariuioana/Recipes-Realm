using System.Linq;
using ModelsLibrary;
using DatabaseRepository;

namespace DataAccess {
    public class RecipeStepAccessor : IRecipeStepAccessor {

        public RecipesRealmContext context;

        public RecipeStepAccessor(IRecipesRealmContext db) {
            context = (RecipesRealmContext)db;
        }

        public void AddRecipeStep(RecipeStep recipeStep) {
            context.RecipeSteps.Add(recipeStep);
            context.SaveChanges();
        }

        public void DeleteAllStepsForRecipe(long recipeId) {
            var recipeIngreds = context.RecipeSteps.Where(r => r.Recipe_ID == recipeId).ToList();

            context.RecipeSteps.RemoveRange(recipeIngreds);
            context.SaveChanges();
        }

        public void DeleteRecipeStep(RecipeStep recipeStep) {
            context.RecipeSteps.Remove(recipeStep);
            context.SaveChanges();
        }
    }

    public interface IRecipeStepAccessor {
        void AddRecipeStep(RecipeStep recipeStep);

        void DeleteAllStepsForRecipe(long recipeId);

        void DeleteRecipeStep(RecipeStep recipeStep);
    }
}
