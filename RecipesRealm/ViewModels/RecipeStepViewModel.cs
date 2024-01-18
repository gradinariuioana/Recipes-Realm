using System.ComponentModel;

namespace RecipesRealm.ViewModels
{
    public class RecipeStepViewModel
    {
        public long? Recipe_ID { get; set; }
        public int? Step_Number { get; set; }

        [DisplayName("Step Description")]
        public string Step_Description{ get; set; }

        [DisplayName("Step Title")]
        public string Step_Title { get; set; }

        [DisplayName("Is Optional")]
        public bool IsOptional { get; set; }

        public string Picture_Path { get; set; }
    }
}