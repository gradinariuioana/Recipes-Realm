using Microsoft.AspNetCore.Http;
using ModelsLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;


namespace RecipesRealm.ViewModels {
    public class RecipeViewModel {
        [DisplayName("Recipe Name")]
        [Required(ErrorMessage = "A Recipe Name is required")]
        public string Recipe_Name { get; set; }

        [DisplayName("Recipe Description")]
        public string Recipe_Description { get; set; }

        [DisplayName("Cooks In")]
        public string Cooking_Time { get; set; }

        [DisplayName("Difficulty Level")]
        public int? Difficulty_Level { get; set; }

        [DisplayName("Created On")]
        public DateTime Creation_Date { get; set; }

        [DisplayName("Recipe Picture")]
        public string Picture_Path { get; set; }

        [DisplayName("Serves")]
        public int? Servings { get; set; }

        [DisplayName("Author")]
        public string Author_Name { get; set; }

        public long Author_User_ID { get; set; }

        [DisplayName("Average Rating")]
        public long? AverageRating { get; set; }

        /*
        
    
        public virtual ICollection<Review> Reviews { get; set; }*/
        public virtual ICollection<TagViewModel> RecipeTags { get; set; }
        public virtual ICollection<NutritionElementViewModel> RecipeNutritionElements { get; set; }
        public virtual ICollection<IngredientViewModel> RecipeIngredients { get; set; }
        public virtual ICollection<RecipeStepViewModel> RecipeSteps { get; set; }
        public virtual ICollection<CategoryViewModel> RecipeCategories { get; set; }

    }
}