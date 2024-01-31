using System.ComponentModel.DataAnnotations;

namespace Flashcards.Common.DTO.Identity
{
	public class RegisterDTO
	{
		[Required]
		[StringLength(20)]
		public string? PersonName { get; set; }

		[Required]
		[EmailAddress]
		[StringLength(50)]
		public string? Email { get; set; }

		[Required]
		[StringLength(15)]
		[Phone]
		public string? PhoneNumber { get; set; }

		[Required]
		public string? Password { get; set; }

		[Required]
		[Compare(nameof(Password))]
		public string? ConfirmPassword { get; set; }
	}
}
