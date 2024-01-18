using System.Collections.Generic;
using System.Linq;
using DatabaseRepository;
using ModelsLibrary;

namespace DataAccess {
    public class ReviewAccessor : IReviewAccessor {

        public RecipesRealmContext context;

        public ReviewAccessor(IRecipesRealmContext db) {
            context = (RecipesRealmContext)db;
        }

        public IEnumerable<Review> GetReviewsForRecipe(long recipeId) {
            var reviews = context.Reviews.Where(r => r.Recipe_ID == recipeId).ToList();

            return reviews;
        }
    }

    public interface IReviewAccessor {
        IEnumerable<Review> GetReviewsForRecipe(long recipeId);
    }
}
