using System.Linq;

namespace DataAccess
{
    public class RatingAccessor
    {
        public static long GetAverageRatingForRecipe(long recipeId) {
            using (var context = new DatabaseRepository.RecipesRealmContext()) {
                var ratings = context.Ratings.Where(r => r.Recipe_ID == recipeId).Select(r => r.Rating_Value).ToList();

                return (long)(ratings.Count > 0 ? ratings.Average() : 0.0); ;
            }
        }
    }
}
