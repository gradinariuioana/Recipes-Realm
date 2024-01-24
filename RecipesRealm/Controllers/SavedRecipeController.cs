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
    public class SavedRecipeController : BaseController {

        public SavedRecipeController(IRecipeRecommendationEngine recipeRecommendationEngine) : base(recipeRecommendationEngine) {
        }

        public void SaveRecipe(string modelString) {
            try {
                SavedRecipeViewModel savedRecipeViewModel = JsonConvert.DeserializeObject<SavedRecipeViewModel>(modelString);
                SavedRecipe savedRecipe = AutoMapperConfig.Mapper.Map<SavedRecipe>(savedRecipeViewModel);

                recipeRecommendationEngine.SaveRecipe(savedRecipe);
            }
            catch (Exception ex) {
                logger.Error(ex, "Error on Saving Recipe");
            }
        }
        public void UnSaveRecipe(string modelString) {
            try {
                SavedRecipeViewModel savedRecipeViewModel = JsonConvert.DeserializeObject<SavedRecipeViewModel>(modelString);
                SavedRecipe savedRecipe = AutoMapperConfig.Mapper.Map<SavedRecipe>(savedRecipeViewModel);

                recipeRecommendationEngine.UnSaveRecipe(savedRecipe);
            }
            catch (Exception ex) {
                logger.Error(ex, "Error on Saving Recipe");
            }
        }


        [Produces("application/json")]
        public string CheckUserSavedRecipe(string modelString) {
            try {
                SavedRecipeViewModel savedRecipeVM = JsonConvert.DeserializeObject<SavedRecipeViewModel>(modelString);
                SavedRecipe savedRecipe = AutoMapperConfig.Mapper.Map<SavedRecipe>(savedRecipeVM);

                var saved = recipeRecommendationEngine.CheckSavedRecipe(savedRecipe);

                return JsonConvert.SerializeObject(new { saved });
            }
            catch (Exception ex) {
                logger.Error(ex, "Error on Checking if User Has Saved Recipe");
                return ex.ToString();
            }
        }
    }
}