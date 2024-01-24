using Microsoft.AspNetCore.Http;
using ModelsLibrary;
using RecipesRealm.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BusinessLayer.Interfaces;
using System.IO;
using System.Web.Hosting;
using System.Web;

namespace RecipesRealm.Controllers {
    public class RecipeController : BaseController {

        public RecipeController(IRecipeRecommendationEngine recipeRecommendationEngine) : base(recipeRecommendationEngine) {
        }

        public ActionResult Index() {
            try {
                IEnumerable<RecipeViewModel> recipeViewModels = new List<RecipeViewModel>();
                IEnumerable<Recipe> recipes = recipeRecommendationEngine.GetRecipesList();

                if (recipes is null) {
                    logger.Warn("Recipes List Empty");
                }

                foreach (Recipe r in recipes)
                    recipeViewModels = recipeViewModels.Append(AutoMapperConfig.Mapper.Map<RecipeViewModel>(r));

                return View(recipeViewModels);
            }
            catch (Exception ex) {
                logger.Error(ex, "Error on Getting Recipes");
                return new HttpStatusCodeResult(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }

        #region Create

        public ActionResult Create() {
            try {
                RecipeViewModel recipeViewModel = new RecipeViewModel();

                //all categories
                ICollection<CategoryViewModel> recipeAllCategories = new List<CategoryViewModel>();
                var allCategs = recipeRecommendationEngine.GetCategoriesList();

                foreach (var categ in allCategs)
                    recipeAllCategories.Add(AutoMapperConfig.Mapper.Map<CategoryViewModel>(categ));

                recipeViewModel.AllCategories = recipeAllCategories;

                //all tags
                ICollection<TagViewModel> recipeAllTags = new List<TagViewModel>();
                var allTags = recipeRecommendationEngine.GetTagsList();

                foreach (var tag in allTags)
                    recipeAllTags.Add(AutoMapperConfig.Mapper.Map<TagViewModel>(tag));

                recipeViewModel.AllTags = recipeAllTags;

                //all nutrition elements
                ICollection<NutritionElementViewModel> recipeAllNutritionElements = new List<NutritionElementViewModel>();
                var allNElems = recipeRecommendationEngine.GetNutritionElementsList();

                foreach (var nElem in allNElems)
                    recipeAllNutritionElements.Add(AutoMapperConfig.Mapper.Map<NutritionElementViewModel>(nElem));

                recipeViewModel.AllNutritionElements = recipeAllNutritionElements;

                //all ingredients
                ICollection<IngredientViewModel> recipeAllIngredients = new List<IngredientViewModel>();
                var allIngreds = recipeRecommendationEngine.GetIngredientsList();

                foreach (var ingred in allIngreds)
                    recipeAllIngredients.Add(AutoMapperConfig.Mapper.Map<IngredientViewModel>(ingred));

                recipeViewModel.AllIngredients = recipeAllIngredients;

                return View(recipeViewModel);
            }
            catch (Exception ex) {
                logger.Error(ex, "Error on Getting Recipe Model for Creation");
                return new HttpStatusCodeResult(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }

        [Microsoft.AspNetCore.Mvc.Produces("application/json")]
        public string UploadPhoto(HttpPostedFileBase file) {
            try {
                if (file != null) {
                    var path = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "Content/Images", file.FileName);
                    file.SaveAs(path);

                    return "/Content/Images/" + file.FileName;
                }
                return "";
            }
            catch (Exception ex) {
                logger.Error(ex, "Error on Uploading Recipe file");
                return ex.ToString();
            }
        }

        [HttpPost]
        public ActionResult Create(RecipeViewModel model) {
            try {
                if (ModelState.IsValid) {
                    Recipe recipe = AutoMapperConfig.Mapper.Map<Recipe>(model);

                    recipe.Is_Active = true;
                    recipe.Creation_Date = DateTime.Now;

                    ICollection<RecipeTag> recipeTags = new List<RecipeTag>();

                    foreach (var tagId in model.RecipeTagsIds) {
                        var recipeTag = new RecipeTag {
                            Recipe_ID = 0,
                            Tag_ID = tagId
                        };

                        recipeTags.Add(recipeTag);
                    }

                    recipe.RecipeTags = recipeTags;

                    ICollection<RecipeCategory> recipeCategories = new List<RecipeCategory>();

                    foreach (var catId in model.RecipeCategoriesIds) {
                        var recipeCateg = new RecipeCategory {
                            Recipe_ID = 0,
                            Category_ID = catId
                        };

                        recipeCategories.Add(recipeCateg);
                    }

                    recipe.RecipeCategories = recipeCategories;

                    var id = recipeRecommendationEngine.AddRecipe(recipe);

                    return RedirectToAction("Details", new { id });
                }

                ViewBag.Warning = "Could not Create Recipe";
                return View();
            }
            catch (Exception ex) {
                logger.Error(ex, "Error on Creating Recipe");
                return new HttpStatusCodeResult(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }

        #endregion Create

        #region Details

        public ActionResult Details(long id) {
            try {
                var recipe = recipeRecommendationEngine.GetRecipe(id);
                RecipeViewModel recipeViewModel = new RecipeViewModel();

                if (recipe == null) {
                    ViewBag.Warning = "The recipe could not be found";
                    return View(recipeViewModel);
                }

                recipeViewModel = AutoMapperConfig.Mapper.Map<RecipeViewModel>(recipe);

                recipeViewModel.Author_Name = recipeRecommendationEngine.GetUserName(recipe.Author_User_ID);
                recipeViewModel.AverageRating = recipeRecommendationEngine.GetAverageRatingForRecipe(id);


                return View(recipeViewModel);
            }
            catch (Exception ex) {
                logger.Error(ex, "Error on getting Recipe Details");
                return new HttpStatusCodeResult(StatusCodes.Status500InternalServerError, ex.ToString());
            }

        }

        #endregion Details


        #region Edit

        public ActionResult Edit(long id) {
            try {
                var recipe = recipeRecommendationEngine.GetRecipe(id);
                RecipeViewModel recipeViewModel = new RecipeViewModel();

                if (recipe == null) {
                    ViewBag.Warning = "The recipe could not be found";
                    return View(recipeViewModel);
                }

                recipeViewModel = AutoMapperConfig.Mapper.Map<RecipeViewModel>(recipe);

                recipeViewModel.Author_Name = recipeRecommendationEngine.GetUserName(recipe.Author_User_ID);
                recipeViewModel.AverageRating = recipeRecommendationEngine.GetAverageRatingForRecipe(id);
                recipeViewModel.RecipeCategoriesIds = recipeRecommendationEngine.GetCategoriesIdsForRecipe(id);
                recipeViewModel.RecipeTagsIds = recipeRecommendationEngine.GetTagsIdsForRecipe(id);

                //all categories
                ICollection<CategoryViewModel> recipeAllCategories = new List<CategoryViewModel>();
                var allCategs = recipeRecommendationEngine.GetCategoriesList();
                foreach (var categ in allCategs)
                    recipeAllCategories.Add(AutoMapperConfig.Mapper.Map<CategoryViewModel>(categ));
                recipeViewModel.AllCategories = recipeAllCategories;

                //all tags
                ICollection<TagViewModel> recipeAllTags = new List<TagViewModel>();
                var allTags = recipeRecommendationEngine.GetTagsList();
                foreach (var tag in allTags)
                    recipeAllTags.Add(AutoMapperConfig.Mapper.Map<TagViewModel>(tag));
                recipeViewModel.AllTags = recipeAllTags;

                //all nutrition elements
                ICollection<NutritionElementViewModel> recipeAllNutritionElements = new List<NutritionElementViewModel>();
                var allNElems = recipeRecommendationEngine.GetNutritionElementsList();
                foreach (var nElem in allNElems)
                    recipeAllNutritionElements.Add(AutoMapperConfig.Mapper.Map<NutritionElementViewModel>(nElem));
                recipeViewModel.AllNutritionElements = recipeAllNutritionElements;

                //all ingredients
                ICollection<IngredientViewModel> recipeAllIngredients = new List<IngredientViewModel>();
                var allIngreds = recipeRecommendationEngine.GetIngredientsList();
                foreach (var ingred in allIngreds)
                    recipeAllIngredients.Add(AutoMapperConfig.Mapper.Map<IngredientViewModel>(ingred));
                recipeViewModel.AllIngredients = recipeAllIngredients;

                return View(recipeViewModel);
            }
            catch (Exception ex) {
                logger.Error(ex, "Error on getting Recipe for Edit");
                return new HttpStatusCodeResult(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }

        [HttpPost]
        public ActionResult Edit(long id, RecipeViewModel model) {
            try {
                if (ModelState.IsValid) {
                    model.Recipe_ID = id;
                    Recipe recipe = AutoMapperConfig.Mapper.Map<Recipe>(model);
                    recipeRecommendationEngine.UpdateRecipe(recipe);

                    //update tags
                    recipeRecommendationEngine.DeleteAllTagsForRecipe(id);

                    if (model.RecipeTagsIds != null) {
                        foreach (var tagId in model.RecipeTagsIds) {
                            var recipeTag = new RecipeTag {
                                Recipe_ID = id,
                                Tag_ID = tagId
                            };

                            recipeRecommendationEngine.AddRecipeTag(recipeTag);
                        }
                    }

                    //update categories
                    recipeRecommendationEngine.DeleteAllCategoriesForRecipe(id);

                    if (model.RecipeCategoriesIds != null) {
                        foreach (var categoryId in model.RecipeCategoriesIds) {
                            var recipeCategory = new RecipeCategory {
                                Recipe_ID = id,
                                Category_ID = categoryId
                            };

                            recipeRecommendationEngine.AddRecipeCategory(recipeCategory);
                        }
                    }

                    //update nutritionElements

                    recipeRecommendationEngine.DeleteAllNutritionElementsForRecipe(id);
                    if (model.RecipeNutritionElements != null) {
                        foreach (var ne in model.RecipeNutritionElements) {
                            var recipeNE = new RecipeNutritionElement {
                                Recipe_ID = id,
                                NutritionElement_ID = ne.NutritionElement_ID,
                                Value = ne.Value,
                                Measurement_Unit = ne.Measurement_Unit
                            };

                            recipeRecommendationEngine.AddRecipeNutritionElement(recipeNE);
                        }
                    }


                    //update ingredients
                    recipeRecommendationEngine.DeleteAllIngredientsForRecipe(id);
                    foreach (var ing in model.RecipeIngredients) {
                        var recipeIng = new RecipeIngredient {
                            Recipe_ID = id,
                            Ingredient_ID = ing.Ingredient_ID,
                            Other_Info = ing.Other_Info,
                            Measurement_Unit = ing.Measurement_Unit,
                            Quantity = ing.Quantity.HasValue ? ing.Quantity.Value : 0,
                            IsOptional = ing.IsOptional
                        };

                        recipeRecommendationEngine.AddRecipeIngredient(recipeIng);
                    }

                    //update steps
                    recipeRecommendationEngine.DeleteAllStepsForRecipe(id);
                    foreach (var step in model.RecipeSteps) {
                        var recipeStep = new RecipeStep {
                            Recipe_ID = id,
                            Step_Number = step.Step_Number,
                            Step_Title = step.Step_Title,
                            Step_Description = step.Step_Description,
                            IsOptional = step.IsOptional
                        };

                        recipeRecommendationEngine.AddRecipeStep(recipeStep);
                    }

                    return RedirectToAction("Edit", new { id });
                }

                ViewBag.Warning = "Could not Edit Recipe";
                return View(id);
            }
            catch (Exception ex) {
                logger.Error(ex, "Error on Editing Recipe");
                return new HttpStatusCodeResult(StatusCodes.Status500InternalServerError, ex.ToString());
            }

        }

        #endregion Edit


        #region Delete

        public ActionResult Delete(long id) {
            try {
                var recipe = recipeRecommendationEngine.GetRecipe(id);
                RecipeViewModel recipeViewModel = new RecipeViewModel();

                if (recipe == null) {
                    ViewBag.Warning = "The recipe could not be found";
                    return View(recipeViewModel);
                }

                recipeViewModel = AutoMapperConfig.Mapper.Map<RecipeViewModel>(recipe);

                recipeViewModel.Author_Name = recipeRecommendationEngine.GetUserName(recipe.Author_User_ID);
                recipeViewModel.AverageRating = recipeRecommendationEngine.GetAverageRatingForRecipe(id);

                return View(recipeViewModel);
            }
            catch (Exception ex) {
                logger.Error(ex, "Error on getting Recipe for Delete");
                return new HttpStatusCodeResult(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }

        [ActionName("Delete")]
        [HttpPost]

        public ActionResult DeleteRecipe(long id) {
            try {
                recipeRecommendationEngine.DeleteAllTagsForRecipe(id);
                recipeRecommendationEngine.DeleteAllStepsForRecipe(id);
                recipeRecommendationEngine.DeleteAllNutritionElementsForRecipe(id);
                recipeRecommendationEngine.DeleteAllCategoriesForRecipe(id);
                recipeRecommendationEngine.DeleteAllIngredientsForRecipe(id);

                recipeRecommendationEngine.DeleteRecipe(id);

                return RedirectToAction("Index");
            }
            catch (Exception ex) {
                logger.Error(ex, "Error on Deleting Recipe");
                return new HttpStatusCodeResult(StatusCodes.Status500InternalServerError, ex.ToString());
            }

        }

        [HttpPost]

        public ActionResult Deactivate(long id) {
            try {
                recipeRecommendationEngine.DeactivateRecipe(id);

                return RedirectToAction("Index");
            }
            catch (Exception ex) {
                logger.Error(ex, "Error on Deactivating Recipe");
                return new HttpStatusCodeResult(StatusCodes.Status500InternalServerError, ex.ToString());
            }

        }

        #endregion Delete

        [HttpPost]

        public ActionResult Reactivate(long id) {
            try {
                recipeRecommendationEngine.ReactivateRecipe(id);

                return RedirectToAction("Index");
            }
            catch (Exception ex) {
                logger.Error(ex, "Error on Reactivating Recipe");
                return new HttpStatusCodeResult(StatusCodes.Status500InternalServerError, ex.ToString());
            }

        }

    }
}