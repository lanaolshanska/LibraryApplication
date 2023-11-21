using Library.DataAccess.Data;
using Library.DataAccess.Repository.Interfaces;
using Library.Models;

namespace Library.DataAccess.Repository
{
	public class ShipmentDetailRepository : Repository<ShipmentDetail>, IShipmentDetailRepository
	{
		public ShipmentDetailRepository(ApplicationDbContext db) : base(db)
		{
		}
	}
}
