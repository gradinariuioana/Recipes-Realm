using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ModelsLibrary
{
    [Serializable]
    public class Tag
    {    
        [Key]
        public long Tag_ID { get; set; }

        [Required]
        [MaxLength(200)]
        public string Tag_Name { get; set; }
    
        public virtual ICollection<RecipeTag> RecipeTags { get; set; }
    }
}
