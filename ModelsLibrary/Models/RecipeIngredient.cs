using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelsLibrary
{
    [Serializable]
    public class RecipeIngredient
    {
        [Key]
        public long ID { get; set; }

        [Required]
        public long Quantity { get; set; }

        [MaxLength(200)]
        public string Measurement_Unit { get; set; }

        public bool? IsOptional { get; set; }

        [MaxLength(300)]
        public string Other_Info { get; set; }

        [Required]
        public long Recipe_ID { get; set; }

        [Required]
        public long Ingredient_ID { get; set; }
    

        [ForeignKey("Ingredient_ID")]
        public virtual Ingredient Ingredient { get; set; }

        [ForeignKey("Recipe_ID")]
        public virtual Recipe Recipe { get; set; }
    }
}
