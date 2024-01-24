using RecipesRealm.ViewModels;
using System.Web.Mvc;
using BusinessLayer.Interfaces;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System;
using Microsoft.AspNetCore.Mvc;
using ModelsLibrary;
using Newtonsoft.Json;

namespace RecipesRealm.Controllers {
    public class RatingController : BaseController {

        public RatingController(IRecipeRecommendationEngine recipeRecommendationEngine) : base(recipeRecommendationEngine) {
        }

        [Produces("application/json")]
        public string AddRating(string modelString) {
            try {
                RatingViewModel ratingVM = JsonConvert.DeserializeObject<RatingViewModel>(modelString);
                Rating rating = AutoMapperConfig.Mapper.Map<Rating>(ratingVM);

                var ratId = recipeRecommendationEngine.AddRating(rating);

                return JsonConvert.SerializeObject(new { ratingId = ratId, averageRating = recipeRecommendationEngine.GetAverageRatingForRecipe(rating.Recipe_ID) });
            }
            catch (Exception ex) {
                logger.Error(ex, "Error on Adding Rating");
                return ex.ToString();
            }
        }

        [Produces("application/json")]
        public string CheckUserCanRateRecipe(string modelString) {
            try {
                RatingViewModel ratingVM = JsonConvert.DeserializeObject<RatingViewModel>(modelString);
                Rating rating = AutoMapperConfig.Mapper.Map<Rating>(ratingVM);

                var canRate = recipeRecommendationEngine.CheckUserCanRate(rating);

                return JsonConvert.SerializeObject(new { canRate });
            }
            catch (Exception ex) {
                logger.Error(ex, "Error on Checking if User can Rate Recipe");
                return ex.ToString();
            }
        }
    }
}