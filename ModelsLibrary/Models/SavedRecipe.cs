using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelsLibrary
{
    public class SavedRecipe
    {
        [Key]
        public long ID { get; set; }

        [Required]
        public DateTime Saving_Date { get; set; }

        [Required]
        public long User_ID { get; set; }

        [Required]
        public long Recipe_ID { get; set; }


        [ForeignKey("Recipe_ID")]
        public virtual Recipe Recipe { get; set; }

        [ForeignKey("User_ID")]
        public virtual User User { get; set; }
    }
}
