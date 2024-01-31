using Flashcards.Common.Entities;
using Flashcards.Common.RepositoryContracts;
using System.Linq.Expressions;

namespace Flashcards.Common.Repositories
{
	public class ListCardRepository : ICardRepository
	{
		private List<Flashcard> _cards;
		private List<Flashcard> _cardsToDelete;

        public ListCardRepository()
        {
            _cards = new List<Flashcard>();
			_cardsToDelete = new List<Flashcard>();
        }

		public bool ContainsMarkedAsDeleted(Flashcard flashcard)
		{
			return _cardsToDelete.Contains(flashcard);
		}

		public async Task CreateAsync(Flashcard flashcard)
		{
			_cards.Add(flashcard);

			await Task.CompletedTask;
		}

		public async Task DeleteAsync(Guid cardId)
		{
			_cardsToDelete.Add(await GetByIdAsync(cardId));
			_cards.RemoveAll(temp => temp.CardId == cardId);

			await Task.CompletedTask;
		}

		public async Task<bool> Exists(Guid cardId)
		{
			return await Task.FromResult(_cards.Any(temp => temp.CardId == cardId));
		}

		public async Task<IEnumerable<Flashcard>?> GetAllAsync(Expression<Func<Flashcard, bool>> expression)
		{
			return await Task.FromResult(_cards.Where(expression.Compile()));
		}

		public async Task<Flashcard?> GetByIdAsync(Guid cardId)
		{
			return await Task.FromResult(_cards.FirstOrDefault(temp => temp.CardId == cardId));
		}

		public async Task ReplaceAllAsync(IEnumerable<Flashcard> replacement)
		{
			_cards = replacement.ToList();

			await Task.CompletedTask;
		}

		public async Task UpdateAsync(Guid cardId, string mainSide, string oppositeSide)
		{
			var found = _cards.FirstOrDefault(temp => temp.CardId == cardId);

			found.MainSide = mainSide;
			found.OppositeSide = oppositeSide;

			await Task.CompletedTask;
		}
	}
}
