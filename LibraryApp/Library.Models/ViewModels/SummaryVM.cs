namespace Library.Models.ViewModels
{
    public class SummaryVM
    {
		public IEnumerable<ShoppingCart> ProductList { get; set; }
        public double OrderTotal { get; set; }
		public UserAddress Address { get; set; }
		public DateTime MinArrivalDate { get; set; } = DateTime.Now.AddDays(7);
        public DateTime MaxArrivalDate { get; set; } = DateTime.Now.AddDays(14);
    }
}
