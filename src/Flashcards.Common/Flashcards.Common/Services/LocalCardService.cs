using Flashcards.Common.CustomExceptions;
using Flashcards.Common.DTO.Flashcard;
using Flashcards.Common.DTO.Flashcard.API;
using Flashcards.Common.Entities;
using Flashcards.Common.Extensions;
using Flashcards.Common.RepositoryContracts;
using Flashcards.Common.ServiceContracts;
using FlashCards.Common.Helpers;

namespace Flashcards.Common.Services
{
	public class LocalCardService : ILocalCardService
	{
		private readonly ICardRepository _cardRepository;
		private readonly IApiRepository _apiRepository;
		private readonly int _maxResponseQuality;

		public LocalCardService(
				ICardRepository cardRepository,
				IApiRepository apiRepository,
				int maxResponseQuality = 5
			)
		{
			_cardRepository = cardRepository;
			_apiRepository = apiRepository;
			_maxResponseQuality = maxResponseQuality;
		}

		public async Task Create(FlashcardAddRequest? flashcardAddRequest)
		{
			await ValidationHelper.ValidateObjects(flashcardAddRequest);

			await _cardRepository.CreateAsync(new Flashcard
			{
				CardId = Guid.NewGuid(),
				EFactor = 2.5f,
				MainSide = flashcardAddRequest.MainSide,
				OppositeSide = flashcardAddRequest.OppositeSide,
				NextRepeatDate = DateTime.Now.ToDateOnly(),
				RepetitionCount = 0
			});
		}

		public async Task Delete(Guid? cardId)
		{
			await ValidationHelper.ValidateObjects(cardId);

			if (!await _cardRepository.Exists(cardId!.Value))
			{
				throw new ArgumentException("Card id is invalid.");
			}

			await _cardRepository.DeleteAsync(cardId.Value);
		}

		public async Task<IEnumerable<FlashcardResponse>> GetAll()
		{
			var retrieved = await _cardRepository.GetAllAsync(temp => true);

			if (retrieved is null)
			{
				throw new InvalidOperationException("Unable to retrieve all cards.");
			}

			return retrieved.Select(temp => new FlashcardResponse()
			{
				CardId = temp.CardId,
				MainSide = temp.MainSide,
				OppositeSide = temp.OppositeSide,
			});
		}

		public async Task<FlashcardResponse> GetById(Guid? cardId)
		{
			await ValidationHelper.ValidateObjects(cardId);

			var found = await _cardRepository.GetByIdAsync(cardId!.Value);

			if (found is null)
			{
				throw new ArgumentException("The card id is invalid.");
			}

			return new FlashcardResponse()
			{
				CardId = found.CardId,
				MainSide = found.MainSide,
				OppositeSide = found.OppositeSide,
			};
		}

		public async Task<IEnumerable<FlashcardResponse>> GetCardsToReview()
		{
			var retrieved = await _cardRepository.GetAllAsync(temp => temp.NextRepeatDate <= DateTime.Now.ToDateOnly());

			if (retrieved is null)
			{
				throw new InvalidOperationException("Unable to retrieve cards to review.");
			}

			return retrieved.Select(temp => new FlashcardResponse()
			{
				CardId = temp.CardId,
				MainSide = temp.MainSide,
				OppositeSide = temp.OppositeSide,
			});
		}

		public async Task Review(Guid? cardId, int? responseQuality)
		{
			await ValidationHelper.ValidateObjects(cardId, responseQuality);

			var card = await _cardRepository.GetByIdAsync(cardId!.Value);

			if (card is null)
			{
				throw new ArgumentException("Card id is invalid.");
			}

			card!.RepetitionCount++;

			await UpdateInterRepetitionInterval(card);
			await UpdateEFactor(card, responseQuality!.Value);

			if (responseQuality < 3)
			{
				card.RepetitionCount = 0;
				card.NextRepeatDate = DateTime.Now.ToDateOnly();
			}
		}

		public async Task SyncWithRemote()
		{
			var localCards = await _cardRepository.GetAllAsync(temp => true);
			var flashcardApiRequests = localCards.Select(temp => new FlashcardApiRequest()
			{
				CardId = temp.CardId,
				EFactor = temp.EFactor,
				MainSide = temp.MainSide,
				OppositeSide = temp.OppositeSide,
				NextRepeatDate = temp.NextRepeatDate,
				RepetitionCount = temp.RepetitionCount,
				WhetherToDelete = _cardRepository.ContainsMarkedAsDeleted(temp)
			});

			var syncResult = await _apiRepository.SyncAndGetCards(flashcardApiRequests);

			if (syncResult is null)
			{
				throw new FailedSyncException("Failed to sync flashcards.");
			}

			await _cardRepository.ReplaceAllAsync(syncResult.Select(temp => new Flashcard()
			{
				CardId = temp.CardId,
				EFactor = temp.EFactor,
				MainSide = temp.MainSide,
				OppositeSide = temp.OppositeSide,
				NextRepeatDate = temp.NextRepeatDate,
				RepetitionCount = temp.RepetitionCount
			}));
		}

		public async Task Update(Guid? cardId, FlashcardUpdateRequest? flashcardUpdateRequest)
		{
			await ValidationHelper.ValidateObjects(cardId, flashcardUpdateRequest);

			if (!await _cardRepository.Exists(cardId!.Value))
			{
				throw new ArgumentException("Card id is not valid.");
			}

			await _cardRepository.UpdateAsync(cardId!.Value, flashcardUpdateRequest!.MainSide!, flashcardUpdateRequest!.OppositeSide!);
		}

		private async Task UpdateEFactor(Flashcard flashcard, int responseQuality)
		{
			float newEFactor = flashcard.EFactor + (0.1f - (_maxResponseQuality - responseQuality) * (0.08f + (_maxResponseQuality - responseQuality) * 0.02f));

			flashcard.EFactor = Math.Max(1.3f, newEFactor);

			await Task.CompletedTask;
		}

		private async Task UpdateInterRepetitionInterval(Flashcard flashcard)
		{
			if (flashcard.RepetitionCount == 1)
			{
				flashcard.NextRepeatDate = DateTime.Now.AddDays(1).ToDateOnly();
			}
			else if (flashcard.RepetitionCount == 2)
			{
				flashcard.NextRepeatDate = DateTime.Now.AddDays(6).ToDateOnly();
			}
			else
			{
				DateTime currentDate = DateTime.Now;
				DateTime nextRepeatDateTime = DateTime.Parse(flashcard.NextRepeatDate.ToString());

				TimeSpan timeDifference = nextRepeatDateTime - currentDate;

				double daysToAdd = timeDifference.TotalDays * flashcard.EFactor;

				flashcard.NextRepeatDate = currentDate.AddDays(daysToAdd).ToDateOnly();
			}

			await Task.CompletedTask;
		}
	}
}
