using Flashcards.Common.Entities;
using System.Linq.Expressions;

namespace Flashcards.Common.RepositoryContracts
{
	public interface ICardRepository
	{
		bool ContainsMarkedAsDeleted(Flashcard flashcard);

		Task CreateAsync(Flashcard flashcard);

		Task UpdateAsync(Guid cardId, string mainSide, string oppositeSide);

		Task DeleteAsync(Guid cardId);

		Task ReplaceAllAsync(IEnumerable<Flashcard> replacement);

		Task<bool> Exists(Guid cardId);

		Task<Flashcard?> GetByIdAsync(Guid cardId);

		Task<IEnumerable<Flashcard>?> GetAllAsync(Expression<Func<Flashcard, bool>> expression);
	}
}
