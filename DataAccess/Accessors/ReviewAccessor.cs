using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using ModelsLibrary;

namespace DataAccess
{
    public class ReviewAccessor
    {
        public static void ShowReviews()
        {
            using (var context = new DatabaseRepository.RecipesRealmContext())
            {
                Console.WriteLine("--- Reviews ---");

                var reviews = context.Reviews.ToList();
                foreach (Review item in reviews)
                {
                    //Recipe and User - Lazy Loading
                    Console.WriteLine("User Name: {0}\nReview Text: {1}\nReview Date: {2}\nRecipe Name: {3}\n\n", 
                                       item.User.User_Name, item.Review_Text, item.Review_Date.ToString("dd-MM-yyyy"), item.Recipe.Recipe_Name);
                }
            }
        }

        public static string GetUserName(int idx)
        {
            using (var context = new DatabaseRepository.RecipesRealmContext())
            {
                //User - Eager Loading
                var review = context.Reviews.Include(r => r.User).FirstOrDefault(r => r.Review_ID == idx);
                return review.User.User_Name;
            }
        }

        public static string GetRecipeName(int idx)
        {
            using (var context = new DatabaseRepository.RecipesRealmContext())
            {
                //Recipe - Eager Loading
                var review = context.Reviews.Include(r => r.Recipe).FirstOrDefault(r => r.Review_ID == idx);
                return review.Recipe.Recipe_Name;
            }
        }
    }
}
