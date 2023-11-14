using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelsLibrary
{
    public class RecipeNutritionElement
    {
        [Key]
        public long ID { get; set; }

        public int? Value { get; set; }

        [MaxLength(200)]
        public string Measurement_Unit { get; set; }

        [Required]
        public long Recipe_ID { get; set; }

        [Required]
        public long NutritionElement_ID { get; set; }

    
        [ForeignKey("NutritionElement_ID")]
        public virtual NutritionElement NutritionElement { get; set; }
      
        [ForeignKey("Recipe_ID")]
        public virtual Recipe Recipe { get; set; }
    }
}
