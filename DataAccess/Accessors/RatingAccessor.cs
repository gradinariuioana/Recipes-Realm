using ModelsLibrary;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DataAccess
{
    public class RatingAccessor
    {
        public static long GetAverageRatingForRecipe(long recipeId) {
            using (var context = new DatabaseRepository.RecipesRealmContext()) {
                var ratings = context.Ratings.Where(r => r.Recipe_ID == recipeId).Select(r => r.Rating_Value).ToList();

                return (long)(ratings.Count > 0 ? ratings.Average() : 0.0);
            }
        }

        public static List<Rating> GetRatingsForUser (long userId)
        {
            using (var context = new DatabaseRepository.RecipesRealmContext())
            {
                var ratings = context.Ratings.Where(r => r.User_ID == userId).Include("Recipe.RecipeTags.Tag").Include("Recipe.RecipeCategories.Category").ToList();

                return ratings;
            }
        }

        public static List<Rating> GetRatingsWithScoreForUser(long userId, int score)
        {
            using (var context = new DatabaseRepository.RecipesRealmContext())
            {
                var ratings = context.Ratings.Where(r => r.User_ID == userId && r.Rating_Value == score).Include("Recipe.RecipeTags.Tag").Include("Recipe.RecipeCategories.Category").ToList();

                return ratings;
            }            
        }
    }
}
