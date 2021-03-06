﻿using System.Net;
using System.Web.Mvc;
using SpringTest.Core.EfContext;
using SpringTest.Core.Repositories;
using SpringTest.Core.Services;
using SpringTest.Domain.Entities;
using SpringTest.Domain.Repositories;
using SpringTest.Domain.Services;

namespace SpringTest.Web.Controllers {
	public class CategoryController : Controller {

		private ICategoryRepository _categoryRepository;
		private ICategoryService _categoryService;
		public CategoryController() {
			_categoryRepository = new CategoryRepository(new EfDbContext());
			_categoryService = new CategoryService(_categoryRepository);
		}
		public ActionResult Index() {
			var model = _categoryService.GetAll();
			return View(model);
		}

		public ActionResult Details(int id) {
			if (id == 0)
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

			var model = _categoryService.Get(c => c.Id == id);
			return View(model);
		}

		public ActionResult Create() {
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(Category model) {
			try {
				if (ModelState.IsValid) {
					_categoryService.Add(model);
					_categoryService.Commit();
					return RedirectToAction("Index");
				}
			} catch {
				return View();
			}

			return View(model);
		}

		public ActionResult Edit(int id) {
			if (id == 0)
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

			var model = _categoryService.Get(c => c.Id == id);
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit(int id, Category model) {
			try {
				if (ModelState.IsValid) {
					_categoryService.Update(model);
					_categoryService.Commit();
					return RedirectToAction("Index");
				}
			} catch {
				return View();
			}
			return View(model);
		}

		public ActionResult Delete(int id) {
			if (id == 0)
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Delete(int id, Category model) {
			try {
				_categoryService.Delete(id);
				_categoryService.Commit();
				return RedirectToAction("Index");
			} catch {
				return View();
			}
		}
	}
}
