using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelsLibrary
{
    [Serializable]
    public class Recipe
    {
        [Key]
        public long Recipe_ID { get; set; }

        [Required]
        [MaxLength(500)]
        public string Recipe_Name { get; set; }
        public string Recipe_Description { get; set; }

        [MaxLength(50)]
        public string Cooking_Time { get; set; }
        public int? Difficulty_Level { get; set; }

        [Required]
        public DateTime Creation_Date { get; set; }
        public string Picture_Path { get; set; }
        public int? Servings { get; set; }

        [Required]
        public long Author_User_ID { get; set; }

        public bool? Is_Active { get; set; }

    
        [ForeignKey("Author_User_ID")]
        public virtual User User { get; set; }

        public virtual ICollection<Rating> Ratings { get; set; }
        public virtual ICollection<RecipeCategory> RecipeCategories { get; set; }
        public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; }
        public virtual ICollection<RecipeNutritionElement> RecipeNutritionElements { get; set; }
        public virtual ICollection<RecipeStep> RecipeSteps { get; set; }
        public virtual ICollection<RecipeTag> RecipeTags { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<SavedRecipe> SavedRecipes { get; set; }

    }
}
