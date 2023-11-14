using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using ModelsLibrary;

namespace DataAccess
{
    public class RecipeCategoryAccessor
    {
        public static void ShowRecipeCategories()
        {
            using (var context = new DatabaseRepository.RecipesRealmContext())
            {
                Console.WriteLine("--- Recipe Categories ---");

                var recipeCategories = context.RecipeCategories.ToList();
                foreach (RecipeCategory item in recipeCategories) 
                {
                    //Recipe and Category - Lazy Loading
                    Console.WriteLine("Recipe Name: {0}\nCategory Name: {1}\n\n", 
                                       item.Recipe.Recipe_Name, item.Category.Category_Name);
                }
            }
        }

        public static string GetCategoryName(int idx)
        {
            using (var context = new DatabaseRepository.RecipesRealmContext())
            {
                //Category - Eager Loading
                var recipeCategory = context.RecipeCategories.Include(r => r.Category).FirstOrDefault(r => r.Category_ID == idx);
                return recipeCategory.Category.Category_Name;
            }
        }

        public static string GetRecipeName(int idx)
        {
            using (var context = new DatabaseRepository.RecipesRealmContext())
            {
                //Recipe - Eager Loading
                var recipeCategory = context.RecipeCategories.Include(r => r.Recipe).FirstOrDefault(r => r.Category_ID == idx);
                return recipeCategory.Recipe.Recipe_Name;
            }
        }
    }
}
