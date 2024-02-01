using Microsoft.AspNetCore.Mvc.Rendering;

namespace Library.Models.ViewModels
{
	public class RoleManagementVM
	{
		public string UserId { get; set; }
		public string Email { get; set; }
		public string Role { get; set; }
		public int? CompanyId { get; set; }
		public IEnumerable<SelectListItem> CompanyList { get; set; }
		public List<SelectListItem> RoleList { get; set; }

	}
}
