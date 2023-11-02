using Library.DataAccess.Repository.Interfaces;
using Library.Models;
using Library.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.Areas.Admin.Controllers
{
	[Area(Role.Admin)]
	[Authorize(Roles = Role.Admin)]
	public class CompanyController : Controller
	{
		private readonly ICompanyRepository _companyRepository;

		public CompanyController(ICompanyRepository companyRepository)
		{
			_companyRepository = companyRepository;
		}

		public IActionResult Index()
		{
			var companies = _companyRepository.GetAll();
			return View(companies);
		}

		public IActionResult CreateOrUpdate(int? id)
		{
			var company = id.HasValue ? _companyRepository.GetById(id.Value) : new Company();
			return View(company);
		}

		[HttpPost]
		public IActionResult CreateOrUpdate(Company company)
		{
			if (ModelState.IsValid)
			{
				var oldCompany = _companyRepository.GetById(company.Id);
				if (oldCompany == null)
				{
					_companyRepository.Create(company);
				}
				else
				{
					_companyRepository.Update(company);
				}
				return RedirectToAction("Index");
			}
			else
			{
				var companies = _companyRepository.GetAll();
				return View(companies);
			}
		}

		#region ApiCalls

		public IActionResult GetAll()
		{
			var companies = _companyRepository.GetAll();
			return Json(new { data = companies });
		}

		[HttpDelete]
		public IActionResult Delete(int id)
		{
			var company = _companyRepository.GetById(id);
			if (company == null)
			{
				return Json(new { success = false, message = "Item for deleting not found!" });
			}
			else
			{
				_companyRepository.Delete(id);
				return Json(new { success = true, message = "Item was deleted!" });
			}
		}

		#endregion
	}
}