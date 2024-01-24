using Microsoft.AspNetCore.Http;
using ModelsLibrary;
using RecipesRealm.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BusinessLayer.Interfaces;
using System;

namespace RecipesRealm.Controllers {
    public class RecommendationController : BaseController {

        public RecommendationController(IRecipeRecommendationEngine recipeRecommendationEngine) : base(recipeRecommendationEngine) {
        }

        public ActionResult Index(long? id) {
            try {
                IEnumerable<RecipeViewModel> recipeViewModels = new List<RecipeViewModel>();

                if (id.HasValue) {

                    IEnumerable<Recipe> recipes = recipeRecommendationEngine.GenerateRecommendations(id.Value);

                    foreach (Recipe r in recipes)
                        recipeViewModels = recipeViewModels.Append(AutoMapperConfig.Mapper.Map<RecipeViewModel>(r));

                }
                else {
                    IEnumerable<Recipe> recipes = recipeRecommendationEngine.GenerateRecommendationsNoUser(null);

                    foreach (Recipe r in recipes)
                        recipeViewModels = recipeViewModels.Append(AutoMapperConfig.Mapper.Map<RecipeViewModel>(r));
                }

                return View(recipeViewModels);
            }
            catch (Exception ex) {
                logger.Error(ex, "Error on Getting Recommendations");
                return new HttpStatusCodeResult(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }
    }
}