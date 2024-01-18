using System.ComponentModel;

namespace RecipesRealm.ViewModels {
    public class NutritionElementViewModel
    {
        public long? Recipe_ID { get; set; }
        public long NutritionElement_ID { get; set; }

        [DisplayName("Element")]
        public string Element_Name { get; set; }

        public string Element_Description { get; set; }

        public long? Value { get; set; }

        [DisplayName("Unit")]
        public string Measurement_Unit { get; set; }
    }
}