using Flashcards.Common.Entities;
using System.Linq.Expressions;

namespace Flashcards.Common.RepositoryContracts
{
	public interface ICardRepository
	{
		Task CreateAsync(Flashcard flashcard);

		Task UpdateAsync(Guid cardId, Flashcard flashcard);

		Task DeleteAsync(Guid cardId);

		Task ReplaceAllAsync(IEnumerable<Flashcard> replacement);

		Task<Flashcard?> GetByIdAsync(Guid cardId);

		Task<IEnumerable<Flashcard>?> GetAllAsync(Expression<Func<Flashcard, bool>> expression);
	}
}
