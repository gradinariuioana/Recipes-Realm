using System.ComponentModel;

namespace RecipesRealm.ViewModels
{
    public class IngredientViewModel
    {
        public long? Recipe_ID {  get; set; }
        public long Ingredient_ID { get; set; }

        [DisplayName("Ingredient")]
        public string Ingredient_Name { get; set; }

        public long? Quantity { get; set; }

        [DisplayName("Unit")]
        public string Measurement_Unit { get; set; }

        [DisplayName("Is Optional")]
        public bool? IsOptional { get; set; }

        public string Category { get; set; }

        [DisplayName("Other Info")]
        public string Other_Info { get; set; }
    }
}