using System.ComponentModel.DataAnnotations;

namespace WebApplication1.ViewModels
{
	public class Changepass
	{
		[Required(ErrorMessage = "Current Password is required")]
		[MinLength(12, ErrorMessage = "Password must be at least 12 characters long")]
		[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{12,}$", ErrorMessage = "Password must include lowercase, uppercase, number, and special character")]
		[DataType(DataType.Password)]
		public string currPass { get; set; }


		[Required(ErrorMessage = "New Password is required")]
		[MinLength(12, ErrorMessage = "Password must be at least 12 characters long")]
		[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{12,}$", ErrorMessage = "Password must include lowercase, uppercase, number, and special character")]
		[DataType(DataType.Password)]
		public string newPass { get; set; }

		[Required(ErrorMessage = "Confirm Password is required")]
		[Compare(nameof(newPass), ErrorMessage = "Password and confirmation password do not match")]
		[DataType(DataType.Password)]
		public string cfmPass { get; set; }
	}
}
