using DatabaseRepository;
using ModelsLibrary;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess {
    public class IngredientAccessor : IIngredientAccessor {

        public RecipesRealmContext context;

        public IngredientAccessor(IRecipesRealmContext db) {
            context = (RecipesRealmContext)db;
        }

        public IEnumerable<Ingredient> GetIngredientsList() {
            var ingredients = context.Ingredients.ToList();
            return ingredients;
        }

        public bool CheckIngredientExists(string ingredient) {
            Ingredient ing = context.Ingredients.FirstOrDefault(t => t.Ingredient_Name == ingredient);

            return ing != null;
        }

        public long AddIngredient(Ingredient ingredient) {
            context.Ingredients.Add(ingredient);
            context.SaveChanges();

            return ingredient.Ingredient_ID;
        }
    }

    public interface IIngredientAccessor {
        IEnumerable<Ingredient> GetIngredientsList();
        bool CheckIngredientExists(string ingredient);
        long AddIngredient(Ingredient ingredient);
    }
}
