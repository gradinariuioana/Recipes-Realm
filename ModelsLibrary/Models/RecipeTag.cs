using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelsLibrary
{
    [Serializable]
    public class RecipeTag
    {
        [Key]
        public long ID { get; set; }

        [Required]
        public long Tag_ID { get; set; }

        [Required]
        public long Recipe_ID { get; set; }


        [ForeignKey("Recipe_ID")]
        public virtual Recipe Recipe { get; set; }

        [ForeignKey("Tag_ID")]
        public virtual Tag Tag { get; set; }
    }
}
