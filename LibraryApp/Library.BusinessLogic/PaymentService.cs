using Library.BusinessLogic.Interfaces;
using Library.DataAccess.Repository.Interfaces;
using Library.Models;
using Library.Utility.Constants;
using Stripe.Checkout;

namespace Library.BusinessLogic
{
    public class PaymentService : BaseService<PaymentDetail>, IPaymentService
    {
        private readonly IPaymentDetailRepository _paymentDetailRepository;

		public PaymentService(IPaymentDetailRepository paymentDetailRepository) : base(paymentDetailRepository)
        {
            _paymentDetailRepository = paymentDetailRepository;
        }

        public void ApprovePayment(int paymentId, string sessionId)
        {
			var sessionService = new SessionService();
			var session = sessionService.Get(sessionId);
			if (session != null && session.PaymentStatus.ToLower() == "paid")
			{
				UpdateStripePaymentDetails(paymentId, session.Id, session.PaymentIntentId);
				UpdateStatus(paymentId, PaymentStatus.Approved);
			}
		}

        public void UpdateStripePaymentDetails(int id, string sessionId, string paymentIntentId)
        {
            var payment = GetById(id);
            if(payment != null)
            {
                if (!string.IsNullOrEmpty(sessionId))
                {
                    payment.SessionId = sessionId;
                }
                if (!string.IsNullOrEmpty(paymentIntentId))
                {
                    payment.PaymentIntentId = paymentIntentId;
                    payment.Date = DateTime.Now;
                }
                Update(payment);
            }
        }

		public void UpdateStatus(int id, string status)
		{
			var payment = _paymentDetailRepository.GetById(id);
			payment.Status = status;
			_paymentDetailRepository.Update(payment);
		}
	}
}
