namespace LibraryApp.Areas.Admin.Controllers
{
    using Library.DataAccess.Repository.Interfaces;
    using Library.Models;
    using Library.Utility.Constants;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Area(Role.Admin)]
    [Authorize(Roles=Role.Admin)]
	public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public IActionResult Index()
        {
            var categories = _categoryRepository.GetAll();
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
                _categoryRepository.Create(category);
                TempData["successMessage"] = "Item was successfully created!";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Update(int? id)
        {
            if (id.HasValue)
            {
                var category = _categoryRepository.GetById(id.Value);
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
                _categoryRepository.Update(category);
                TempData["successMessage"] = "Item was successfully updated!";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Delete(int? id)
        {
            if (id.HasValue)
            {
                _categoryRepository.Delete(id.Value);
                TempData["warningMessage"] = "Item was deleted!";
                return RedirectToAction("Index");
            }
            return NotFound();
        }
    }
}
