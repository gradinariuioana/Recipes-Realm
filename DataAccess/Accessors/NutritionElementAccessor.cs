using ModelsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class NutritionElementAccessor
    {
        public static void ShowNutritionElements()
        {
            using (var context = new DatabaseRepository.RecipesRealmContext())
            {
                Console.WriteLine("--- Nutrition Elements ---");

                var nutritionElements = context.NutritionElements.ToList();
                foreach (NutritionElement item in nutritionElements)
                {
                    Console.WriteLine("Nutrition Element Name: {0}\nNutrition Element Description: {1}\n\n", 
                                       item.Element_Name, item.Element_Description);
                }
            }
        }

        public static string GetNutritionElementName(int idx)
        {
            using (var context = new DatabaseRepository.RecipesRealmContext())
            {
                var nutritionElement = context.NutritionElements.FirstOrDefault(n => n.ID == idx);
                return nutritionElement.Element_Name;
            }
        }
    }
}
