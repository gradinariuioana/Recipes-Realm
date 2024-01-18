using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelsLibrary
{
    [Serializable]
    public class RecipeStep
    {
        [Key]
        public long Step_ID { get; set; }

        public int? Step_Number { get; set; }
        public string Step_Description { get; set; }

        [MaxLength(300)]
        public string Step_Title { get; set; }

        public bool? IsOptional { get; set; }
        public string Picture_Path { get; set; }

        [Required]
        public long Recipe_ID { get; set; }

    
        [ForeignKey("Recipe_ID")]
        public virtual Recipe Recipe { get; set; }
    }
}
