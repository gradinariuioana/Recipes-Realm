using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ModelsLibrary
{
    public class Ingredient
    {
        [Key]
        public long Ingredient_ID { get; set; }

        [Required]
        [MaxLength(200)]
        public string Ingredient_Name { get; set; }
    
        public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; }
    }
}