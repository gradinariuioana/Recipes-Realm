using DatabaseRepository;
using ModelsLibrary;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DataAccess {
    public class SavedRecipeAccessor : ISavedRecipeAccessor {

        public RecipesRealmContext context;

        public SavedRecipeAccessor(IRecipesRealmContext db) {
            context = (RecipesRealmContext)db;
        }
        public bool CheckUserSavedRecipe(long recipeId, long userId) {
            return context.SavedRecipes.Any(r => r.Recipe_ID == recipeId && r.User_ID == userId);
        }

        public List<SavedRecipe> GetSavedRecipesForUser(long userId) {
            var savedRecipes = context.SavedRecipes.Where(r => r.User_ID == userId).Include("Recipe.RecipeTags.Tag").Include("Recipe.RecipeCategories.Category").ToList();

            return savedRecipes;
        }

        public List<Tag> GetLikedTagsFromSaving(List<SavedRecipe> savedRecipes) {
            List<Tag> likedTags = new List<Tag>();

            foreach (var savedRecipe in savedRecipes) {
                //Get tags of rated recipe
                var recipeTags = savedRecipe.Recipe.RecipeTags.Select(rt => rt.Tag).ToList();
                likedTags.AddRange(recipeTags);
            }
            return likedTags;
        }

        public List<Category> GetLikedCategoriesFromSaving(List<SavedRecipe> savedRecipes) {
            List<Category> likedCategories = new List<Category>();

            foreach (var savedRecipe in savedRecipes) {
                //Get categories of rated recipe
                var recipeCategories = savedRecipe.Recipe.RecipeCategories.Select(rt => rt.Category).ToList();
                likedCategories.AddRange(recipeCategories);
            }
            return likedCategories;
        }
    }

    public interface ISavedRecipeAccessor {
        bool CheckUserSavedRecipe(long recipeId, long userId);

        List<SavedRecipe> GetSavedRecipesForUser(long userId);

        List<Tag> GetLikedTagsFromSaving(List<SavedRecipe> savedRecipes);

        List<Category> GetLikedCategoriesFromSaving(List<SavedRecipe> savedRecipes);
    }
}
