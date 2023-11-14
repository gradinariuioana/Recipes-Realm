namespace RecipesRealm.ViewModels
{
    public class RecipeStepViewModel
    {         
        public int? Step_Number { get; set; }  

        public string Step_Description{ get; set; }

        public string Step_Title { get; set; }

        public bool? IsOptional { get; set; }
    }
}