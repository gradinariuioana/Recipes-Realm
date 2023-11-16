using Microsoft.AspNetCore.Http;
using ModelsLibrary;
using RecipesRealm.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace RecipesRealm.Controllers
{
    public class RecipeController : Controller
    {
        public ActionResult Index()
        {
            IEnumerable<TagViewModel> tagViewModels = new List<TagViewModel>();
            IEnumerable<Tag> tags = DataAccess.TagAccessor.GetTagsList();

            foreach (Tag tag in tags)
            {
                var tagViewModel = new TagViewModel
                {
                    Tag_Name = tag.Tag_Name,
                    Tag_Recipes = string.Join(", ", DataAccess.RecipeTagAccessor.GetRecipesForTag(tag.Tag_ID).Select(r => r.Recipe_Name).ToList())
                };

                tagViewModels = tagViewModels.Append(tagViewModel);
            }

            return View(tagViewModels);
        }

        #region Create

        // GET: Tag/Create
        public ActionResult Create()
        {
            var model = new RecipeViewModel();

            //Get Ingredients for Select

            var ingredients = DataAccess.IngredientAccessor.GetIngredientsList();
            ICollection<IngredientViewModel> ingVMs = new List<IngredientViewModel>();

            foreach(var ing in ingredients) {
                var ingVM = new IngredientViewModel {
                    Ingredient_ID = ing.Ingredient_ID,
                    Ingredient_Name = ing.Ingredient_Name,
                    Category = ing.Category
                };

                ingVMs.Add(ingVM);
            }

            model.RecipeIngredients = ingVMs;

            //Get Tags for Select

            var tags = DataAccess.TagAccessor.GetTagsList();
            ICollection<TagViewModel> tagVMs = new List<TagViewModel>();

            foreach (var tag in tags) {
                var tagVM = new TagViewModel {
                    Tag_ID = tag.Tag_ID,
                    Tag_Name = tag.Tag_Name
                };

                tagVMs.Add(tagVM);
            }

            model.RecipeTags = tagVMs;

            //Get Categories for Select

            var categs = DataAccess.CategoryAccessor.GetCategoriesList();
            ICollection<CategoryViewModel> categoryVMs = new List<CategoryViewModel>();

            foreach (var category in categs) {
                var categoryVM = new CategoryViewModel {
                    Category_ID = category.Category_ID,
                    Category_Name = category.Category_Name
                };

                categoryVMs.Add(categoryVM);
            }

            model.RecipeCategories = categoryVMs;

            //Get Nutrition Elements for Select

            var elems = DataAccess.NutritionElementAccessor.GetNutritionElementsList();
            ICollection<NutritionElementViewModel> nutritionElementVMs = new List<NutritionElementViewModel>();

            foreach (var nutritionElement in elems) {
                var nutritionElementVM = new NutritionElementViewModel {
                    Element_Name = nutritionElement.Element_Name,
                    Element_Description = nutritionElement.Element_Description
                };

                nutritionElementVMs.Add(nutritionElementVM);
            }

            model.RecipeNutritionElements = nutritionElementVMs;

            return View(model);
        }

        // POST: Tag/Create/
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
                        Author_User_ID = DataAccess.UserAccessor.GetUserIdByName(model.Author_Name)
                    };

                    var rId = DataAccess.RecipeAccessor.AddRecipe(recipe);

                    //add Ingredients
                    foreach(var ing in model.RecipeIngredients) {
                        var ingredientExists = DataAccess.IngredientAccessor.CheckIngredientExists(ing.Ingredient_ID);
                        long ingId;

                        if (ingredientExists) {                           
                            ingId = ing.Ingredient_ID;
                        }
                        else {
                            var newIng = new Ingredient {
                                Ingredient_Name = ing.Ingredient_Name
                            };

                            ingId = DataAccess.IngredientAccessor.AddIngredient(newIng);
                        }

                        var recipeIng = new RecipeIngredient {
                            Recipe_ID = rId,
                            Ingredient_ID = ingId,
                            Other_Info = ing.Other_Info,
                            Measurement_Unit = ing.Measurement_Unit,
                            Quantity = ing.Quantity,
                            IsOptional = ing.IsOptional
                        };

                        DataAccess.RecipeIngredientAccessor.AddRecipeIngredient(recipeIng);
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

                        DataAccess.RecipeStepAccessor.AddRecipeStep(recipeStep);
                    }

                    //add Tags
                    foreach (var tag in model.RecipeTags) {
                        var tagExists = DataAccess.TagAccessor.CheckTagExists(tag.Tag_Name);
                        long tagId;

                        if (tagExists) {
                            tagId = tag.Tag_ID;
                        }
                        else {
                            var newTag = new Tag {
                                Tag_Name = tag.Tag_Name
                            };

                            tagId = DataAccess.TagAccessor.AddTag(newTag);
                        }

                        var recipeTag = new RecipeTag {
                            Recipe_ID = rId,
                            Tag_ID = tagId
                        };

                        DataAccess.RecipeTagAccessor.AddRecipeTag(recipeTag);
                    }

                    //add Categories
                    foreach (var category in model.RecipeCategories) {
                        long categoryId = category.Category_ID;
                        
                        var recipeCategory = new RecipeCategory {
                            Recipe_ID = rId,
                            Category_ID = categoryId
                        };

                        DataAccess.RecipeCategoryAccessor.AddRecipeCategory(recipeCategory);
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

                        DataAccess.RecipeNutritionElementAccessor.AddRecipeNutritionElement(recipeNE);
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

        // GET: Recipe/Details/id
        public ActionResult Details(long id)
        {
            try
            {
                var recipe = DataAccess.RecipeAccessor.GetRecipe(id);
                RecipeViewModel recipeViewModel = new RecipeViewModel();

                if (recipe == null)
                {
                    ViewBag.Warning = "The recipe could not be found";
                    return View(recipeViewModel);
                }

                recipeViewModel.Author_Name = DataAccess.UserAccessor.GetUserName(recipe.Author_User_ID);
                recipeViewModel.Recipe_Name = recipe.Recipe_Name;
                recipeViewModel.Recipe_Description = recipe.Recipe_Description;
                recipeViewModel.Cooking_Time = recipe.Cooking_Time;
                recipeViewModel.Difficulty_Level = recipe.Difficulty_Level;
                recipeViewModel.Servings = recipe.Servings;
                recipeViewModel.Picture_Path = recipe.Picture_Path;
                recipeViewModel.AverageRating = DataAccess.RatingAccessor.GetAverageRatingForRecipe(recipe.Recipe_ID);

                //categories
                ICollection<CategoryViewModel> recipeCategories = new List<CategoryViewModel>();

                var categs = DataAccess.RecipeCategoryAccessor.GetCategoriesForRecipe(id);
                foreach (var categ in categs) {
                    var categViewModel = new CategoryViewModel {
                        Category_ID = categ.Category_ID,
                        Category_Name = categ.Category_Name,
                        Category_Description = categ.Category_Description
                    };
                    recipeCategories.Add(categViewModel);
                }
                recipeViewModel.RecipeCategories = recipeCategories;

                //tags
                ICollection<TagViewModel> recipeTags = new List<TagViewModel>();

                var tags = DataAccess.RecipeTagAccessor.GetTagsForRecipe(id);
                foreach (var tag in tags) {
                    var tagViewModel = new TagViewModel {
                        Tag_ID = tag.Tag_ID,
                        Tag_Name = tag.Tag_Name
                    };
                    recipeTags.Add(tagViewModel);
                }
                recipeViewModel.RecipeTags = recipeTags;
                
                //nutrition elements
                ICollection<NutritionElementViewModel> recipeNutritionElements = new List<NutritionElementViewModel>();
                var nElems = DataAccess.RecipeNutritionElementAccessor.GetNutritionElementsForRecipe(id);
                foreach (var nElem in nElems)
                {
                    var neViewModel = new NutritionElementViewModel
                    {
                        Element_Name = nElem.NutritionElement.Element_Name,
                        Element_Description = nElem.NutritionElement.Element_Description,
                        Measurement_Unit = nElem.Measurement_Unit,
                        Value = nElem.Value
                    };
                    recipeNutritionElements.Add(neViewModel);
                }
                recipeViewModel.RecipeNutritionElements = recipeNutritionElements;

                //ingredients
                ICollection<IngredientViewModel> recipeIngredients = new List<IngredientViewModel>();
                var ingreds = DataAccess.RecipeIngredientAccessor.GetIngredientsForRecipe(id);
                foreach (var ingred in ingreds)
                {
                    var ingr = new IngredientViewModel
                    {
                        Ingredient_Name = ingred.Ingredient.Ingredient_Name,
                        Quantity = ingred.Quantity,
                        Measurement_Unit = ingred.Measurement_Unit,
                        IsOptional = ingred.IsOptional,
                        Other_Info = ingred.Other_Info,
                        Category = ingred.Ingredient.Category
                    };
                    recipeIngredients.Add(ingr);
                }
                recipeViewModel.RecipeIngredients = recipeIngredients;

                //steps
                ICollection<RecipeStepViewModel> recipeSteps = new List<RecipeStepViewModel>();
                var steps = DataAccess.RecipeStepAccessor.GetStepsForRecipe(id);
                foreach (var step in steps)
                {
                    var stepVM = new RecipeStepViewModel
                    {
                        Step_Number = step.Step_Number,
                        Step_Title = step.Step_Title,
                        Step_Description = step.Step_Description,
                        IsOptional = step.IsOptional
                    };
                    recipeSteps.Add(stepVM);
                }
                recipeViewModel.RecipeSteps = recipeSteps;

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

        // GET: Tag/Edit/id
        public ActionResult Edit(long id)
        {
            try {
                var recipe = DataAccess.RecipeAccessor.GetRecipe(id);
                RecipeViewModel recipeViewModel = new RecipeViewModel();

                if (recipe == null) {
                    ViewBag.Warning = "The recipe could not be found";
                    return View(recipeViewModel);
                }

                recipeViewModel.Author_Name = DataAccess.UserAccessor.GetUserName(recipe.Author_User_ID);
                recipeViewModel.Recipe_Name = recipe.Recipe_Name;
                recipeViewModel.Recipe_Description = recipe.Recipe_Description;
                recipeViewModel.Cooking_Time = recipe.Cooking_Time;
                recipeViewModel.Difficulty_Level = recipe.Difficulty_Level;
                recipeViewModel.Servings = recipe.Servings;
                recipeViewModel.Picture_Path = recipe.Picture_Path;
                recipeViewModel.AverageRating = DataAccess.RatingAccessor.GetAverageRatingForRecipe(recipe.Recipe_ID);

                //categories
                recipeViewModel.RecipeCategoriesIds = DataAccess.RecipeCategoryAccessor.GetCategoriesIdsForRecipe(id);
                ICollection<CategoryViewModel> recipeAllCategories = new List<CategoryViewModel>();
                ICollection<CategoryViewModel> recipeCategories = new List<CategoryViewModel>();

                var categs = DataAccess.RecipeCategoryAccessor.GetCategoriesForRecipe(id);
                foreach (var categ in categs) {
                    var categViewModel = new CategoryViewModel {
                        Category_ID = categ.Category_ID,
                        Category_Name = categ.Category_Name,
                        Category_Description = categ.Category_Description
                    };
                    recipeCategories.Add(categViewModel);
                }
                recipeViewModel.RecipeCategories = recipeCategories;

                var allCategs = DataAccess.CategoryAccessor.GetCategoriesList();
                foreach(var categ in allCategs) {
                    var categViewModel = new CategoryViewModel {
                        Category_ID = categ.Category_ID,
                        Category_Name = categ.Category_Name,
                        Category_Description = categ.Category_Description
                    };
                    recipeAllCategories.Add(categViewModel);
                }

                recipeViewModel.AllCategories = recipeAllCategories;

                //tags
                recipeViewModel.RecipeTagsIds = DataAccess.RecipeTagAccessor.GetTagsIdsForRecipe(id);
                ICollection<TagViewModel> recipeAllTags = new List<TagViewModel>();

                var allTags = DataAccess.TagAccessor.GetTagsList();
                foreach (var tag in allTags) {
                    var tagViewModel = new TagViewModel {
                        Tag_ID = tag.Tag_ID,
                        Tag_Name = tag.Tag_Name
                    };
                    recipeAllTags.Add(tagViewModel);
                }
                recipeViewModel.AllTags = recipeAllTags;

                //nutrition elements
                recipeViewModel.RecipeNutritionElementsIds = DataAccess.RecipeNutritionElementAccessor.GetNutritionElementsIdsForRecipe(id);
                ICollection<NutritionElementViewModel> recipeAllNutritionElements = new List<NutritionElementViewModel>();
                ICollection<NutritionElementViewModel> recipeNutritionElements = new List<NutritionElementViewModel>();
                
                var nElems = DataAccess.RecipeNutritionElementAccessor.GetNutritionElementsForRecipe(id);
                foreach (var nElem in nElems) {
                    var neViewModel = new NutritionElementViewModel {
                        Element_Name = nElem.NutritionElement.Element_Name,
                        Element_Description = nElem.NutritionElement.Element_Description,
                        Measurement_Unit = nElem.Measurement_Unit,
                        Value = nElem.Value
                    };
                    recipeNutritionElements.Add(neViewModel);
                }
                recipeViewModel.RecipeNutritionElements = recipeNutritionElements;

                var allNElems = DataAccess.NutritionElementAccessor.GetNutritionElementsList();
                foreach (var nElem in allNElems) {
                    var neViewModel = new NutritionElementViewModel {
                        Element_Name = nElem.Element_Name,
                        Element_Description = nElem.Element_Description
                    };
                    recipeAllNutritionElements.Add(neViewModel);
                }
                recipeViewModel.AllNutritionElements = recipeAllNutritionElements;

                //ingredients
                recipeViewModel.RecipeIngredientsIds = DataAccess.RecipeIngredientAccessor.GetIngredientsIdsForRecipe(id);
                ICollection<IngredientViewModel> recipeAllIngredients = new List<IngredientViewModel>();
                ICollection<IngredientViewModel> recipeIngredients = new List<IngredientViewModel>();

                var ingreds = DataAccess.RecipeIngredientAccessor.GetIngredientsForRecipe(id);
                foreach (var ingred in ingreds) {
                    var ingr = new IngredientViewModel {
                        Ingredient_Name = ingred.Ingredient.Ingredient_Name,
                        Quantity = ingred.Quantity,
                        Measurement_Unit = ingred.Measurement_Unit,
                        IsOptional = ingred.IsOptional,
                        Other_Info = ingred.Other_Info,
                        Category = ingred.Ingredient.Category
                    };
                    recipeIngredients.Add(ingr);
                }
                recipeViewModel.RecipeIngredients = recipeIngredients;

                var allIngreds = DataAccess.IngredientAccessor.GetIngredientsList();
                foreach (var ingred in allIngreds) {
                    var ingr = new IngredientViewModel {
                        Ingredient_Name = ingred.Ingredient_Name,
                        Category = ingred.Category
                    };
                    recipeAllIngredients.Add(ingr);
                }
                recipeViewModel.AllIngredients = recipeAllIngredients;

                //steps
                ICollection<RecipeStepViewModel> recipeSteps = new List<RecipeStepViewModel>();
                var steps = DataAccess.RecipeStepAccessor.GetStepsForRecipe(id);
                foreach (var step in steps) {
                    var stepVM = new RecipeStepViewModel {
                        Step_Number = step.Step_Number,
                        Step_Title = step.Step_Title,
                        Step_Description = step.Step_Description,
                        IsOptional = step.IsOptional
                    };
                    recipeSteps.Add(stepVM);
                }
                recipeViewModel.RecipeSteps = recipeSteps;

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

                   var tagExistsInDB = DataAccess.TagAccessor.CheckTagExists(model.Tag_Name);

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

                   DataAccess.TagAccessor.EditTag(newTag);

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

        /*
       #region Delete

       // GET: Tag/Edit/id
       public ActionResult Delete(long id)
       {
           try
           {
               var tag = DataAccess.TagAccessor.GetTag(id);
               TagViewModel tagViewModel = new TagViewModel();

               if (tag == null)
               {
                   ViewBag.Warning = "The tag could not be found";
                   return View(tagViewModel);
               }

               tagViewModel.Tag_Name = tag.Tag_Name;
               tagViewModel.Tag_Recipes = string.Join(", ", DataAccess.RecipeTagAccessor.GetRecipesForTag(tag.Tag_ID).Select(r => r.Recipe_Name).ToList());

               return View(tagViewModel);
           }
           catch (Exception ex)
           {
               Utils.WriteToLog("Tags", "Delete", ex.ToString());
               return new HttpStatusCodeResult(StatusCodes.Status500InternalServerError, ex.ToString());
           }
       }


       // POST: Tag/Delete/id

       [HttpPost]
       [ActionName("Delete")]
       public ActionResult DeleteTag(long id)
       {
           try
           {
               DataAccess.TagAccessor.RemoveTag(id);

               return RedirectToAction("Index");
           }
           catch (Exception ex)
           {
               Utils.WriteToLog("Tags", "Delete", ex.ToString());
               return new HttpStatusCodeResult(StatusCodes.Status500InternalServerError, ex.ToString());
           }

       }

       #endregion Delete
       */
    }
}