using Library.DataAccess.Data;
using Library.DataAccess.Repository.Interfaces;
using Library.Models;

namespace Library.DataAccess.Repository
{
	public class PaymentDetailRepository : Repository<PaymentDetail>, IPaymentDetailRepository
	{
		public PaymentDetailRepository(ApplicationDbContext db) : base(db)
		{
		}
	}
}
