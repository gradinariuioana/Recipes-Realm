using AutoMapper;
using Microsoft.AspNetCore.Http;
using ModelsLibrary;
using RecipesRealm.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DataAccess;

namespace RecipesRealm.Controllers
{
    public class RecipeController : BaseController { 

        public ActionResult Index()
        {
            IEnumerable<RecipeViewModel> recipeViewModels = new List<RecipeViewModel>();
            IEnumerable<Recipe> recipes = RecipeAccessor.GetRecipesList();

            foreach (Recipe r in recipes)
                recipeViewModels = recipeViewModels.Append(AutoMapperConfig.Mapper.Map<RecipeViewModel>(r));

            return View(recipeViewModels);
        }

        #region Create

        public ActionResult Create()
        {
            var recipeViewModel = new RecipeViewModel();

            //all categories
            ICollection<CategoryViewModel> recipeAllCategories = new List<CategoryViewModel>();
            var allCategs = CategoryAccessor.GetCategoriesList();
            foreach (var categ in allCategs)
                recipeAllCategories.Add(AutoMapperConfig.Mapper.Map<CategoryViewModel>(categ));
            recipeViewModel.AllCategories = recipeAllCategories;

            //all tags
            ICollection<TagViewModel> recipeAllTags = new List<TagViewModel>();
            var allTags = TagAccessor.GetTagsList();
            foreach (var tag in allTags)
                recipeAllTags.Add(AutoMapperConfig.Mapper.Map<TagViewModel>(tag));
            recipeViewModel.AllTags = recipeAllTags;

            //all nutrition elements
            ICollection<NutritionElementViewModel> recipeAllNutritionElements = new List<NutritionElementViewModel>();
            var allNElems = NutritionElementAccessor.GetNutritionElementsList();
            foreach (var nElem in allNElems)
                recipeAllNutritionElements.Add(AutoMapperConfig.Mapper.Map<NutritionElementViewModel>(nElem));
            recipeViewModel.AllNutritionElements = recipeAllNutritionElements;

            //all ingredients
            ICollection<IngredientViewModel> recipeAllIngredients = new List<IngredientViewModel>();
            var allIngreds = IngredientAccessor.GetIngredientsList();
            foreach (var ingred in allIngreds)
                recipeAllIngredients.Add(AutoMapperConfig.Mapper.Map<IngredientViewModel>(ingred));
            recipeViewModel.AllIngredients = recipeAllIngredients;

            return View(recipeViewModel);
        }

        [HttpPost]
        public ActionResult Create(RecipeViewModel model)
        {
            try
            {
                if (ModelState.IsValid) {

                    Recipe recipe = new Recipe {
                        Recipe_Name = model.Recipe_Name,
                        Recipe_Description = model.Recipe_Description,
                        Cooking_Time = model.Cooking_Time,
                        Difficulty_Level = model.Difficulty_Level,
                        Creation_Date = DateTime.Now,
                        Servings = model.Servings,
                        Picture_Path = model.Picture_Path,
                        Author_User_ID = UserAccessor.GetUserIdByName(model.Author_Name)
                    };

                    var rId = RecipeAccessor.AddRecipe(recipe);

                    //add Ingredients
                    foreach(var ing in model.RecipeIngredients) {
                        var ingredientExists = IngredientAccessor.CheckIngredientExists(ing.Ingredient_ID);
                        long ingId;

                        if (ingredientExists) {                           
                            ingId = ing.Ingredient_ID;
                        }
                        else {
                            var newIng = new Ingredient {
                                Ingredient_Name = ing.Ingredient_Name
                            };

                            ingId = IngredientAccessor.AddIngredient(newIng);
                        }

                        var recipeIng = new RecipeIngredient {
                            Recipe_ID = rId,
                            Ingredient_ID = ingId,
                            Other_Info = ing.Other_Info,
                            Measurement_Unit = ing.Measurement_Unit,
                            Quantity = ing.Quantity,
                            IsOptional = ing.IsOptional
                        };

                        RecipeIngredientAccessor.AddRecipeIngredient(recipeIng);
                    }

                    //add Steps
                    foreach (var step in model.RecipeSteps) {                     

                        var recipeStep = new RecipeStep {
                            Recipe_ID = rId,
                            Step_Description = step.Step_Description,
                            Step_Number = step.Step_Number,
                            Step_Title = step.Step_Title,
                            Picture_Path = step.Picture_Path,
                            IsOptional = step.IsOptional
                        };

                        RecipeStepAccessor.AddRecipeStep(recipeStep);
                    }

                    //add Tags
                    foreach (var tag in model.RecipeTags) {
                        var tagExists = TagAccessor.CheckTagExists(tag.Tag_Name);
                        long tagId;

                        if (tagExists) {
                            tagId = tag.Tag_ID;
                        }
                        else {
                            var newTag = new Tag {
                                Tag_Name = tag.Tag_Name
                            };

                            tagId = TagAccessor.AddTag(newTag);
                        }

                        var recipeTag = new RecipeTag {
                            Recipe_ID = rId,
                            Tag_ID = tagId
                        };

                        RecipeTagAccessor.AddRecipeTag(recipeTag);
                    }

                    //add Categories
                    foreach (var category in model.RecipeCategories) {
                        long categoryId = category.Category_ID;
                        
                        var recipeCategory = new RecipeCategory {
                            Recipe_ID = rId,
                            Category_ID = categoryId
                        };

                        RecipeCategoryAccessor.AddRecipeCategory(recipeCategory);
                    }

                    //add Nutrition Elements

                    foreach (var nutritionElem in model.RecipeNutritionElements) {
                        long nutritionElemId = nutritionElem.ID;

                        var recipeNE = new RecipeNutritionElement {
                            Recipe_ID = rId,
                            NutritionElement_ID = nutritionElemId,
                            Measurement_Unit = nutritionElem.Measurement_Unit,
                            Value = nutritionElem.Value                            
                        };

                        RecipeNutritionElementAccessor.AddRecipeNutritionElement(recipeNE);
                    }

                    return RedirectToAction("Details", new { id = rId });
                }

                ViewBag.Warning = "Could not create Recipe";
                return View(model);
            }
            catch (Exception ex)
            {
                Utils.WriteToLog("Recipe", "Create", ex.ToString());
                return new HttpStatusCodeResult(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }

        #endregion Create

        #region Details

        public ActionResult Details(long id)
        {
            try
            {
                var recipe = RecipeAccessor.GetRecipe(id);
                RecipeViewModel recipeViewModel = new RecipeViewModel();

                if (recipe == null)
                {
                    ViewBag.Warning = "The recipe could not be found";
                    return View(recipeViewModel);
                }

                recipeViewModel = AutoMapperConfig.Mapper.Map<RecipeViewModel>(recipe);            

                return View(recipeViewModel);
            }
            catch (Exception ex)
            {
                Utils.WriteToLog("Recipe", "Details", ex.ToString());
                return new HttpStatusCodeResult(StatusCodes.Status500InternalServerError, ex.ToString());
            }

        }

        #endregion Details


        #region Edit

        public ActionResult Edit(long id)
        {
            try {
                var recipe = RecipeAccessor.GetRecipe(id);
                RecipeViewModel recipeViewModel = new RecipeViewModel();

                if (recipe == null)
                {
                    ViewBag.Warning = "The recipe could not be found";
                    return View(recipeViewModel);
                }

                recipeViewModel = AutoMapperConfig.Mapper.Map<RecipeViewModel>(recipe);

                //all categories
                ICollection<CategoryViewModel> recipeAllCategories = new List<CategoryViewModel>();
                var allCategs = CategoryAccessor.GetCategoriesList();
                foreach(var categ in allCategs)
                    recipeAllCategories.Add(AutoMapperConfig.Mapper.Map<CategoryViewModel>(categ));
                recipeViewModel.AllCategories = recipeAllCategories;

                //all tags
                ICollection<TagViewModel> recipeAllTags = new List<TagViewModel>();
                var allTags = TagAccessor.GetTagsList();
                foreach (var tag in allTags)
                    recipeAllTags.Add(AutoMapperConfig.Mapper.Map<TagViewModel>(tag));
                recipeViewModel.AllTags = recipeAllTags;

                //all nutrition elements
                ICollection<NutritionElementViewModel> recipeAllNutritionElements = new List<NutritionElementViewModel>(); 
                var allNElems = NutritionElementAccessor.GetNutritionElementsList();
                foreach (var nElem in allNElems)
                    recipeAllNutritionElements.Add(AutoMapperConfig.Mapper.Map<NutritionElementViewModel>(nElem));
                recipeViewModel.AllNutritionElements = recipeAllNutritionElements;

                //all ingredients
                ICollection<IngredientViewModel> recipeAllIngredients = new List<IngredientViewModel>();
                var allIngreds = IngredientAccessor.GetIngredientsList();
                foreach (var ingred in allIngreds)
                    recipeAllIngredients.Add(AutoMapperConfig.Mapper.Map<IngredientViewModel>(ingred));
                recipeViewModel.AllIngredients = recipeAllIngredients;

                return View(recipeViewModel);
            }
            catch (Exception ex) {
                Utils.WriteToLog("Recipe", "Edit", ex.ToString());
                return new HttpStatusCodeResult(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }

        /*
       // POST: Tag/Edit/id
       [HttpPost]
       public ActionResult Edit(long id, TagViewModel model)
       {
           try
           {
               if (ModelState.IsValid)
               {

                   var tagExistsInDB = TagAccessor.CheckTagExists(model.Tag_Name);

                   if (tagExistsInDB)
                   {
                       ViewBag.Warning = "There is already a tag with this name in the DataBase";
                       return View(model);
                   }

                   Tag newTag = new Tag
                   {
                       Tag_Name = model.Tag_Name,
                       Tag_ID = id
                   };

                   TagAccessor.EditTag(newTag);

                   return RedirectToAction("Details", new { id = id });
               }

               ViewBag.Warning = "Could not edit Tag";
               return View(model);
           }
           catch (Exception ex)
           {
               Utils.WriteToLog("Tags", "Edit", ex.ToString());
               return new HttpStatusCodeResult(StatusCodes.Status500InternalServerError, ex.ToString());
           }

       }*/

       #endregion Edit

       
       #region Delete

       public ActionResult Delete(long id)
       {
            try
            {
                var recipe = RecipeAccessor.GetRecipe(id);
                RecipeViewModel recipeViewModel = new RecipeViewModel();

                if (recipe == null)
                {
                    ViewBag.Warning = "The recipe could not be found";
                    return View(recipeViewModel);
                }

                recipeViewModel = AutoMapperConfig.Mapper.Map<RecipeViewModel>(recipe);

                return View(recipeViewModel);
            }
            catch (Exception ex)
            {
                Utils.WriteToLog("Recipe", "Delete", ex.ToString());
                return new HttpStatusCodeResult(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }

        
       [HttpPost]
       public ActionResult DeleteTag(RecipeViewModel model)
       {
           try
           {
                RecipeTagAccessor.DeleteAllTagsForRecipe(model.Recipe_ID);
                RecipeStepAccessor.DeleteAllStepsForRecipe(model.Recipe_ID);
                RecipeNutritionElementAccessor.DeleteAllNutritionElementsForRecipe(model.Recipe_ID);
                RecipeCategoryAccessor.DeleteAllCategoriesForRecipe(model.Recipe_ID);
                RecipeIngredientAccessor.DeleteAllIngredientsForRecipe(model.Recipe_ID);

                RecipeAccessor.DeleteRecipe(model.Recipe_ID);

                return RedirectToAction("Index");
           }
           catch (Exception ex)
           {
               Utils.WriteToLog("Recipe", "Delete", ex.ToString());
               return new HttpStatusCodeResult(StatusCodes.Status500InternalServerError, ex.ToString());
           }

       }

       #endregion Delete
 
    }
}