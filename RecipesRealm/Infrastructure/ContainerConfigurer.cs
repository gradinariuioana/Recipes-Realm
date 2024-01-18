using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using BusinessLayer;
using BusinessLayer.Interfaces;
using DataAccess;
using DatabaseRepository;

namespace RecipesRealm.Infrastructure
{
    public class ContainerConfigurer
    {
        public static void ConfigureContainer()
        {
            var builder = new ContainerBuilder();

            // Register dependencies in controllers
            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            // Register individual types 
            builder.RegisterType<RecipesRealmContext>().As<IRecipesRealmContext>();
            builder.RegisterType<RecipeRecommendationEngine>().As<IRecipeRecommendationEngine>();

            builder.RegisterType<MemoryCacheService>().As<ICache>();

            builder.RegisterType<CategoryAccessor>().As<ICategoryAccessor>();
            builder.RegisterType<IngredientAccessor>().As<IIngredientAccessor>();
            builder.RegisterType<NutritionElementAccessor>().As<INutritionElementAccessor>();
            builder.RegisterType<RatingAccessor>().As<IRatingAccessor>();
            builder.RegisterType<RecipeAccessor>().As<IRecipeAccessor>();
            builder.RegisterType<RecipeCategoryAccessor>().As<IRecipeCategoryAccessor>();
            builder.RegisterType<RecipeIngredientAccessor>().As<IRecipeIngredientAccessor>();
            builder.RegisterType<RecipeNutritionElementAccessor>().As<IRecipeNutritionElementAccessor>();
            builder.RegisterType<RecipeStepAccessor>().As<IRecipeStepAccessor>();
            builder.RegisterType<RecipeTagAccessor>().As<IRecipeTagAccessor>();
            builder.RegisterType<ReviewAccessor>().As<IReviewAccessor>();
            builder.RegisterType<SavedRecipeAccessor>().As<ISavedRecipeAccessor>();
            builder.RegisterType<TagAccessor>().As<ITagAccessor>();
            builder.RegisterType<UserAccessor>().As<IUserAccessor>();

            // Get your HttpConfiguration.
            var config = GlobalConfiguration.Configuration;

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
       
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}