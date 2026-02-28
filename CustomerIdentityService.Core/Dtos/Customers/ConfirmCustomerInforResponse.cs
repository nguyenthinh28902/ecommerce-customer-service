namespace CustomerIdentityService.Core.Dtos.Customers
{
    public class ConfirmCustomerInforResponse
    {
        public ConfirmCustomerInforResponse() { }
        public int Id { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

    }
}
