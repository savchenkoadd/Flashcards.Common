﻿using System.ComponentModel.DataAnnotations;

namespace Flashcards.Common.DTO.Flashcard.API
{
    public class FlashcardApiRequest
    {
        [Required]
        public Guid CardId { get; set; }

        [Required]
        [StringLength(100)]
        public string? MainSide { get; set; }

        [Required]
        [StringLength(500)]
        public string? OppositeSide { get; set; }

        [Required]
        [Range(0, float.MaxValue)]
        public float EFactor { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int RepetitionCount { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateOnly NextRepeatDate { get; set; }

        public bool WhetherToDelete { get; set; }
    }
}
