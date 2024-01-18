using RecipesRealm.ViewModels;
using System.Web.Mvc;
using BusinessLayer.Interfaces;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System;

namespace RecipesRealm.Controllers {
    public class CategoryController : BaseController {

        public CategoryController(IRecipeRecommendationEngine recipeRecommendationEngine) : base(recipeRecommendationEngine) {
        }

        public ActionResult Details(long id) {
            try {
                var categ = recipeRecommendationEngine.GetCategoryById(id);

                var model = AutoMapperConfig.Mapper.Map<CategoryViewModel>(categ);
                var recipes = recipeRecommendationEngine.GetRecipes(id);
                ICollection<RecipeViewModel> recipesVMs = new List<RecipeViewModel>();

                foreach (var r in recipes) {
                    var rVM = AutoMapperConfig.Mapper.Map<RecipeViewModel>(r);

                    recipesVMs.Add(rVM);
                }

                model.Recipes = recipesVMs;

                return View(model);
            }
            catch (Exception ex) {
                logger.Error(ex, "Error on getting Category Details");
                return new HttpStatusCodeResult(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }
    }
}