using System.ComponentModel.DataAnnotations;

namespace Flashcards.Common.DTO.Flashcard
{
	public class FlashcardUpdateRequest
	{
		[Required]
		[StringLength(100)]
		public string? MainSide { get; set; }

		[Required]
		[StringLength(500)]
		public string? OppositeSide { get; set; }
	}
}
