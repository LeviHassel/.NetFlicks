﻿using DotNetFlicks.Common.Models;
using DotNetFlicks.Managers.Interfaces;
using DotNetFlicks.ViewModels.Genre;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNetFlicks.Web.Controllers
{
    [Authorize]
    public class GenreController : Controller
    {
        private IGenreManager _genreManager;

        public GenreController(IGenreManager genreManager)
        {
            _genreManager = genreManager;
        }

        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page, int? pageSize)
        {
            sortOrder = sortOrder ?? (string)TempData["GenreIndexRequest_SortOrder"];
            currentFilter = currentFilter ?? (string)TempData["GenreIndexRequest_CurrentFilter"];
            searchString = searchString ?? (string)TempData["GenreIndexRequest_SearchString"];
            page = page ?? (int?)TempData["GenreIndexRequest_Page"];
            pageSize = pageSize ?? (int?)TempData["GenreIndexRequest_PageSize"];

            TempData["GenreIndexRequest_SortOrder"] = sortOrder;
            TempData["GenreIndexRequest_CurrentFilter"] = currentFilter;
            TempData["GenreIndexRequest_SearchString"] = searchString;
            TempData["GenreIndexRequest_Page"] = page;
            TempData["GenreIndexRequest_PageSize"] = pageSize;

            TempData.Keep();

            var request = new DataTableRequest(sortOrder, currentFilter, searchString, page, pageSize);

            var vms = _genreManager.GetAllByRequest(request);

            return View(vms);
        }

        public ActionResult View(int id)
        {
            var vm = _genreManager.Get(id);

            return View(vm);
        }

        public ActionResult Edit(int? id)
        {
            var vm = _genreManager.Get(id);

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(GenreViewModel vm)
        {
            if (ModelState.IsValid)
            {
                _genreManager.Save(vm);
                return RedirectToAction("Index");
            }

            return View(vm);
        }

        public ActionResult Delete(int id)
        {
            var vm = _genreManager.Get(id);

            return View(vm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _genreManager.Delete(id);

            return RedirectToAction("Index");
        }
    }
}
