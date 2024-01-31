using Flashcards.Common.Extensions;

namespace Flashcards.Common.Entities
{
	public class Flashcard
	{
		public Guid CardId { get; set; }

		public float EFactor { get; set; }

		public int RepetitionCount { get; set; }

		public DateOnly NextRepeatDate { get; set; }

		public string? MainSide { get; set; }

		public string? OppositeSide { get; set; }
	}
}
