﻿using Library.BusinessLogic;
using Library.BusinessLogic.Interfaces;
using Library.BusinessLogic.Payments;
using Library.DataAccess.Repository.Interfaces;
using Library.Models;
using Library.Models.ViewModels;
using Library.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Stripe.Checkout;
using System.Security.Claims;

namespace LibraryApp.Areas.Customer.Controllers
{
    [Area(Role.Customer)]
    [Authorize]
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IApplicationUserRepository _userRepository;
        private readonly IPaymentService _paymentService;
        private readonly IAddressService _addressService;
        private readonly IOrderService _orderService;

        public ShoppingCartController(IShoppingCartRepository shoppingCartRepository,
                                    IApplicationUserRepository userRepository,
                                    IPaymentService paymentService,
                                    IAddressService addressService,
                                    IOrderService orderService
                                    )
        {
            _shoppingCartRepository = shoppingCartRepository;
            _userRepository = userRepository;
            _paymentService = paymentService;
            _addressService = addressService;
            _orderService = orderService;
        }

        public IActionResult Index()
        {
            var userId = GetApplicationUserId();
            var userShoppingCarts = _shoppingCartRepository.GetByUserId(userId);

            var shoppingCartModel = new ShoppingCartVM
            {
                ShoppingCartList = userShoppingCarts,
                OrderTotal = userShoppingCarts.Sum(x => x.Count * x.Product.Price)
            };

            return View(shoppingCartModel);
        }

        public IActionResult Summary()
        {
            var userId = GetApplicationUserId();
            var products = _shoppingCartRepository.GetByUserId(userId);

            var primaryAddressId = _addressService.GetPrimaryUserAddress(userId)?.Id;

            var summaryViewModel = new SummaryVM
            {
                ProductList = products,
                OrderTotal = products.Sum(x => x.Count * x.Product.Price),
                Address = new UserAddress { Id = primaryAddressId ?? 0 }
            };
            return View(summaryViewModel);
        }

        [HttpPost]
        public IActionResult CreateOrder(SummaryVM summaryViewModel)
        {
            if (ModelState.IsValid)
            {
                var userId = GetApplicationUserId();
                var user = _userRepository.GetById(userId);
                var shoppingCarts = _shoppingCartRepository.GetByUserId(userId).ToList();

                var order = _orderService.CreateOrder(user, summaryViewModel.Address, shoppingCarts);

                if (!user.CompanyId.HasValue)
                {
                    var successUrl = $"Customer/ShoppingCart/OrderConfirmation?id={order.Id}";
                    var cancelUrl = "Customer/ShoppingCart/Summary";

                    var stripeService = new StripeService();
                    var stripeSession = stripeService.CreateStripeSession(successUrl, cancelUrl, order.Products);

                    _paymentService.UpdateStripePaymentDetails(order.PaymentDetailId, stripeSession.Id, stripeSession.PaymentIntentId);
                    
                    Response.Headers.Add("Location", stripeSession.Url);
                    return new StatusCodeResult(303);
                }

                return RedirectToAction(nameof(OrderConfirmation), new
                {
                    id = order.Id
                });
            }
            TempData["errorMessage"] = "Address is not valid!";
            return RedirectToAction(nameof(Summary));
        }

        public IActionResult OrderConfirmation(int id)
        {
            var order = _orderService.GetById(id);
            var payment = _paymentService.GetById(order.PaymentDetailId);
            if(payment.Status != PaymentStatus.Delayed)
            {
                var sessionService = new SessionService();
                var session = sessionService.Get(payment.SessionId);
                if(session.PaymentStatus.ToLower() == "paid")
                {
                    _paymentService.UpdateStripePaymentDetails(payment.Id, session.Id, session.PaymentIntentId);
                    
                    order.Status = OrderStatus.Approved;
                    _orderService.Update(order);

                    payment.Status = PaymentStatus.Approved;
                    _paymentService.Update(payment);
                }
            }
            var oldShoppingCarts = _shoppingCartRepository.GetByUserId(order.ApplicationUserId).ToList();
            _shoppingCartRepository.RemoveRange(oldShoppingCarts);

            return View(id);
        }

        public IActionResult Plus(int id)
        {
            var order = _shoppingCartRepository.GetById(id);
            if (order != null)
            {
                order.Count++;
                _shoppingCartRepository.Update(order);
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Minus(int id)
        {
            var order = _shoppingCartRepository.GetById(id);
            if (order != null)
            {
                if (order.Count > 1)
                {
                    order.Count--;
                    _shoppingCartRepository.Update(order);
                }
                else
                {
                    _shoppingCartRepository.Delete(id);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var order = _shoppingCartRepository.GetById(id);
            if (order != null)
            {
                _shoppingCartRepository.Delete(id);
            }
            return RedirectToAction(nameof(Index));
        }

        private string GetApplicationUserId()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            return userId;
        }

        #region ApiCalls

        [HttpGet]
        public IActionResult GetAddress(int id)
        {
            var address = _addressService.GetById(id);
            if (address != null)
            {
                return Json(new { success = true, address = address });
            }
            else
            {
                return Json(new { success = false, message = "Can not retrieve address!" });
            }
        }
        #endregion
    }
}
