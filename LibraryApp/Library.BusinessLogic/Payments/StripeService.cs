using Library.Models;
using Stripe;
using Stripe.Checkout;

namespace Library.BusinessLogic.Payments
{
    public class StripeService
    {
        private const string domain = "http://localhost:5059/";
        private const string defaultCurrency = "usd";
        private const string defaultMode = "payment";

        public Session CreateStripeSession(string successUrl, string cancelUrl, List<OrderProduct> products, 
                                          string currency = defaultCurrency, string mode = defaultMode)
        {
            var options = new SessionCreateOptions
            {
                SuccessUrl = string.Concat(domain, successUrl),
                CancelUrl = string.Concat(domain,cancelUrl),
                LineItems = new List<SessionLineItemOptions>(),
                Mode = mode
            };

            foreach (var item in products)
            {
                var sessionLineItem = new SessionLineItemOptions
                {
                    Quantity = item.Count,
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = item.Product.Price * 100,
                        Currency = currency,
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Title
                        }
                    }
                };
                options.LineItems.Add(sessionLineItem);
            }

            var service = new SessionService();
            Session session = service.Create(options);
            return session;
        }

        public void RefundPayment(string paymentIntentId)
        {
			var options = new RefundCreateOptions
			{
				Reason = RefundReasons.RequestedByCustomer,
				PaymentIntent = paymentIntentId
			};

			var service = new RefundService();
			var refund = service.Create(options);
		}
    }
}
