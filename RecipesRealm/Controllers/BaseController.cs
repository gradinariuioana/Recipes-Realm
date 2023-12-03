using RecipesRealm.ViewModels;
using System.Collections.Generic;
using System.Web.Mvc;

namespace RecipesRealm.Controllers
{
    public class BaseController : Controller
    {
        public HeaderViewModel headerVM;

        public BaseController()
        {
            // Here you will use some business logic to populate your Layout Model
            // You might also consider placing this model into the cache to prevent constant fetching of data from the database on each page request.
            headerVM = new HeaderViewModel();

            var catgs = DataAccess.CategoryAccessor.GetCategoriesList();
            ICollection<CategoryViewModel> catgsVMs = new List<CategoryViewModel>();

            foreach(var cat in catgs)
                catgsVMs.Add(AutoMapperConfig.Mapper.Map<CategoryViewModel>(cat));

            headerVM.Categories = catgsVMs;

            ViewBag.LayoutModel = headerVM;
        }
    }
}