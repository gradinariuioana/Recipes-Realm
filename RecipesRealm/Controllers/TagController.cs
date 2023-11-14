using Microsoft.AspNetCore.Http;
using ModelsLibrary;
using RecipesRealm.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace RecipesRealm.Controllers
{
    public class TagController : Controller
    {
        public ActionResult Index()
        {
            IEnumerable<TagViewModel> tagViewModels = new List<TagViewModel>();
            IEnumerable<Tag> tags = DataAccess.TagAccessor.GetTags();

            foreach (Tag tag in tags)
            {
                var tagViewModel = new TagViewModel
                {
                    Tag_Name = tag.Tag_Name,
                    Tag_Recipes = string.Join(", ", DataAccess.RecipeTagAccessor.GetRecipesByTag(tag.Tag_ID).Select(r => r.Recipe_Name).ToList())
                };

                tagViewModels = tagViewModels.Append(tagViewModel);
            }

            return View(tagViewModels);
        }

        #region Create

        // GET: Tag/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tag/Create/
        [HttpPost]
        public ActionResult Create(TagViewModel model)
        {
            try
            {
                Tag tag = new Tag
                {
                    Tag_Name = model.Tag_Name
                };

                var tId = DataAccess.TagAccessor.AddTag(tag);

                return RedirectToAction("Details", tId);
            }
            catch (Exception ex)
            {
                Utils.WriteToLog("Tags", "Create", ex.ToString());
                return new HttpStatusCodeResult(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }

        #endregion Create

        #region Details

        // GET: Tag/Details/id
        public ActionResult Details(long id)
        {
            try
            {
                var tag = DataAccess.TagAccessor.GetTag(id);
                TagViewModel tagViewModel = new TagViewModel();

                if (tag == null)
                {
                    ViewBag.Warning = "The tag could not be found";
                    return View(tagViewModel);
                }

                tagViewModel.Tag_Name = tag.Tag_Name;
                tagViewModel.Tag_Recipes = string.Join(", ", DataAccess.RecipeTagAccessor.GetRecipesByTag(tag.Tag_ID).Select(r => r.Recipe_Name).ToList());

                return View(tagViewModel);
            }
            catch (Exception ex)
            {
                Utils.WriteToLog("Tags", "Details", ex.ToString());
                return new HttpStatusCodeResult(StatusCodes.Status500InternalServerError, ex.ToString());
            }

        }

        #endregion Details

        #region Edit

        // GET: Tag/Edit/id
        public ActionResult Edit(long id)
        {
            try
            {
                var tag = DataAccess.TagAccessor.GetTag(id);
                TagViewModel tagViewModel = new TagViewModel();

                if (tag == null)
                {
                    ViewBag.Warning = "The tag could not be found";
                    return View(tagViewModel);
                }

                tagViewModel.Tag_Name = tag.Tag_Name;
                tagViewModel.Tag_Recipes = string.Join(", ", DataAccess.RecipeTagAccessor.GetRecipesByTag(tag.Tag_ID).Select(r => r.Recipe_Name).ToList());

                return View(tagViewModel);
            }
            catch (Exception ex)
            {
                Utils.WriteToLog("Tags", "Edit", ex.ToString());
                return new HttpStatusCodeResult(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }


        // POST: Tag/Edit/id
        [HttpPost]
        public ActionResult Edit(long id, TagViewModel model)
        {
            try
            {
                Tag tag = DataAccess.TagAccessor.GetTag(id);
                if (tag == null)
                {
                    ViewBag.Warning = "The tag could not be found";
                    return View(model);
                }

                tag.Tag_Name = model.Tag_Name;
                DataAccess.TagAccessor.EditTag(tag);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Utils.WriteToLog("Tags", "Edit", ex.ToString());
                return new HttpStatusCodeResult(StatusCodes.Status500InternalServerError, ex.ToString());
            }

        }

        #endregion Edit

        #region Delete

        // GET: Tag/Edit/id
        public ActionResult Delete(long id)
        {
            try
            {
                var tag = DataAccess.TagAccessor.GetTag(id);
                TagViewModel tagViewModel = new TagViewModel();

                if (tag == null)
                {
                    ViewBag.Warning = "The tag could not be found";
                    return View(tagViewModel);
                }

                tagViewModel.Tag_Name = tag.Tag_Name;
                tagViewModel.Tag_Recipes = string.Join(", ", DataAccess.RecipeTagAccessor.GetRecipesByTag(tag.Tag_ID).Select(r => r.Recipe_Name).ToList());

                return View(tagViewModel);
            }
            catch (Exception ex)
            {
                Utils.WriteToLog("Tags", "Delete", ex.ToString());
                return new HttpStatusCodeResult(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }


        // POST: Tag/Delete/id
        [HttpPost]
        public ActionResult Delete(long id, TagViewModel model)
        {
            try
            {
                Tag tag = DataAccess.TagAccessor.GetTag(id);
                if (tag == null)
                {
                    ViewBag.Warning = "The tag could not be found";
                    return View(model);
                }

                DataAccess.TagAccessor.RemoveTag(tag);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Utils.WriteToLog("Tags", "Delete", ex.ToString());
                return new HttpStatusCodeResult(StatusCodes.Status500InternalServerError, ex.ToString());
            }

        }

        #endregion Delete
    }
}