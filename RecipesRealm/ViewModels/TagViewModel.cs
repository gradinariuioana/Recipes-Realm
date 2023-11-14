using ModelsLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RecipesRealm.ViewModels
{
    public class TagViewModel
    { 
        [DisplayName("Tag Name")]
        [Required(ErrorMessage = "Tag name is required")]
        public string Tag_Name { get; set; }

        [DisplayName("Recipes with this Tag")]
        public string Tag_Recipes { get; set; }
    }
}