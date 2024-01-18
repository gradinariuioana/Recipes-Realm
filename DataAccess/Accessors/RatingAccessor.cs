using DatabaseRepository;
using ModelsLibrary;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DataAccess {
    public class RatingAccessor : IRatingAccessor {

        public RecipesRealmContext context;

        public RatingAccessor(IRecipesRealmContext db) {
            context = (RecipesRealmContext)db;
        }
        public long GetAverageRatingForRecipe(long recipeId) {
            var ratings = context.Ratings.Where(r => r.Recipe_ID == recipeId).Select(r => r.Rating_Value).ToList();

            return (long)(ratings.Count > 0 ? ratings.Average() : 0.0);
        }

        public List<Rating> GetRatingsForUser(long userId) {
            var ratings = context.Ratings.Where(r => r.User_ID == userId).Include("Recipe.RecipeTags.Tag").Include("Recipe.RecipeCategories.Category").ToList();

            return ratings;
        }

        public List<Rating> GetRatingsWithScoreForUser(long userId, int score) {
            var ratings = context.Ratings.Where(r => r.User_ID == userId && r.Rating_Value == score).Include("Recipe.RecipeTags.Tag").Include("Recipe.RecipeCategories.Category").ToList();

            return ratings;
        }

        public List<Tag> GetLikedTagsFromRatings(List<Rating> userRatings) {
            List<Tag> likedTags = new List<Tag>();

            foreach (var userRating in userRatings) {
                //Get tags of rated recipe
                var recipeTags = userRating.Recipe.RecipeTags.Select(rt => rt.Tag).ToList();
                likedTags.AddRange(recipeTags);
            }
            return likedTags;
        }

        public List<Category> GetLikedCategoriesFromRatings(List<Rating> userRatings) {
            List<Category> likedCategories = new List<Category>();

            foreach (var userRating in userRatings) {
                //Get categories of rated recipe
                var recipeCategories = userRating.Recipe.RecipeCategories.Select(rt => rt.Category).ToList();
                likedCategories.AddRange(recipeCategories);
            }
            return likedCategories;
        }

        public long AddRating(Rating rating) {
            context.Ratings.Add(rating);
            context.SaveChanges();

            return rating.Rating_ID;
        }

        public bool CheckUserCanRate(Rating rating) {
            var r = context.Ratings.FirstOrDefault(re => re.Recipe_ID == rating.Recipe_ID && re.User_ID == rating.User_ID);

            return r == null;
        }
    }

    public interface IRatingAccessor {
        long GetAverageRatingForRecipe(long recipeId);

        List<Rating> GetRatingsForUser(long userId);

        List<Rating> GetRatingsWithScoreForUser(long userId, int score);

        List<Tag> GetLikedTagsFromRatings(List<Rating> userRatings);

        List<Category> GetLikedCategoriesFromRatings(List<Rating> userRatings);

        long AddRating(Rating rating);

        bool CheckUserCanRate(Rating rating);

    }
}
