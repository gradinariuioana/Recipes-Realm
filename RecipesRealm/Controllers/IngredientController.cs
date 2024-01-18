using Microsoft.AspNetCore.Http;
using ModelsLibrary;
using RecipesRealm.ViewModels;
using System;
using System.Web.Mvc;
using BusinessLayer.Interfaces;

namespace RecipesRealm.Controllers {
    public class IngredientController : BaseController {

        public IngredientController(IRecipeRecommendationEngine recipeRecommendationEngine) : base(recipeRecommendationEngine) {
        }

        public ActionResult Create() {
            try {
                var ingredientVM = new IngredientViewModel();

                return View(ingredientVM);
            }
            catch (Exception ex) {
                logger.Error(ex, "Error on getting Ingredient view model for Create");
                return new HttpStatusCodeResult(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }

        [HttpPost]
        public ActionResult Create(IngredientViewModel model) {
            try {
                if (ModelState.IsValid) {

                    if (recipeRecommendationEngine.CheckIngredientExists(model.Ingredient_Name)) {
                        ViewBag.Warning = "This Ingredient already exists in the DataBase";
                        return View(model);
                    }

                    Ingredient ingredient = new Ingredient {
                        Ingredient_Name = model.Ingredient_Name
                    };

                    var ingId = recipeRecommendationEngine.AddIngredient(ingredient);
                    model.Ingredient_ID = ingId;

                    return View(model);
                }

                ViewBag.Warning = "Could not create Ingredient";
                return View(model);
            }
            catch (Exception ex) {
                logger.Error(ex, "Error on Creating Ingredient");
                return new HttpStatusCodeResult(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }
    }
}