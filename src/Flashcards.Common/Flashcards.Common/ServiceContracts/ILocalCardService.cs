using Flashcards.Common.DTO.Flashcard;
using Flashcards.Common.Entities;

namespace Flashcards.Common.ServiceContracts
{
	public interface ILocalCardService
	{
		Task Review(Guid? cardId, int? responseQuality);

		Task Create(FlashcardAddRequest? flashcardAddRequest);

		Task Update(Guid? cardId, FlashcardUpdateRequest? flashcardUpdateRequest);

		Task Delete(Guid? cardId);

		Task SyncWithRemote();

		Task<FlashcardResponse> GetById(Guid? cardId);

		Task<IEnumerable<FlashcardResponse>> GetCardsToReview();

		Task<IEnumerable<FlashcardResponse>> GetAll();
	}
}
