using System.ComponentModel.DataAnnotations;

namespace WebApplication1.ViewModels
{
    public class Register
    {
        [Required(ErrorMessage = "First Name is required")]
        [DataType(DataType.Text)]
        public string FName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [DataType(DataType.Text)]
        public string LName { get; set; }

        [Required(ErrorMessage = "Credit Card No is required")]
        [RegularExpression(@"^\d{16}$", ErrorMessage = "Credit Card No must be a 16-digit number")]
        [DataType(DataType.CreditCard)]
        public string CCNo { get; set; }

        [Required(ErrorMessage = "Mobile No is required")]
        [RegularExpression(@"^[0-9]{8}$", ErrorMessage = "Mobile No must be a 8-digit number")]
        [DataType(DataType.PhoneNumber)]
        public string MobileNo { get; set; }

        [Required(ErrorMessage = "Billing Address is required")]
        [DataType(DataType.Text)]
        public string BillingAddress { get; set; }
        
        //allow all special characters
        [Required(ErrorMessage = "Shipping Address is required")]
        [DataType(DataType.Text)]
        [RegularExpression(@"[\s\S]*", ErrorMessage = "Invalid characters in Shipping Address")]
        public string ShippingAddress { get; set; }


        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }


        //password complexity check
        //password feedback
        [Required(ErrorMessage = "Password is required")]
        [MinLength(12, ErrorMessage = "Password must be at least 12 characters long")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{12,}$", ErrorMessage = "Password must include lowercase, uppercase, number, and special character")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare(nameof(Password), ErrorMessage = "Password and confirmation password do not match")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }


        //jpg only 
        public IFormFile Photo { get; set; } // Add a property to accept the uploaded photo file
    }   
}
