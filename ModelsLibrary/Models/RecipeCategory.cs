using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelsLibrary
{
    public class RecipeCategory
    {
        [Key]
        public long ID { get; set; }

        [Required]
        public long Category_ID { get; set; }

        [Required]
        public long Recipe_ID { get; set; }


        [ForeignKey("Category_ID")]
        public virtual Category Category { get; set; }

        [ForeignKey("Recipe_ID")]
        public virtual Recipe Recipe { get; set; }
    }
}
