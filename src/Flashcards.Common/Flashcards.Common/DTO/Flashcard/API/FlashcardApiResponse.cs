using System.ComponentModel.DataAnnotations;

namespace Flashcards.Common.DTO.Flashcard.API
{
    public class FlashcardApiResponse
    {
        public Guid CardId { get; set; }

        public string? MainSide { get; set; }

        public string? OppositeSide { get; set; }

        public float EFactor { get; set; }

        public int RepetitionCount { get; set; }

        public DateOnly NextRepeatDate { get; set; }
    }
}
