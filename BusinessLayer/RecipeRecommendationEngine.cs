using ModelsLibrary;
using System.Collections.Generic;
using System.Linq;
using BusinessLayer.Interfaces;
using DataAccess;
using NLog;
using System;

namespace BusinessLayer {
    public class RecipeRecommendationEngine : IRecipeRecommendationEngine {
        protected Logger logger = LogManager.GetCurrentClassLogger();
        private readonly int numberOfRecommendationsToGenerate = 50;
        private ICategoryAccessor categoryAccessor;
        private IIngredientAccessor IngredientAccessor;
        private INutritionElementAccessor nutritionElementAccessor;
        private IRatingAccessor ratingAccessor;
        private IRecipeAccessor recipeAccessor;
        private IRecipeCategoryAccessor recipeCategoryAccessor;
        private IRecipeIngredientAccessor recipeIngredientAccessor;
        private IRecipeNutritionElementAccessor recipeNutritionElementAccessor;
        private IRecipeStepAccessor recipeStepAccessor;
        private IRecipeTagAccessor recipeTagAccessor;
        private IReviewAccessor reviewAccessor;
        private ISavedRecipeAccessor savedRecipeAccessor;
        private ITagAccessor tagAccessor;
        private IUserAccessor userAccessor;
        private ICache cacheManager;
        public RecipeRecommendationEngine(ICategoryAccessor categoryAccessor, IIngredientAccessor ingredientAccessor, INutritionElementAccessor nutritionElementAccessor,
                                           IRatingAccessor ratingAccessor, IRecipeAccessor recipeAccessor, IRecipeCategoryAccessor recipeCategoryAccessor,
                                           IRecipeIngredientAccessor recipeIngredientAccessor, IRecipeNutritionElementAccessor recipeNutritionElementAccessor,
                                           IRecipeStepAccessor recipeStepAccessor, IRecipeTagAccessor recipeTagAccessor, IReviewAccessor reviewAccessor,
                                           ISavedRecipeAccessor savedRecipeAccessor, ITagAccessor tagAccessor, IUserAccessor userAccessor, ICache cacheManager) {
            this.categoryAccessor = categoryAccessor;
            this.IngredientAccessor = ingredientAccessor;
            this.nutritionElementAccessor = nutritionElementAccessor;
            this.ratingAccessor = ratingAccessor;
            this.recipeAccessor = recipeAccessor;
            this.recipeCategoryAccessor = recipeCategoryAccessor;
            this.recipeIngredientAccessor = recipeIngredientAccessor;
            this.recipeNutritionElementAccessor = recipeNutritionElementAccessor;
            this.recipeStepAccessor = recipeStepAccessor;
            this.recipeTagAccessor = recipeTagAccessor;
            this.reviewAccessor = reviewAccessor;
            this.savedRecipeAccessor = savedRecipeAccessor;
            this.tagAccessor = tagAccessor;
            this.userAccessor = userAccessor;
            this.cacheManager = cacheManager;
        }

        public List<Recipe> GenerateRecommendations(long userId) {
            try {
                logger.Debug("Start Generating Recommendations");

                List<Recipe> recommendations = new List<Recipe>();

                var allRatedRecipesIds = ratingAccessor.GetRatingsForUser(userId).Select(r => r.Recipe_ID);
                var allSavedRecipesIds = savedRecipeAccessor.GetSavedRecipesForUser(userId).Select(r => r.Recipe_ID);
                var allUserRecipesIds = recipeAccessor.GetRecipesForUser(userId).Select(r => r.Recipe_ID);

                //Exclude user Recipes and recipes that have already been Saved or Rated by User
                var idsToExclude = allRatedRecipesIds.Union(allSavedRecipesIds).Union(allUserRecipesIds);


                logger.Debug("User Ratings and Saved Recipes retrieved");

                logger.Debug("Getting saved recipes for user");
                //Get Saved Recipes for User
                var userSavedRecipes = savedRecipeAccessor.GetSavedRecipesForUser(userId);

                //Get Tags for Saved Recipes
                logger.Debug("Getting liked tags based on saved recipes");

                List<Tag> likedTags = savedRecipeAccessor.GetLikedTagsFromSaving(userSavedRecipes);

                foreach (var tag in likedTags) {
                    //Get Recipes with Liked Tags that have not been rated or saved by User
                    var recipesWithLikedTags = recipeTagAccessor.GetRecipesForTag(tag.Tag_ID)
                                                                .Where(r => !idsToExclude.Contains(r.Recipe_ID));
                    recommendations.AddRange(recipesWithLikedTags);

                    recommendations = recommendations.GroupBy(r => r.Recipe_ID).Select(r => r.FirstOrDefault()).ToList();
                }

                if (recommendations.Count > numberOfRecommendationsToGenerate) {
                    return recommendations;
                }

                //Get Categories for Saved Recipes
                logger.Debug("Getting liked categories based on saved recipes");

                List<Category> likedCategories = savedRecipeAccessor.GetLikedCategoriesFromSaving(userSavedRecipes);

                foreach (var category in likedCategories) {
                    //Get Recipes with Liked Categories that have not been rated or saved by User
                    var recipesWithLikedTags = recipeCategoryAccessor.GetRecipesForCategory(category.Category_ID).Where(r => !idsToExclude.Contains(r.Recipe_ID));
                    recommendations.AddRange(recipesWithLikedTags);

                    recommendations = recommendations.GroupBy(r => r.Recipe_ID).Select(r => r.FirstOrDefault()).ToList();
                }

                if (recommendations.Count > numberOfRecommendationsToGenerate) {
                    return recommendations;
                }

                for (var score = 5; score > 2; score--) {

                    logger.Debug("Getting ratings with score = " + score);
                    //Get Ratings For User starting from 5 to 1

                    var userRatings = ratingAccessor.GetRatingsWithScoreForUser(userId, score);

                    if (userRatings.Count > 0) {
                        //Get Tags of Liked Recipes
                        logger.Debug("Getting liked tags based on ratings with score = " + score);

                        likedTags = ratingAccessor.GetLikedTagsFromRatings(userRatings);

                        foreach (var tag in likedTags) {
                            //Get Recipes with Liked Tags that have not been rated or saved by User
                            var recipesWithLikedTags = recipeTagAccessor.GetRecipesForTag(tag.Tag_ID).Where(r => !idsToExclude.Contains(r.Recipe_ID));
                            recommendations.AddRange(recipesWithLikedTags);

                            recommendations = recommendations.GroupBy(r => r.Recipe_ID).Select(r => r.FirstOrDefault()).ToList();
                        }

                        if (recommendations.Count > numberOfRecommendationsToGenerate) {
                            return recommendations;
                        }

                        //Get Categories of Liked Recipes
                        logger.Debug("Getting liked categories based on ratings with score = " + score);

                        likedCategories = ratingAccessor.GetLikedCategoriesFromRatings(userRatings);

                        foreach (var category in likedCategories) {
                            //Get Recipes with Liked Categories that have not been rated or saved by User
                            var recipesWithLikedTags = recipeCategoryAccessor.GetRecipesForCategory(category.Category_ID).Where(r => !idsToExclude.Contains(r.Recipe_ID));
                            recommendations.AddRange(recipesWithLikedTags);

                            recommendations = recommendations.GroupBy(r => r.Recipe_ID).Select(r => r.FirstOrDefault()).ToList();
                        }

                        if (recommendations.Count > numberOfRecommendationsToGenerate) {
                            return recommendations;
                        }
                    }
                }

                //OPTIONAL: Add other recipes ordered by rating descending
                /*if (recommendations.Count < numberOfRecommendationsToGenerate) {
                    idsToExclude = idsToExclude.Union(recommendations.Select(r => r.Recipe_ID).ToList());
                    var noUserRecommendations = GenerateRecommendationsNoUser(numberOfRecommendationsToGenerate - recommendations.Count, idsToExclude.ToList());
                    recommendations.AddRange(noUserRecommendations);
                }*/

                logger.Debug("End Generating Recommendations");
                logger.Debug("Generated " + recommendations.Count + " recommendations");

                return recommendations;
            }
            catch (Exception ex) {
                logger.Error(ex, "Error on Generating Recommendations");
                return new List<Recipe> { };
            }
        }

        public List<Recipe> GenerateRecommendationsNoUser(int? number, List<long> idsToExclude = null) {
            try {
                logger.Debug("Start Generating No User Recommendations");

                //Get Recipes ordered by rating descending
                List<Recipe> recommendations = recipeAccessor.GetRecipesList()
                                               .OrderByDescending(r => ratingAccessor.GetAverageRatingForRecipe(r.Recipe_ID))
                                               .ToList();

                if (recommendations is null) {
                    logger.Warn("Recommendation No User List is empty");
                }
                else {

                    if (number != null) {
                        recommendations = recommendations.Where(r => !idsToExclude.Contains(r.Recipe_ID)).ToList();
                        recommendations = recommendations.Take(number.Value).ToList();
                    }
                }

                logger.Debug("End Generating No User Recommendations");
                logger.Debug("Generated " + recommendations.Count + " recommendations");

                return recommendations;
            }
            catch (Exception ex) {
                logger.Error(ex, "Error on Generating Recommendations");
                return new List<Recipe> { };
            }
        }

        #region Recipe

        public long AddRecipe(Recipe recipe) {
            try {
                var id = recipeAccessor.AddRecipe(recipe);
                cacheManager.Remove("recipes_list");

                return id;
            }
            catch (Exception ex) {
                logger.Error(ex, "Error on Adding Recipe to DB");
                return -1;
            }
        }

        public Recipe GetRecipe(long id) {
            try {
                Recipe r;
                string key = "recipes_" + id;

                if (cacheManager.IsSet(key)) {
                    r = cacheManager.Get<Recipe>(key);
                }
                else {
                    r = recipeAccessor.GetRecipe(id);
                    cacheManager.Set(key, r);
                }
                return r;
            }
            catch (Exception ex) {
                logger.Error(ex, "Error on Getting Recipe from DB");
                return null;
            }
        }

        public IEnumerable<Recipe> GetRecipesList() {
            try {
                string key = "recipes_list";
                IEnumerable<Recipe> recipes;

                if (cacheManager.IsSet(key)) {
                    recipes = cacheManager.Get<List<Recipe>>(key);
                }
                else {
                    recipes = recipeAccessor.GetRecipesList();
                    cacheManager.Set(key, recipes);
                }

                return recipes;
            }
            catch (Exception ex) {
                logger.Error(ex, "Error on Getting Recipes List from DB");
                return new List<Recipe> { };
            }
        }

        public void UpdateRecipe(Recipe recipe) {
            try {
                recipeAccessor.UpdateRecipe(recipe);

                string individual_key = "recipes_" + recipe.Recipe_ID;
                string list_key = "recipes_list";

                cacheManager.Remove(individual_key);
                cacheManager.RemoveByPattern(list_key);
            }
            catch (Exception ex) {
                logger.Error(ex, "Error on Updating Recipe");
            }
        }

        public void DeleteRecipe(long id) {
            try {
                recipeAccessor.DeleteRecipe(id);

                string individual_key = "recipes_" + id;
                string list_key = "recipes_list";

                cacheManager.Remove(individual_key);
                cacheManager.RemoveByPattern(list_key);
            }
            catch (Exception ex) {
                logger.Error(ex, "Error on Removing Recipe from DB");
            }
        }

        public void DeactivateRecipe(long id) {
            try {
                recipeAccessor.DeactivateRecipe(id);

                string individual_key = "recipes_" + id;
                string list_key = "recipes_list";

                cacheManager.Remove(individual_key);
                cacheManager.RemoveByPattern(list_key);
            }
            catch (Exception ex) {
                logger.Error(ex, "Error on Deactivating Recipe");
            }
        }

        public void ReactivateRecipe(long id) {
            try {
                recipeAccessor.ReactivateRecipe(id);

                string individual_key = "recipes_" + id;
                string list_key = "recipes_list";

                cacheManager.Remove(individual_key);
                cacheManager.RemoveByPattern(list_key);
            }
            catch (Exception ex) {
                logger.Error(ex, "Error on Reactivating Recipe");
            }
        }

        #endregion Recipe

        #region Ingredient
        public IEnumerable<Ingredient> GetIngredientsList() {
            try {
                string key = "ingredients_list";
                IEnumerable<Ingredient> ingredients;

                if (cacheManager.IsSet(key)) {
                    ingredients = cacheManager.Get<List<Ingredient>>(key);
                }
                else {
                    ingredients = IngredientAccessor.GetIngredientsList();
                    cacheManager.Set(key, ingredients);
                }

                return ingredients;
            }
            catch (Exception ex) {
                logger.Error(ex, "Error on Getting Ingredients List from DB");
                return new List<Ingredient> { };
            }
        }
        public long AddIngredient(Ingredient ingredient) {
            try {
                var id = IngredientAccessor.AddIngredient(ingredient);

                cacheManager.Remove("ingredients_list");

                return id;
            }
            catch (Exception ex) {
                logger.Error(ex, "Error on Adding Ingredient to DB");
                return -1;
            }
        }

        public bool CheckIngredientExists(string ingredient) {
            return IngredientAccessor.CheckIngredientExists(ingredient);
        }

        #endregion Ingredient


        #region Category

        public IEnumerable<Category> GetCategoriesList() {
            try {
                string key = "categories_list";
                IEnumerable<Category> categories;

                if (cacheManager.IsSet(key)) {
                    categories = cacheManager.Get<List<Category>>(key);
                }
                else {
                    categories = categoryAccessor.GetCategoriesList();
                    cacheManager.Set(key, categories);
                }

                return categories;
            }
            catch (Exception ex) {
                logger.Error(ex, "Error on Getting Categories List from DB");
                return new List<Category> { };
            }
        }

        public Category GetCategoryById(long id) {
            try {
                Category c;
                string key = "categories_" + id;

                if (cacheManager.IsSet(key)) {
                    c = cacheManager.Get<Category>(key);
                }
                else {
                    c = categoryAccessor.GetCategoryById(id);
                    cacheManager.Set(key, c);
                }
                return c;
            }
            catch (Exception ex) {
                logger.Error(ex, "Error on Getting Category from DB");
                return null;
            }
        }

        #endregion Category

        #region Tag

        public IEnumerable<Tag> GetTagsList() {
            try {
                string key = "tags_list";
                IEnumerable<Tag> tags;

                if (cacheManager.IsSet(key)) {
                    tags = cacheManager.Get<List<Tag>>(key);
                }
                else {
                    tags = tagAccessor.GetTagsList();
                    cacheManager.Set(key, tags);
                }

                return tags;
            }
            catch (Exception ex) {
                logger.Error(ex, "Error on Getting Tags List from DB");
                return new List<Tag> { };
            }
        }

        #endregion Tag

        #region Nutrition Element
        public IEnumerable<NutritionElement> GetNutritionElementsList() {
            try {
                string key = "nutritionElements_list";
                IEnumerable<NutritionElement> nElems;

                if (cacheManager.IsSet(key)) {
                    nElems = cacheManager.Get<List<NutritionElement>>(key);
                }
                else {
                    nElems = nutritionElementAccessor.GetNutritionElementsList();
                    cacheManager.Set(key, nElems);
                }

                return nElems;
            }
            catch (Exception ex) {
                logger.Error(ex, "Error on Getting Nutrition Elements List from DB");
                return new List<NutritionElement> { };
            }
        }

        #endregion Nutrition Element

        #region User 

        public string GetUserName(long id) {
            try {
                return userAccessor.GetUserName(id);
            }
            catch (Exception ex) {
                logger.Error(ex, "Error on Getting User Name from DB");
                return "";
            }
        }

        public User CheckUserForLogin(User user) {
            try {
                return userAccessor.CheckUserForLogin(user);
            }
            catch (Exception ex) {
                logger.Error(ex, "Error on Checking User for Login");
                return null;
            }
        }

        public User SignUpUser(User user) {
            try {
                return userAccessor.AddUser(user);
            }
            catch (Exception ex) {
                logger.Error(ex, "Error on Signing Up User");
                return null;
            }
        }

        public bool CheckUserEmailExists(string email) {
            try {
                return userAccessor.CheckUserEmailExists(email);
            }
            catch (Exception ex) {
                logger.Error(ex, "Error on Checking User Email in DB");
                return false;
            }
        }

        public List<Recipe> GetUserRecipes(long userId) {
            try {
                return recipeAccessor.GetRecipesForUser(userId);

            }
            catch (Exception ex) {
                logger.Error(ex, "Error on Getting User Recipes from DB");
                return new List<Recipe> { };
            }
        }

        #endregion User

        #region Rating

        public long GetAverageRatingForRecipe(long recipeId) {
            try {
                return ratingAccessor.GetAverageRatingForRecipe(recipeId);
            }
            catch (Exception ex) {
                logger.Error(ex, "Error on Getting Average Rating for Recipe from DB");
                return -1;
            }
        }

        public long AddRating(Rating rating) {
            try {
                rating.Rating_Date = DateTime.Now;

                return ratingAccessor.AddRating(rating);
            }
            catch (Exception ex) {
                logger.Error(ex, "Error on Adding Rating to DB");
                return -1;
            }
        }

        public bool CheckUserCanRate(Rating rating) {
            try {
                return ratingAccessor.CheckUserCanRate(rating);
            }
            catch (Exception ex) {
                logger.Error(ex, "Error on Checking if User can Rate Recipe in DB");
                return false;
            }
        }

        #endregion Rating

        #region SavedRecipe
        public bool CheckSavedRecipe(SavedRecipe savedRecipe) {
            try {
                return savedRecipeAccessor.CheckUserSavedRecipe(savedRecipe);
            }
            catch (Exception ex) {
                logger.Error(ex, "Error on Checking if User has Saved Recipe in DB");
                return false;
            }
        }

        public void SaveRecipe(SavedRecipe savedRecipe) {
            try {
                savedRecipe.Saving_Date = DateTime.Now;

                savedRecipeAccessor.SaveRecipe(savedRecipe);
            }
            catch (Exception ex) {
                logger.Error(ex, "Error on Adding Saved Recipe to DB");
            }
        }

        public void UnSaveRecipe(SavedRecipe savedRecipe) {
            try {
                savedRecipeAccessor.UnSaveRecipe(savedRecipe);
            }
            catch (Exception ex) {
                logger.Error(ex, "Error on Removing Saved Recipe from DB");
            }
        }
        public List<Recipe> GetUserSavedRecipes(long userId) {
            try {
                return savedRecipeAccessor.GetSavedRecipesListForUser(userId);

            }
            catch (Exception ex) {
                logger.Error(ex, "Error on Getting User Saved Recipes from DB");
                return new List<Recipe> { };
            }
        }


        #endregion SavedRecipe

        #region Recipe Category

        public IEnumerable<Recipe> GetRecipes(long id) {
            try {
                return recipeCategoryAccessor.GetRecipes(id);
            }
            catch (Exception ex) {
                logger.Error(ex, "Error on Getting Recipes with Category from DB");
                return new List<Recipe> { };
            }
        }

        public ICollection<long> GetCategoriesIdsForRecipe(long id) {
            try {

                return recipeCategoryAccessor.GetCategoriesIdsForRecipe(id);
            }
            catch (Exception ex) {
                logger.Error(ex, "Error on Getting Categories for Recipe");
                return new List<long> { };
            }
        }

        public void DeleteRecipeCategory(long id) {
            try {
                recipeCategoryAccessor.DeleteRecipeCategory(id);
            }
            catch (Exception ex) {
                logger.Error(ex, "Error on Deleting Recipe Category from DB");
            }
        }

        public void DeleteAllCategoriesForRecipe(long recipeId) {
            try {
                recipeCategoryAccessor.DeleteAllCategoriesForRecipe(recipeId);
            }
            catch (Exception ex) {
                logger.Error(ex, "Error on Deleting All Recipe Categories from DB");
            }

        }

        public void AddRecipeCategory(RecipeCategory recipeCategory) {
            try {
                recipeCategoryAccessor.AddRecipeCategory(recipeCategory);
            }
            catch (Exception ex) {
                logger.Error(ex, "Error on Adding Recipe Category to DB");
            }
        }

        #endregion Recipe Category

        #region Recipe Tag

        public ICollection<long> GetTagsIdsForRecipe(long id) {
            try {
                return recipeTagAccessor.GetTagsIdsForRecipe(id);
            }
            catch (Exception ex) {
                logger.Error(ex, "Error on Getting Recipe Tags from DB");
                return new List<long> { };
            }
        }

        public void AddRecipeTag(RecipeTag recipeTag) {
            try {
                recipeTagAccessor.AddRecipeTag(recipeTag);
            }
            catch (Exception ex) {
                logger.Error(ex, "Error on Adding Recipe Tag to DB");
            }
        }

        public void DeleteRecipeTag(long id) {
            try {
                recipeTagAccessor.DeleteRecipeTag(id);
            }
            catch (Exception ex) {
                logger.Error(ex, "Error on Removing Recipe Tag from DB");
            }
        }

        public void DeleteAllTagsForRecipe(long recipeId) {
            try {
                recipeTagAccessor.DeleteAllTagsForRecipe(recipeId);
            }
            catch (Exception ex) {
                logger.Error(ex, "Error on Deleting All Recipe Tags from DB");
            }
        }

        #endregion Recipe Tag

        #region Recipe NE

        public void DeleteAllNutritionElementsForRecipe(long recipeId) {
            try {
                recipeNutritionElementAccessor.DeleteAllNutritionElementsForRecipe(recipeId);
            }
            catch (Exception ex) {
                logger.Error(ex, "Error on Removing all Recipe NElems from DB");
            }
        }
        public void AddRecipeNutritionElement(RecipeNutritionElement recipeNutritionElement) {
            try {
                recipeNutritionElementAccessor.AddRecipeNutritionElement(recipeNutritionElement);
            }
            catch (Exception ex) {
                logger.Error(ex, "Error on Adding Recipe NElem to DB");
            }
        }

        #endregion Recipe NE

        #region Recipe Ingredient

        public void DeleteAllIngredientsForRecipe(long recipeId) {
            try {
                recipeIngredientAccessor.DeleteAllIngredientsForRecipe(recipeId);
            }
            catch (Exception ex) {
                logger.Error(ex, "Error on Removing all Recipe Ingredients from DB");
            }
        }

        public void AddRecipeIngredient(RecipeIngredient recipeIngredient) {
            try {
                recipeIngredientAccessor.AddRecipeIngredient(recipeIngredient);
            }
            catch (Exception ex) {
                logger.Error(ex, "Error on Adding Recipe Ingredient to DB");
            }
        }

        #endregion Recipe Ingredient

        #region Recipe Step

        public void DeleteAllStepsForRecipe(long recipeId) {
            try {
                recipeStepAccessor.DeleteAllStepsForRecipe(recipeId);
            }
            catch (Exception ex) {
                logger.Error(ex, "Error on Removing all Recipe Steps from DB");
            }
        }

        public void AddRecipeStep(RecipeStep recipeStep) {
            try {
                recipeStepAccessor.AddRecipeStep(recipeStep);
            }
            catch (Exception ex) {
                logger.Error(ex, "Error on Adding Recipe Step to DB");
            }
        }

        #endregion Recipe Step
    }
}
