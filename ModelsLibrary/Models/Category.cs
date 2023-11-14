using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ModelsLibrary
{
    public class Category
    {
        [Key]
        public long Category_ID { get; set; }

        [Required]
        [MaxLength(200)]
        public string Category_Name { get; set; }

        public string Category_Description { get; set; }
    
        public virtual ICollection<RecipeCategory> RecipeCategories { get; set; }
    }
}
