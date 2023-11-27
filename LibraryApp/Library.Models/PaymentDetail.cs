using System.ComponentModel.DataAnnotations;

namespace Library.Models
{
	public class PaymentDetail
	{
		[Key]
		public int Id { get; set; }
		public string? Status { get; set; }
		public DateTime Date { get; set; }
		public DateOnly DueDate { get; set; }

        public string? SessionId { get; set; }
        public string? PaymentIntentId { get; set; }
	}
}
