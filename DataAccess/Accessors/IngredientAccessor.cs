using ModelsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class IngredientAccessor
    {
        public static bool CheckIngredientExists(long id) {
            using (var context = new DatabaseRepository.RecipesRealmContext()) {
                Ingredient ingredient = context.Ingredients.FirstOrDefault(t => t.Ingredient_ID == id);

                return ingredient != null;
            }
        }

        public static long AddIngredient(Ingredient ingredient) {
            using (var context = new DatabaseRepository.RecipesRealmContext()) {
                context.Ingredients.Add(ingredient);
                context.SaveChanges();

                return ingredient.Ingredient_ID;
            }
        }
    }
}
