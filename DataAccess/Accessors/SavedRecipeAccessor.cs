using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using ModelsLibrary;

namespace DataAccess
{
    public class SavedRecipeAccessor
    {
        public static List<SavedRecipe> GetSavedRecipesForUser(long userId)
        {
            using (var context = new DatabaseRepository.RecipesRealmContext())
            {
                var savedRecipes = context.SavedRecipes.Where(r => r.User_ID == userId).Include("Recipe.RecipeTags.Tag").Include("Recipe.RecipeCategories.Category").ToList();

                return savedRecipes;
            }
        }
    }
}
