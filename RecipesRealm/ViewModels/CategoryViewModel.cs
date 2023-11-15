using ModelsLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RecipesRealm.ViewModels
{
    public class CategoryViewModel
    {         
        public long Category_ID { get; set; }

        [DisplayName("Category Name")]
        [Required(ErrorMessage = "Category name is required")]
        public string Category_Name { get; set; }

        [DisplayName("Category Description")]
        public string Category_Description { get; set; }

        [DisplayName("Recipes with this Category")]
        public string Category_Recipes { get; set; }
    }
}