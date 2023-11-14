using ModelsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class CategoryAccessor
    {
        public static void ShowCategories()
        {
            using (var context = new DatabaseRepository.RecipesRealmContext())
            {
                Console.WriteLine("--- Categories ---");

                var categorys = context.Categories.ToList();
                foreach (Category item in categorys)
                {
                    Console.WriteLine("Category Name: {0}, Category Description: {1}", item.Category_Name, item.Category_Description);
                }
            }
        }

        public static string GetCategoryName(int idx)
        {
            using (var context = new DatabaseRepository.RecipesRealmContext())
            {
                var category = context.Categories.FirstOrDefault(c => c.Category_ID == idx);
                return category.Category_Name;
            }
        }
    }
}
