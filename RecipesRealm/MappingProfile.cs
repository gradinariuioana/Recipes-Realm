﻿using AutoMapper;
using ModelsLibrary;
using RecipesRealm.ViewModels;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace RecipesRealm
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Recipe, RecipeViewModel>()
                .ForMember(rVM => rVM.RecipeSteps, opt => opt.MapFrom<RecipeStepConverter>())
                .ForMember(rVM => rVM.RecipeCategories, opt => opt.MapFrom<RecipeCategoryConverter>())
                .ForMember(rVM => rVM.RecipeIngredients, opt => opt.MapFrom<RecipeIngredientConverter>())
                .ForMember(rVM => rVM.RecipeTags, opt => opt.MapFrom<RecipeTagConverter>())
                .ForMember(rVM => rVM.RecipeNutritionElements, opt => opt.MapFrom<RecipeNutritionElementConverter>());

            CreateMap<RecipeViewModel, Recipe>();

            CreateMap<Tag, TagViewModel>();
            CreateMap<TagViewModel, Tag>();

            CreateMap<Ingredient, IngredientViewModel>();
            CreateMap<IngredientViewModel, Ingredient>();

            CreateMap<NutritionElement, NutritionElementViewModel>()
                 .ForMember(nVM => nVM.NutritionElement_ID, opt => opt.MapFrom(r => r.ID));
            CreateMap<NutritionElementViewModel, NutritionElement>()
                .ForMember(nVM => nVM.ID, opt => opt.MapFrom(r => r.NutritionElement_ID));

            CreateMap<Category, CategoryViewModel>();
            CreateMap<CategoryViewModel, Category>();

            CreateMap<RecipeTag, TagViewModel>()
                .ForMember(tVM => tVM.Tag_Name, opt => opt.MapFrom(t => t.Tag.Tag_Name));
            CreateMap<TagViewModel, RecipeTag>();

            CreateMap<RecipeIngredient, IngredientViewModel>()
            .ForMember(nVM => nVM.Ingredient_Name, opt => opt.MapFrom(r => r.Ingredient.Ingredient_Name))
            .ForMember(nVM => nVM.Category, opt => opt.MapFrom(r => r.Ingredient.Category))
            .ForMember(nVM => nVM.IsOptional, opt => opt.MapFrom(r => r.IsOptional ?? false));
            CreateMap<IngredientViewModel, RecipeIngredient>();

            CreateMap<RecipeNutritionElement, NutritionElementViewModel>()
                .ForMember(nVM => nVM.Element_Name, opt => opt.MapFrom(r => r.NutritionElement.Element_Name))
                .ForMember(nVM => nVM.Element_Description, opt => opt.MapFrom(r => r.NutritionElement.Element_Description));
            CreateMap<NutritionElementViewModel, RecipeNutritionElement>();

            CreateMap<RecipeCategory, CategoryViewModel>()
                .ForMember(cVM => cVM.Category_Name, opt => opt.MapFrom(c => c.Category.Category_Name))
                .ForMember(cVM => cVM.Category_Description, opt => opt.MapFrom(c => c.Category.Category_Description));
            CreateMap<CategoryViewModel, RecipeCategory>();

            CreateMap<RecipeStep, RecipeStepViewModel>()
                .ForMember(cVM => cVM.IsOptional, opt => opt.MapFrom(c => c.IsOptional ?? false));
            CreateMap<RecipeStepViewModel, RecipeStep>();

            CreateMap<UserViewModel, User>()
                .ForMember(uVM => uVM.Password, opt => opt.MapFrom<PasswordConverter>());

            CreateMap<RatingViewModel, Rating>();
            CreateMap<SavedRecipeViewModel, SavedRecipe>();
        }
        public class RecipeTagConverter : IValueResolver<Recipe, RecipeViewModel, ICollection<TagViewModel>>
        {
            public ICollection<TagViewModel> Resolve(Recipe source, RecipeViewModel dest, ICollection<TagViewModel> destMember, ResolutionContext context)
            {
                ICollection<TagViewModel> tVMs = new List<TagViewModel>();

                foreach(var t in source.RecipeTags)
                {
                    var tVM = context.Mapper.Map<TagViewModel>(t);
                    tVMs.Add(tVM);
                }

                return tVMs;
            }
        }

        public class RecipeIngredientConverter : IValueResolver<Recipe, RecipeViewModel, ICollection<IngredientViewModel>>
        {
            public ICollection<IngredientViewModel> Resolve(Recipe source, RecipeViewModel dest, ICollection<IngredientViewModel> destMember, ResolutionContext context)
            {
                ICollection<IngredientViewModel> iVMs = new List<IngredientViewModel>();

                foreach (var i in source.RecipeIngredients)
                {
                    var iVM = context.Mapper.Map<IngredientViewModel>(i);
                    iVMs.Add(iVM);
                }

                return iVMs;
            }
        }

        public class RecipeStepConverter : IValueResolver<Recipe, RecipeViewModel, ICollection<RecipeStepViewModel>>
        {
            public ICollection<RecipeStepViewModel> Resolve(Recipe source, RecipeViewModel dest, ICollection<RecipeStepViewModel> destMember, ResolutionContext context)
            {
                ICollection<RecipeStepViewModel> rsVMs = new List<RecipeStepViewModel>();

                foreach (var s in source.RecipeSteps)
                {
                    var rsVM = context.Mapper.Map<RecipeStepViewModel>(s);
                    rsVMs.Add(rsVM);
                }

                return rsVMs;
            }
        }

        public class RecipeNutritionElementConverter : IValueResolver<Recipe, RecipeViewModel, ICollection<NutritionElementViewModel>>
        {
            public ICollection<NutritionElementViewModel> Resolve(Recipe source, RecipeViewModel dest, ICollection<NutritionElementViewModel> destMember, ResolutionContext context)
            {
                ICollection<NutritionElementViewModel> neVMs = new List<NutritionElementViewModel>();

                foreach (var ne in source.RecipeNutritionElements)
                {
                    var neVM = context.Mapper.Map<NutritionElementViewModel>(ne);
                    neVMs.Add(neVM);
                }

                return neVMs;
            }
        }

        public class RecipeCategoryConverter : IValueResolver<Recipe, RecipeViewModel, ICollection<CategoryViewModel>>
        {
            public ICollection<CategoryViewModel> Resolve(Recipe source, RecipeViewModel dest, ICollection<CategoryViewModel> destMember, ResolutionContext context)
            {
                ICollection<CategoryViewModel> cVMs = new List<CategoryViewModel>();

                foreach (var c in source.RecipeCategories)
                {
                    var cVM = context.Mapper.Map<CategoryViewModel>(c);
                    cVMs.Add(cVM);
                }

                return cVMs;
            }
        }

        public class PasswordConverter : IValueResolver<UserViewModel, User, byte[]> {
            public byte[] Resolve(UserViewModel source, User dest, byte[] destMember, ResolutionContext context) {
                string passString = source.Password;

                using (SHA256 sha256Hash = SHA256.Create()) {
                    // ComputeHash - returns byte array
                    byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(passString));

                    return bytes;
                }
            }
        }

    }

    public static class AutoMapperConfig
    {
        private static IMapper _mapper;

        public static IMapper Mapper
        {
            get
            {
                // Lazy initialization to ensure that the configuration is set up only once
                if (_mapper == null)
                {
                    Initialize();
                }

                return _mapper;
            }
        }

        public static void Initialize()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });
            
            _mapper = config.CreateMapper();
        }
    }
}