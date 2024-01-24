using ModelsLibrary;
using System.Collections.Generic;

namespace BusinessLayer.Interfaces {
    public interface IRecipeRecommendationEngine {
        IEnumerable<Category> GetCategoriesList();
        Category GetCategoryById(long id);
        IEnumerable<Recipe> GetRecipes(long id);
        long AddIngredient(Ingredient ingredient);
        IEnumerable<Tag> GetTagsList();
        IEnumerable<NutritionElement> GetNutritionElementsList();
        IEnumerable<Recipe> GetRecipesList();
        IEnumerable<Ingredient> GetIngredientsList();
        long AddRecipe(Recipe recipe);
        Recipe GetRecipe(long id);
        void UpdateRecipe(Recipe recipe);

        ICollection<long> GetTagsIdsForRecipe(long id);

        void AddRecipeTag(RecipeTag recipeTag);

        void DeleteRecipeTag(long id);

        ICollection<long> GetCategoriesIdsForRecipe(long id);
        void DeleteRecipeCategory(long id);
        void AddRecipeCategory(RecipeCategory recipeCategory);

        void DeleteAllNutritionElementsForRecipe(long recipeId);

        void AddRecipeNutritionElement(RecipeNutritionElement recipeNutritionElement);

        void DeleteAllIngredientsForRecipe(long recipeId);

        void AddRecipeIngredient(RecipeIngredient recipeIngredient);

        void DeleteAllStepsForRecipe(long recipeId);

        void AddRecipeStep(RecipeStep recipeStep);

        void DeleteRecipe(long id);

        void DeleteAllTagsForRecipe(long recipeId);

        void DeleteAllCategoriesForRecipe(long recipeId);

        void DeactivateRecipe(long id);
        void ReactivateRecipe(long id);

        string GetUserName(long id);

        long GetAverageRatingForRecipe(long recipeId);

        List<Recipe> GenerateRecommendations(long userId);
        List<Recipe> GenerateRecommendationsNoUser(int? number, List<long> idsToExclude = null);

        User CheckUserForLogin(User user);

        User SignUpUser(User user);

        bool CheckUserEmailExists(string email);

        bool CheckIngredientExists(string ingredient);

        List<Recipe> GetUserRecipes(long userId);
        List<Recipe> GetUserSavedRecipes(long userId);

        long AddRating(Rating rating);

        bool CheckUserCanRate(Rating rating);

        bool CheckSavedRecipe(SavedRecipe savedRecipe);

        void SaveRecipe(SavedRecipe savedRecipe);
        void UnSaveRecipe(SavedRecipe savedRecipe);

    }
}
