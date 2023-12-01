namespace Library.Models.ViewModels
{
    public class OrderVM
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public double Total { get; set; }
    }
}
