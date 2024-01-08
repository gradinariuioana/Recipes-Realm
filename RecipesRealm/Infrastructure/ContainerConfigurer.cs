using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using BusinessLayer;
using DatabaseRepository;

namespace RecipesRealm.Infrastructure
{
    public class ContainerConfigurer
    {
        public static void ConfigureContainer()
        {
            var builder = new ContainerBuilder();

            // Register dependencies in controllers
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // Register individual types 
            builder.RegisterType<RecipesRealmContext>().As<IRecipesRealmContext>();
            builder.RegisterType<RecipeRecommendationEngine>().As<IRecipeRecommendationEngine>();

            // Get your HttpConfiguration.
            var config = GlobalConfiguration.Configuration;

            // Set the dependency resolver to be Autofac.
            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }
    }
}