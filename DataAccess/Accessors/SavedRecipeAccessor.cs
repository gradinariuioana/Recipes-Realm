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

        public List<Recipe> GetSavedRecipesListForUser(long userId) {
            var savedRecipes = context.SavedRecipes.Where(r => r.User_ID == userId).Include(r => r.Recipe).Select(s => s.Recipe)
                                    .Where(r => r.Is_Active == true)
                                    .Include("RecipeIngredients.Ingredient")
                                    .Include(r => r.RecipeSteps)
                                    .Include("RecipeCategories.Category")
                                    .Include("RecipeTags.Tag")
                                    .Include(r => r.User)
                                    .Include("RecipeNutritionElements.NutritionElement")
                                    .ToList();
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

        public bool CheckUserSavedRecipe(SavedRecipe savedRecipe) {
            var r = context.SavedRecipes.FirstOrDefault(re => re.Recipe_ID == savedRecipe.Recipe_ID && re.User_ID == savedRecipe.User_ID);

            return r != null;
        }

        public void SaveRecipe(SavedRecipe savedRecipe) {
            context.SavedRecipes.Add(savedRecipe);
            context.SaveChanges();
        }

        public void UnSaveRecipe(SavedRecipe savedRecipe) {
            var r = context.SavedRecipes.FirstOrDefault(s => s.Recipe_ID == savedRecipe.Recipe_ID && s.User_ID == savedRecipe.User_ID);
            if (r != null) {

                context.SavedRecipes.Remove(r);
                context.SaveChanges();
            }
        }
    }

    public interface ISavedRecipeAccessor {
        bool CheckUserSavedRecipe(long recipeId, long userId);

        List<SavedRecipe> GetSavedRecipesForUser(long userId);

        List<Tag> GetLikedTagsFromSaving(List<SavedRecipe> savedRecipes);

        List<Category> GetLikedCategoriesFromSaving(List<SavedRecipe> savedRecipes);

        bool CheckUserSavedRecipe(SavedRecipe savedRecipe);

        void SaveRecipe(SavedRecipe savedRecipe);
        void UnSaveRecipe(SavedRecipe savedRecipe);
        List<Recipe> GetSavedRecipesListForUser(long userId);


    }
}
