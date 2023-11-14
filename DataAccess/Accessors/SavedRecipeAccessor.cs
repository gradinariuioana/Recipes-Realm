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
        public static void ShowSavedRecipes()
        {
            using (var context = new DatabaseRepository.RecipesRealmContext())
            {
                Console.WriteLine("--- SavedRecipes ---");

                var savedRecipes = context.SavedRecipes.ToList();
                foreach (SavedRecipe item in savedRecipes)
                {
                    //Recipe and User - Lazy Loading
                    Console.WriteLine("User Name: {0}\nSaving Date: {1}\nRecipe Name: {2}\n\n", 
                                       item.User.User_Name, item.Saving_Date.ToString("dd-MM-yyyy"), item.Recipe.Recipe_Name);
                }
            }
        }

        public static string GetUserName(int idx)
        {
            using (var context = new DatabaseRepository.RecipesRealmContext())
            {
                //User - Eager Loading
                var savedRecipe = context.SavedRecipes.Include(s => s.User).FirstOrDefault(s => s.ID == idx);
                return savedRecipe.User.User_Name;
            }
        }

        public static string GetRecipeName(int idx)
        {
            using (var context = new DatabaseRepository.RecipesRealmContext())
            {
                //Recipe - Eager Loading
                var savedRecipe = context.SavedRecipes.Include(s => s.Recipe).FirstOrDefault(s => s.ID == idx);
                return savedRecipe.Recipe.Recipe_Name;
            }
        }
    }
}
