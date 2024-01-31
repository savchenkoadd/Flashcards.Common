using Flashcards.Common.Entities;
using Flashcards.Common.RepositoryContracts;
using System.Linq.Expressions;

namespace Flashcards.Common.Repositories
{
	public class ListCardRepository : ICardRepository
	{
		private List<Flashcard> _repository;

        public ListCardRepository()
        {
            _repository = new List<Flashcard>();
        }

        public async Task CreateAsync(Flashcard flashcard)
		{
			_repository.Add(flashcard);

			await Task.CompletedTask;
		}

		public async Task DeleteAsync(Guid cardId)
		{
			_repository.RemoveAll(temp => temp.CardId == cardId);

			await Task.CompletedTask;
		}

		public async Task<IEnumerable<Flashcard>?> GetAllAsync(Expression<Func<Flashcard, bool>> expression)
		{
			return await Task.FromResult(_repository.Where(expression.Compile()));
		}

		public async Task<Flashcard?> GetByIdAsync(Guid cardId)
		{
			return await Task.FromResult(_repository.FirstOrDefault(temp => temp.CardId == cardId));
		}

		public async Task ReplaceAllAsync(IEnumerable<Flashcard> replacement)
		{
			_repository = replacement.ToList();

			await Task.CompletedTask;
		}

		public async Task UpdateAsync(Guid cardId, Flashcard flashcard)
		{
			await DeleteAsync(cardId);
			_repository.Add(flashcard);

			await Task.CompletedTask;
		}
	}
}
