using ModelsLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RecipesRealm.ViewModels
{
    public class NutritionElementViewModel
    {
        public long ID { get; set; }
        public string Element_Name { get; set; }

        public string Element_Description { get; set; }

        public long? Value { get; set; }

        public string Measurement_Unit { get; set; }
    }
}