using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ModelsLibrary
{
    public class User
    {    
        [Key]
        public long User_ID { get; set; }

        [Required]
        [MaxLength(300)]
        public string User_Name { get; set; }

        [Required]
        [MaxLength(32)]
        public byte[] Password { get; set; }

        [Required]
        [MaxLength(300)]
        public string Email_Address { get; set; }

        public string Profile_Picture_Path { get; set; }

        [Required]
        public DateTime Registration_Date { get; set; }
    
        public virtual ICollection<Rating> Ratings { get; set; }
        public virtual ICollection<Recipe> Recipes { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<SavedRecipe> SavedRecipes { get; set; }
    }
}
