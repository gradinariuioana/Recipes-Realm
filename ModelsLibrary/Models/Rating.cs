using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ModelsLibrary
{
    [Serializable]
    public class Rating
    {
        [Key]
        public long Rating_ID { get; set; }

        [Required]
        public float Rating_Value { get; set; }

        [Required]
        public DateTime Rating_Date { get; set; }

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
