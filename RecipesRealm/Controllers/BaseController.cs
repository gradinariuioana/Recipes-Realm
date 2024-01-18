using BusinessLayer.Interfaces;
using NLog;
using RecipesRealm.ViewModels;
using System.Collections.Generic;
using System.Web.Mvc;

namespace RecipesRealm.Controllers {
    public class BaseController : Controller {
        public HeaderViewModel headerVM;
        public IRecipeRecommendationEngine recipeRecommendationEngine;
        protected Logger logger = LogManager.GetCurrentClassLogger();

        public BaseController(IRecipeRecommendationEngine recipeRecommendationEngine) {
            this.recipeRecommendationEngine = recipeRecommendationEngine;

            headerVM = new HeaderViewModel();

            var catgs = recipeRecommendationEngine.GetCategoriesList();
            ICollection<CategoryViewModel> catgsVMs = new List<CategoryViewModel>();

            foreach (var cat in catgs)
                catgsVMs.Add(AutoMapperConfig.Mapper.Map<CategoryViewModel>(cat));

            headerVM.Categories = catgsVMs;

            ViewBag.LayoutModel = headerVM;
        }
    }
}