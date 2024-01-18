using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ModelsLibrary
{
    [Serializable]
    public class NutritionElement
    {
        [Key]
        public long ID { get; set; }

        [Required]
        [MaxLength(200)]
        public string Element_Name { get; set; }
        public string Element_Description { get; set; }
    
        public virtual ICollection<RecipeNutritionElement> RecipeNutritionElements { get; set; }
    }
}
