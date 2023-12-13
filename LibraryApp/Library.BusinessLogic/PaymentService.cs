using Library.BusinessLogic.Interfaces;
using Library.DataAccess.Repository.Interfaces;
using Library.Models;

namespace Library.BusinessLogic
{
    public class PaymentService : BaseService<PaymentDetail>, IPaymentService
    {
        private IPaymentDetailRepository _paymentDetailRepository;

		public PaymentService(IPaymentDetailRepository paymentDetailRepository) : base(paymentDetailRepository)
        {
            _paymentDetailRepository = paymentDetailRepository;
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
