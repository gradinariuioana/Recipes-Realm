using ModelsLibrary;
using DataAccess;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLayer
{
    public class RecipeRecommendationEngine
    {
        private int numberOfRecommendationsToGenerate;

        public RecipeRecommendationEngine(int n)
        {
            numberOfRecommendationsToGenerate = n;
        }

        public List<Recipe> GenerateRecommendations(long userId)
        {
            List<Recipe> recommendations = new List<Recipe>();
            var allRatedRecipesIds = RatingAccessor.GetRatingsForUser(userId).Select(r => r.Recipe_ID);
            var allSavedRecipesIds = SavedRecipeAccessor.GetSavedRecipesForUser(userId).Select(r => r.Recipe_ID);
            var idsToExclude = allRatedRecipesIds.Union(allSavedRecipesIds);

            List<Tag> likedTags;
            List<Category> likedCategories;

            for (var score = 5; score > 0; score--)
            {
                var userRatings = RatingAccessor.GetRatingsWithScoreForUser(userId, score);

                if (userRatings.Count > 0)
                {
                    likedTags = GetLikedTagsFromRatings(userRatings);

                    foreach(var tag in likedTags)
                    {
                        //get recipes with liked tags that have not been rated or saved by user
                        var recipesWithLikedTags = RecipeTagAccessor.GetRecipesForTag(tag.Tag_ID).Where(r => !idsToExclude.Contains(r.Recipe_ID));
                        recommendations.AddRange(recipesWithLikedTags);
                    }

                    if (recommendations.Count > numberOfRecommendationsToGenerate)
                    {
                        return recommendations;
                    }
                    
                    likedCategories = GetLikedCategoriesFromRatings(userRatings);

                    foreach (var category in likedCategories)
                    {
                        //get recipes with liked categories that have not been rated or saved by user
                        var recipesWithLikedTags = RecipeCategoryAccessor.GetRecipesForCategory(category.Category_ID).Where(r => !idsToExclude.Contains(r.Recipe_ID));
                        recommendations.AddRange(recipesWithLikedTags);
                    }

                    if (recommendations.Count > numberOfRecommendationsToGenerate)
                    {
                        return recommendations;
                    }
                }
            }

            var userSavedRecipes = SavedRecipeAccessor.GetSavedRecipesForUser(userId);

            likedTags = GetLikedTagsFromSaving(userSavedRecipes);

            foreach (var tag in likedTags)
            {
                //get recipes with liked tags that have not been rated or saved by user
                var recipesWithLikedTags = RecipeTagAccessor.GetRecipesForTag(tag.Tag_ID).Where(r => !idsToExclude.Contains(r.Recipe_ID));
                recommendations.AddRange(recipesWithLikedTags);
            }

            if (recommendations.Count > numberOfRecommendationsToGenerate)
            {
                return recommendations;
            }

            likedCategories = GetLikedCategoriesFromSaving(userSavedRecipes);

            foreach (var category in likedCategories)
            {
                //get recipes with liked categories that have not been rated or saved by user
                var recipesWithLikedTags = RecipeCategoryAccessor.GetRecipesForCategory(category.Category_ID).Where(r => !idsToExclude.Contains(r.Recipe_ID));
                recommendations.AddRange(recipesWithLikedTags);
            }

            if (recommendations.Count > numberOfRecommendationsToGenerate)
            {
                return recommendations;
            }

            return recommendations;
        }

        public List<Tag> GetLikedTagsFromRatings(List<Rating> userRatings)
        {
            List<Tag> likedTags = new List<Tag>();

            foreach(var userRating in userRatings)
            {
                //Get tags of rated recipe
                var recipeTags = userRating.Recipe.RecipeTags.Select(rt => rt.Tag).ToList();
                likedTags.AddRange(recipeTags);
            }
            return likedTags;
        }

        public List<Category> GetLikedCategoriesFromRatings(List<Rating> userRatings)
        {
            List<Category> likedCategories = new List<Category>();

            foreach (var userRating in userRatings)
            {
                //Get categories of rated recipe
                var recipeCategories = userRating.Recipe.RecipeCategories.Select(rt => rt.Category).ToList();
                likedCategories.AddRange(recipeCategories);
            }
            return likedCategories;
        }

        public List<Tag> GetLikedTagsFromSaving(List<SavedRecipe> savedRecipes)
        {
            List<Tag> likedTags = new List<Tag>();

            foreach (var savedRecipe in savedRecipes)
            {
                //Get tags of rated recipe
                var recipeTags = savedRecipe.Recipe.RecipeTags.Select(rt => rt.Tag).ToList();
                likedTags.AddRange(recipeTags);
            }
            return likedTags;
        }

        public List<Category> GetLikedCategoriesFromSaving(List<SavedRecipe> savedRecipes)
        {
            List<Category> likedCategories = new List<Category>();

            foreach (var savedRecipe in savedRecipes)
            {
                //Get categories of rated recipe
                var recipeCategories = savedRecipe.Recipe.RecipeCategories.Select(rt => rt.Category).ToList();
                likedCategories.AddRange(recipeCategories);
            }
            return likedCategories;
        }
    }
}
