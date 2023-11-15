namespace RecipesRealm.ViewModels
{
    public class IngredientViewModel
    {
        public long Ingredient_ID { get; set; }

        public string Ingredient_Name { get; set; }

        public long Quantity { get; set; }

        public string Measurement_Unit { get; set; }

        public bool? IsOptional { get; set; }

        public string Category { get; set; }

        public string Other_Info { get; set; }
    }
}