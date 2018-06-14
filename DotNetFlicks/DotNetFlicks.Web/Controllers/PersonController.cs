﻿using DotNetFlicks.Common.Configuration;
using DotNetFlicks.Managers.Interfaces;
using DotNetFlicks.ViewModels.Person;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNetFlicks.Web.Controllers
{
    [Authorize]
    public class PersonController : Controller
    {
        private IPersonManager _personManager;

        public PersonController(IPersonManager personManager)
        {
            _personManager = personManager;
        }

        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page, int? pageSize)
        {
            var request = new IndexRequest
            {
                SortOrder = sortOrder,
                Search = searchString == null ? currentFilter : searchString,
                PageIndex = searchString == null && page.HasValue ? page.Value : 1, 
                PageSize = pageSize.HasValue ? pageSize.Value : 10
            };

            ViewData["CurrentSort"] = request.SortOrder;
            ViewData["NameSortParm"] = string.IsNullOrEmpty(request.SortOrder) ? "name_desc" : "";
            ViewData["RolesSortParm"] = request.SortOrder == "Roles" ? "roles_desc" : "Roles";
            ViewData["CurrentFilter"] = request.Search;
            ViewData["PageSize"] = request.PageSize;

            var vms = _personManager.GetRequest(request);

            return View(vms);
        }

        public ActionResult View(int id)
        {
            var vm = _personManager.Get(id);

            return View(vm);
        }

        public ActionResult Edit(int? id)
        {
            var vm = _personManager.Get(id);

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PersonViewModel vm)
        {
            if (ModelState.IsValid)
            {
                _personManager.Save(vm);
                return RedirectToAction("Index");
            }

            return View(vm);
        }

        public ActionResult Delete(int id)
        {
            var vm = _personManager.Get(id);

            return View(vm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _personManager.Delete(id);

            return RedirectToAction("Index");
        }
    }
}
