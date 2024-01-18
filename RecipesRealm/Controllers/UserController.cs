using RecipesRealm.ViewModels;
using System.Web.Mvc;
using BusinessLayer.Interfaces;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ModelsLibrary;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace RecipesRealm.Controllers {
    public class UserController : BaseController {

        public UserController(IRecipeRecommendationEngine recipeRecommendationEngine) : base(recipeRecommendationEngine) {
        }

        [Produces("application/json")]
        public string Login(string modelString) {
            try {
                UserViewModel userVM = JsonConvert.DeserializeObject<UserViewModel>(modelString);
                User user = AutoMapperConfig.Mapper.Map<User>(userVM);

                User loggedInUser = recipeRecommendationEngine.CheckUserForLogin(user);

                if (loggedInUser != null) {
                    return JsonConvert.SerializeObject(new { userName = loggedInUser.User_Name, userId = loggedInUser.User_ID });
                }

                return JsonConvert.SerializeObject(new { userId = -1 });
            }
            catch (Exception ex) {
                logger.Error(ex, "Error on User Login");
                return ex.ToString();
            }
        }

        [Produces("application/json")]
        public string SignUp(string modelString) {
            try {
                UserViewModel userVM = JsonConvert.DeserializeObject<UserViewModel>(modelString);
                User user = AutoMapperConfig.Mapper.Map<User>(userVM);

                bool emailExists = recipeRecommendationEngine.CheckUserEmailExists(user.Email_Address);
                if (emailExists) {
                    return JsonConvert.SerializeObject(new { userId = -1, EmailExists = true });
                }

                User loggedInUser = recipeRecommendationEngine.SignUpUser(user);

                if (loggedInUser != null) {
                    return JsonConvert.SerializeObject(new { userName = loggedInUser.User_Name, userId = loggedInUser.User_ID });
                }

                return JsonConvert.SerializeObject(new { userId = -1 });
            }
            catch (Exception ex) {
                logger.Error(ex, "Error on User Signup");
                return ex.ToString();
            }
        }

        public System.Web.Mvc.ActionResult MyRecipes(long id) {
            try {
                IEnumerable<RecipeViewModel> recipeViewModels = new List<RecipeViewModel>();
                IEnumerable<Recipe> recipes = recipeRecommendationEngine.GetUserRecipes(id);

                if (recipes is null) {
                    logger.Error("User Recipes could not be retrieved");
                }

                foreach (Recipe r in recipes)
                    recipeViewModels = recipeViewModels.Append(AutoMapperConfig.Mapper.Map<RecipeViewModel>(r));

                return View(recipeViewModels);
            }

            catch (Exception ex) {
                logger.Error(ex, "Error on getting User Recipes");
                return new HttpStatusCodeResult(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }

    }
}