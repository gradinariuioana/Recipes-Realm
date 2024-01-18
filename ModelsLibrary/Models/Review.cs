using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelsLibrary
{
    [Serializable]
    public class Review
    {
        [Key]
        public long Review_ID { get; set; }

        public string Review_Text { get; set; }

        [Required]
        public DateTime Review_Date { get; set; }

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
