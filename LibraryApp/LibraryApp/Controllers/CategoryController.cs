﻿namespace LibraryApp.Controllers
{
	using Data;
	using Microsoft.AspNetCore.Mvc;
	using Models;

	public class CategoryController : Controller
	{
		private readonly ApplicationDbContext _db;

		public CategoryController(ApplicationDbContext db)
		{
			_db = db;
		}

		public IActionResult Index()
		{
			var categories = _db.Categories.ToList();
			return View(categories);
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Create(Category category)
		{
			if (ModelState.IsValid)
			{
				_db.Categories.Add(category);
				_db.SaveChanges();
				return RedirectToAction("Index");
			}
			return View();
		}

		public IActionResult Update(int? id)
		{
			if (id.HasValue)
			{
				var category = _db.Categories.FirstOrDefault(x => x.Id == id.Value);
				if (category != null)
				{
					return View(category);
				}
			}
			return NotFound();
		}

		[HttpPost]
		public IActionResult Update(Category category)
		{
			if (ModelState.IsValid)
			{
				_db.Categories.Update(category);
				_db.SaveChanges();
				return RedirectToAction("Index");
			}
			return View();
		}
	}
}