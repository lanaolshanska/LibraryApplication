using Library.BusinessLogic.Interfaces;
using Library.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.Areas.Admin.Controllers
{
    [Area(Role.Admin)]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region ApiCalls

        public IActionResult GetAll()
        {
            var orders = _orderService.GetAll();
            return Json(new { data = orders });
        }

        #endregion
    }
}
