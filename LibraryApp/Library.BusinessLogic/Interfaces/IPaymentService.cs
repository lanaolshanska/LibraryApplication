﻿using Library.Models;

namespace Library.BusinessLogic.Interfaces
{
	public interface IPaymentService : IBaseService<PaymentDetail>
	{
		void UpdateStripePaymentDetails(int id, string sessionId, string paymentIntentId);
		void UpdateStatus(int id, string status);
		void ApprovePayment(int paymentId, string sessionId);

	}
}
