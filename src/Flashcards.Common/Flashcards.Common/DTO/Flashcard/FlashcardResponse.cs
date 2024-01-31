namespace Flashcards.Common.DTO.Flashcard
{
	public class FlashcardResponse
	{
		public Guid CardId { get; set; }

		public string? MainSide { get; set; }

		public string? OppositeSide { get; set; }
	}
}
