using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using ModelsLibrary;

namespace DataAccess
{
    public class RatingAccessor
    {
        public static void ShowRatings()
        {
            using (var context = new DatabaseRepository.RecipesRealmContext())
            {
                Console.WriteLine("--- Ratings ---");

                var ratings = context.Ratings.ToList();
                foreach (Rating item in ratings)
                {
                    //Recipe and User - Lazy Loading
                    Console.WriteLine("User Name: {0}\nRating Value: {1}\nRating Date: {2}\nRecipe Name: {3}\n\n", 
                                       item.User.User_Name, item.Rating_Value, item.Rating_Date.ToString("dd-MM-yyyy"), item.Recipe.Recipe_Name);
                }
            }
        }

        public static string GetUserName(int idx)
        {
            using (var context = new DatabaseRepository.RecipesRealmContext())
            {
                //User - Eager Loading
                var rating = context.Ratings.Include(r => r.User).FirstOrDefault(r => r.Rating_ID == idx);
                return rating.User.User_Name;
            }
        }

        public static string GetRecipeName(int idx)
        {
            using (var context = new DatabaseRepository.RecipesRealmContext())
            {
                //Recipe - Eager Loading
                var rating = context.Ratings.Include(r => r.Recipe).FirstOrDefault(r => r.Rating_ID == idx);
                return rating.Recipe.Recipe_Name;
            }
        }
    }
}
