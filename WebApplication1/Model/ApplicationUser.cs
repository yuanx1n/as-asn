using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Model
{
    public class ApplicationUser:IdentityUser
    {
        public string? FirstName {  get; set; }
        public string? LastName {  get; set; }
        public string? CreditCardNo {  get; set; }
        public string? MobileNo {  get; set; }
        public string? BillingAddress {  get; set; }
        public string? ShippingAddress {  get; set; }
        public byte[]? Pfp { get; set; } // Add property to store user's photo
    }
}
