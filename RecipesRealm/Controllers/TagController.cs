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
            IEnumerable<Tag> tags = DataAccess.TagAccessor.GetTagsList();

            foreach (Tag tag in tags)
            {
                var tagViewModel = new TagViewModel
                {
                    Tag_ID = tag.Tag_ID,
                    Tag_Name = tag.Tag_Name,
                    Tag_Recipes = string.Join(", ", DataAccess.RecipeTagAccessor.GetRecipesForTag(tag.Tag_ID).Select(r => r.Recipe_Name).ToList())
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
                if (ModelState.IsValid) {

                    var tagExistsInDB = DataAccess.TagAccessor.CheckTagExists(model.Tag_Name);

                    if (tagExistsInDB) {
                        ViewBag.Warning = "There is already a tag with this name in the DataBase";
                        return View(model);
                    }

                    Tag tag = new Tag {
                        Tag_Name = model.Tag_Name
                    };

                    var tId = DataAccess.TagAccessor.AddTag(tag);

                    return RedirectToAction("Details", new { id = tId });
                }

                ViewBag.Warning = "Could not create Tag";
                return View(model);
            }
            catch (Exception ex)
            {
                Utils.WriteToLog("Tag", "Create", ex.ToString());
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

                tagViewModel.Tag_ID = tag.Tag_ID;
                tagViewModel.Tag_Name = tag.Tag_Name;
                tagViewModel.Tag_Recipes = string.Join(", ", DataAccess.RecipeTagAccessor.GetRecipesForTag(tag.Tag_ID).Select(r => r.Recipe_Name).ToList());

                return View(tagViewModel);
            }
            catch (Exception ex)
            {
                Utils.WriteToLog("Tag", "Details", ex.ToString());
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

                tagViewModel.Tag_ID = tag.Tag_ID;
                tagViewModel.Tag_Name = tag.Tag_Name;
                tagViewModel.Tag_Recipes = string.Join(", ", DataAccess.RecipeTagAccessor.GetRecipesForTag(tag.Tag_ID).Select(r => r.Recipe_Name).ToList());

                return View(tagViewModel);
            }
            catch (Exception ex)
            {
                Utils.WriteToLog("Tag", "Edit", ex.ToString());
                return new HttpStatusCodeResult(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }


        // POST: Tag/Edit/id
        [HttpPost]
        public ActionResult Edit(long id, TagViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var tagExistsInDB = DataAccess.TagAccessor.CheckTagExists(model.Tag_Name);

                    if (tagExistsInDB)
                    {
                        ViewBag.Warning = "There is already a tag with this name in the DataBase";
                        return View(model);
                    }

                    Tag newTag = new Tag
                    {
                        Tag_Name = model.Tag_Name,
                        Tag_ID = id
                    };

                    DataAccess.TagAccessor.EditTag(newTag);

                    return RedirectToAction("Details", new { id = id });
                }

                ViewBag.Warning = "Could not edit Tag";
                return View(model);
            }
            catch (Exception ex)
            {
                Utils.WriteToLog("Tag", "Edit", ex.ToString());
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

                tagViewModel.Tag_ID = tag.Tag_ID;
                tagViewModel.Tag_Name = tag.Tag_Name;
                tagViewModel.Tag_Recipes = string.Join(", ", DataAccess.RecipeTagAccessor.GetRecipesForTag(tag.Tag_ID).Select(r => r.Recipe_Name).ToList());

                return View(tagViewModel);
            }
            catch (Exception ex)
            {
                Utils.WriteToLog("Tag", "Delete", ex.ToString());
                return new HttpStatusCodeResult(StatusCodes.Status500InternalServerError, ex.ToString());
            }
        }


        // POST: Tag/Delete/id

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteTag(long id)
        {
            try
            {
                DataAccess.TagAccessor.RemoveTag(id);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Utils.WriteToLog("Tag", "Delete", ex.ToString());
                return new HttpStatusCodeResult(StatusCodes.Status500InternalServerError, ex.ToString());
            }

        }

        #endregion Delete
    }
}