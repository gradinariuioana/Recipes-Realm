using Microsoft.AspNetCore.Http;
using ModelsLibrary;
using RecipesRealm.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BusinessLayer.Interfaces;

namespace RecipesRealm.Controllers {
    public class RecipeController : BaseController {

        public RecipeController(IRecipeRecommendationEngine recipeRecommendationEngine) : base(recipeRecommendationEngine) {
        }

        public ActionResult Index() {
            try {
                IEnumerable<RecipeViewModel> recipeViewModels = new List<RecipeViewModel>();
                IEnumerable<Recipe> recipes = recipeRecommendationEngine.GetRecipesList();

                if (recipes is null) {
                    logger.Error("Recipes could not be retrieved");
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

        [HttpPost]
        public ActionResult Create(RecipeViewModel model) {
            try {
                if (ModelState.IsValid) {
                    Recipe recipe = AutoMapperConfig.Mapper.Map<Recipe>(model);

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
                    var oldTagsIds = recipeRecommendationEngine.GetTagsIdsForRecipe(id);

                    //remove old tags that are not in the model anymore
                    var recipeTagsToRemove = oldTagsIds.Where(i => !model.RecipeTagsIds.Contains(i));
                    foreach (var tagId in recipeTagsToRemove) {
                        recipeRecommendationEngine.DeleteRecipeTag(tagId);
                    }

                    //add new tags that were not in the old model
                    var tagsToAdd = model.RecipeTagsIds.Where(i => !oldTagsIds.Contains(i));
                    foreach (var tagId in tagsToAdd) {
                        var recipeTag = new RecipeTag {
                            Recipe_ID = id,
                            Tag_ID = tagId
                        };

                        recipeRecommendationEngine.AddRecipeTag(recipeTag);
                    }

                    //update categories
                    var oldCategoriesIds = recipeRecommendationEngine.GetCategoriesIdsForRecipe(id);

                    //remove old categories that are not in the model anymore
                    var recipeCategoriesToRemove = oldCategoriesIds.Where(i => !model.RecipeCategoriesIds.Contains(i));
                    foreach (var categoryId in recipeCategoriesToRemove) {
                        recipeRecommendationEngine.DeleteRecipeCategory(categoryId);
                    }

                    //add new categories that were not in the old model
                    var categoriesToAdd = model.RecipeCategoriesIds.Where(i => !oldCategoriesIds.Contains(i));
                    foreach (var categoryId in categoriesToAdd) {
                        var recipeCategory = new RecipeCategory {
                            Recipe_ID = id,
                            Category_ID = categoryId
                        };

                        recipeRecommendationEngine.AddRecipeCategory(recipeCategory);
                    }

                    //update nutritionElements
                    recipeRecommendationEngine.DeleteAllNutritionElementsForRecipe(id);
                    foreach (var ne in model.RecipeNutritionElements) {
                        var recipeNE = new RecipeNutritionElement {
                            Recipe_ID = id,
                            NutritionElement_ID = ne.NutritionElement_ID,
                            Value = ne.Value,
                            Measurement_Unit = ne.Measurement_Unit
                        };

                        recipeRecommendationEngine.AddRecipeNutritionElement(recipeNE);
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

    }
}